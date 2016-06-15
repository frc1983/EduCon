using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using EduCon.Aplicacao.Interfaces;
using EduCon.ImportaFee.Objetos;
using EduCon.Objetos.DTOs;
using EduCon.Utilitarios.Conversores;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;

namespace EduCon.ImportaFee.Infra
{
    public class Executor
    {
        private IList<Dado> dados;
        private IList<TipoEnsinoDTO> tiposEnsino;
        private IList<CategoriaDTO> categorias;
        private IList<MunicipioDTO> municipios;
        private IList<DataDTO> datas;

        private IMunicipioAplServico _municipioServico;
        private IDataAplServico _dataServico;
        private ITipoEnsinoAplServico _tipoEnsinoServico;
        private ICategoriaAplServico _categoriaServico;
        private IDadoAplServico _dadoServico;

        public Executor()
        {
            _municipioServico = ServiceLocator.Current.GetInstance<IMunicipioAplServico>();
            _dataServico = ServiceLocator.Current.GetInstance<IDataAplServico>();
            _tipoEnsinoServico = ServiceLocator.Current.GetInstance<ITipoEnsinoAplServico>();
            _categoriaServico = ServiceLocator.Current.GetInstance<ICategoriaAplServico>();
            _dadoServico = ServiceLocator.Current.GetInstance<IDadoAplServico>();
        }

        public void Executa(bool copia)
        {
            try
            {
                ValidaDiretorio();

                var diretorioProcessamento = new DirectoryInfo(AppContext.BaseDirectory + @"processamento\");
                if (!diretorioProcessamento.Exists)
                {
                    diretorioProcessamento.Create();
                }

                var qtdArquivos = diretorioProcessamento.GetFiles().Count();

                if (copia)
                {
                    if (qtdArquivos > 0)
                    {
                        var resposta = string.Empty;
                        do
                        {
                            Console.WriteLine(DateTime.Now.ToString() + " - Diretório de processamento já possui {0} arquivos carregados.", qtdArquivos);
                            Console.Write(@"Deseja excluir os arquivos e realizar novo carregamento? (S/N) \> ");
                            resposta = Console.ReadLine().ToLower();
                        } while (!resposta.Equals("s") && !resposta.Equals("n"));

                        var apaga = resposta.Equals("s");
                        if (apaga)
                        {
                            foreach (var arquivo in diretorioProcessamento.GetFiles())
                            {
                                arquivo.Delete();
                            }
                        }
                    }

                    CopiaArquivosProcessamento(diretorioProcessamento);
                }

                var atualArquivo = 1;
                var totalArquivos = diretorioProcessamento.GetFiles().Count();

                Console.WriteLine(DateTime.Now.ToString() + " - Arquivos encontrados: " + totalArquivos);
                foreach (var arquivo in diretorioProcessamento.GetFiles())
                {
                    try
                    {
                        ValidaArquivo(arquivo);

                        Console.WriteLine(DateTime.Now.ToString() + " - Processando arquivo {0} de {1}: {2}", atualArquivo, totalArquivos, arquivo.Name);

                        dados = new List<Dado>();
                        tiposEnsino = new List<TipoEnsinoDTO>();
                        categorias = new List<CategoriaDTO>();
                        municipios = new List<MunicipioDTO>();
                        datas = new List<DataDTO>();

                        if (arquivo.Extension.Equals(".json"))
                        {
                            CarregaDadosJson(arquivo);
                        }

                        if (arquivo.Extension.Equals(".csv"))
                        {
                            CarregaDadosCsv(arquivo);
                        }

                        IncluiDados();

                        ExcluirArquivo(arquivo);

                        atualArquivo++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format(DateTime.Now.ToString() + " - Erro ao processar arquivo {0}:", arquivo.Name));
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CopiaArquivosProcessamento(DirectoryInfo diretorioProcessamento)
        {
            var diretorioFonte = new DirectoryInfo(ConfigManager.DiretorioArquivos);
            foreach (var arquivo in diretorioFonte.GetFiles())
            {
                try
                {
                    arquivo.CopyTo(Path.Combine(diretorioProcessamento.FullName, arquivo.Name), false);
                }
                catch (IOException ioEx)
                {
                    Console.WriteLine(DateTime.Now.ToString() + " - Arquivo {0} já existente na pasta", arquivo.Name);
                }
            }
        }

        private void ValidaDiretorio()
        {
            var pasta = new DirectoryInfo(ConfigManager.DiretorioArquivos);
            if (!pasta.Exists)
            {
                throw new Exception("Diretório de arquivos não encontrado.");
            }

            if (!pasta.GetFiles().Any())
            {
                throw new Exception("Nenhum arquivo encontrado no diretório informado.");
            }
        }

        private void ValidaArquivo(FileInfo arquivo)
        {
            if (!arquivo.Extension.Equals(".json") && !arquivo.Extension.Equals(".csv"))
            {
                throw new Exception(string.Format("Extensão {0} não é válida para esta importação.", arquivo.Extension));
            }

            if (arquivo.Length == 0)
            {
                throw new Exception("O arquivo está vazio.");
            }
        }

        private void CarregaDadosJson(FileInfo arquivo)
        {
            var stream = UTF8.Converte(arquivo.FullName);
            var obj = JsonConvert.DeserializeObject<Arquivo>(stream);

            var variavel = obj.Variavel.First();

            foreach (var unidadeGeografica in obj.UnidadesGeograficas)
            {
                MunicipioDTO Municipio = null;
                TipoEnsinoDTO TipoEnsino = null;
                CategoriaDTO Categoria = null;
                CategoriaDTO Subcategoria = null;
                DataDTO Data = null;

                #region Município

                if (Municipio == null || Municipio.CodIBGE != int.Parse(unidadeGeografica.Ibge))
                {
                    Municipio = new MunicipioDTO();
                    var munic = municipios.Where(o => o.CodIBGE == int.Parse(unidadeGeografica.Ibge)).FirstOrDefault();
                    if (munic == null)
                    {
                        munic = _municipioServico.Lista(new MunicipioDTO() { CodIBGE = int.Parse(unidadeGeografica.Ibge) }).FirstOrDefault();
                    }

                    if (munic == null)
                    {
                        var municipio = new MunicipioDTO()
                        {
                            CodIBGE = int.Parse(unidadeGeografica.Ibge),
                            Nome = unidadeGeografica.Nome.Trim(),
                            Latitude = decimal.Parse(unidadeGeografica.Latitude),
                            Longitude = decimal.Parse(unidadeGeografica.Longitude)
                        };

                        municipios.Add(municipio);
                        Municipio = municipio;
                    }
                    else
                    {
                        Municipio = munic;
                    }
                }

                #endregion

                var caminho = variavel.Caminho.Split('/');

                #region Tipo de Ensino

                var tipoEnsinoDescr = (caminho.Length > 3 ? caminho[2] : string.Empty).Trim();

                if (TipoEnsino == null || !TipoEnsino.Nome.Equals(tipoEnsinoDescr))
                {
                    TipoEnsino = new TipoEnsinoDTO();
                    if (!string.IsNullOrEmpty(tipoEnsinoDescr))
                    {
                        var tipoEns = tiposEnsino.Where(o => o.Nome.Equals(tipoEnsinoDescr)).FirstOrDefault();
                        if (tipoEns == null)
                        {
                            tipoEns = _tipoEnsinoServico.Lista(new TipoEnsinoDTO() { Nome = tipoEnsinoDescr }).FirstOrDefault();
                        }

                        if (tipoEns == null)
                        {
                            var tipoEnsino = new TipoEnsinoDTO()
                            {
                                Nome = tipoEnsinoDescr
                            };

                            tiposEnsino.Add(tipoEnsino);
                            TipoEnsino = tipoEnsino;
                        }
                        else
                        {
                            TipoEnsino = tipoEns;
                        }
                    }
                }

                #endregion

                #region Categoria

                var categoriaDescr = (caminho.Length > 4 ? caminho[3] : string.Empty).Trim();

                if (Categoria == null || !Categoria.Nome.Equals(categoriaDescr))
                {
                    Categoria = new CategoriaDTO();
                    if (!string.IsNullOrEmpty(categoriaDescr))
                    {
                        var categ = categorias.Where(o => o.Nome.Equals(categoriaDescr)).FirstOrDefault();
                        if (categ == null)
                        {
                            categ = _categoriaServico.Lista(new CategoriaDTO() { Nome = categoriaDescr }).FirstOrDefault();
                        }

                        if (categ == null)
                        {
                            var categoria = new CategoriaDTO()
                            {
                                Nome = categoriaDescr
                            };

                            categorias.Add(categoria);
                            Categoria = categoria;
                        }
                        else
                        {
                            Categoria = categ;
                        }
                    }
                }

                var subcategoriaDescr = string.Empty;
                if (caminho.Length == 5)
                {
                    subcategoriaDescr = caminho[4].Trim();
                }
                else if (caminho.Length > 5)
                {
                    subcategoriaDescr = caminho[4].Trim() + ' ' + caminho[5].Trim();
                }

                if (Subcategoria == null || !Subcategoria.Nome.Equals(subcategoriaDescr))
                {
                    Subcategoria = new CategoriaDTO();
                    if (!string.IsNullOrEmpty(subcategoriaDescr))
                    {
                        var subcateg = categorias.Where(o => o.Nome.Equals(subcategoriaDescr)).FirstOrDefault();
                        if (subcateg == null)
                        {
                            subcateg = _categoriaServico.Lista(new CategoriaDTO() { Nome = subcategoriaDescr }).FirstOrDefault();
                        }

                        if (subcateg == null)
                        {
                            var subcategoria = new CategoriaDTO()
                            {
                                Nome = subcategoriaDescr
                            };

                            categorias.Add(subcategoria);
                            Subcategoria = subcategoria;
                        }
                        else
                        {
                            Subcategoria = subcateg;
                        }
                    }
                }

                #endregion

                foreach (var valor in unidadeGeografica.Valores)
                {
                    #region Ano

                    if (Data == null || Data.Ano != int.Parse(valor.Ano))
                    {
                        Data = new DataDTO();

                        var dt = datas.Where(o => o.Ano == int.Parse(valor.Ano)).FirstOrDefault();
                        if (dt == null)
                        {
                            dt = _dataServico.Lista(new DataDTO() { Ano = int.Parse(valor.Ano) }).FirstOrDefault();
                        }

                        if (dt == null)
                        {
                            var data = new DataDTO()
                            {
                                Ano = int.Parse(valor.Ano)
                            };

                            datas.Add(data);
                            Data = data;
                        }
                        else
                        {
                            Data = dt;
                        }
                    }

                    #endregion

                    var existe = dados.Any(o => o.Municipio.Nome == Municipio.Nome
                        && o.TipoEnsino.Nome == TipoEnsino.Nome
                        && o.Categoria.Nome == Categoria.Nome
                        && o.Subcategoria.Nome == Subcategoria.Nome
                        && o.Data.Ano == Data.Ano);

                    if (!existe)
                    {
                        if (Municipio.Id != 0
                            && TipoEnsino.Id != 0
                            && Categoria.Id != 0
                            && Subcategoria.Id != 0
                            && Data.Id != 0)
                        {
                            var d = new DadoDTO()
                            {
                                IdMunicipio = Municipio.Id,
                                IdTipoEnsino = TipoEnsino.Id,
                                IdCategoria = Categoria.Id,
                                IdSubcategoria = Subcategoria.Id,
                                IdData = Data.Id
                            };

                            existe = _dadoServico.Lista(d).Any();
                        }
                    }

                    if (!existe)
                    {
                        var dado = new Dado()
                        {
                            TipoEnsino = TipoEnsino,
                            Categoria = Categoria,
                            Subcategoria = Subcategoria,
                            Municipio = Municipio,
                            Data = Data,
                            Valor = valor.Valor
                        };

                        dados.Add(dado);
                    }
                }
            }
        }

        private void CarregaDadosCsv(FileInfo arquivo)
        {
            MunicipioDTO Municipio = null;
            TipoEnsinoDTO TipoEnsino = null;
            CategoriaDTO Categoria = null;
            CategoriaDTO Subcategoria = null;
            DataDTO Data = null;

            var ordemAnos = new List<DataDTO>();

            using (var reader = new StreamReader(arquivo.FullName, Encoding.Default))
            {
                while (!reader.EndOfStream)
                {
                    var linha = reader.ReadLine();

                    //variavel: Estadual
                    //caminho: / Educação / Educação Infantil / Funções Docentes / Estadual
                    //fonte: Secretaria de Educação do Rio Grande do Sul.
                    //agrupador: Município
                    //descricao: 


                    if (linha.StartsWith("variavel:")
                        || linha.StartsWith("agrupador:")
                        || linha.StartsWith("descricao:"))
                    {
                        continue;
                    }

                    if (linha.StartsWith("caminho:"))
                    {
                        var registro = linha.Replace("caminho:", "").Trim();
                        var caminho = registro.Split('/');

                        #region Tipo de Ensino

                        var tipoEnsinoDescr = (caminho.Length > 3 ? caminho[2] : string.Empty).Trim();

                        if (TipoEnsino == null || !TipoEnsino.Nome.Equals(tipoEnsinoDescr))
                        {
                            TipoEnsino = new TipoEnsinoDTO();
                            if (!string.IsNullOrEmpty(tipoEnsinoDescr))
                            {
                                var tipoEns = tiposEnsino.Where(o => o.Nome.Equals(tipoEnsinoDescr)).FirstOrDefault();
                                if (tipoEns == null)
                                {
                                    tipoEns = _tipoEnsinoServico.Lista(new TipoEnsinoDTO() { Nome = tipoEnsinoDescr }).FirstOrDefault();
                                }

                                if (tipoEns == null)
                                {
                                    var tipoEnsino = new TipoEnsinoDTO()
                                    {
                                        Nome = tipoEnsinoDescr
                                    };

                                    tiposEnsino.Add(tipoEnsino);
                                    TipoEnsino = tipoEnsino;
                                }
                                else
                                {
                                    TipoEnsino = tipoEns;
                                }
                            }
                        }

                        #endregion

                        #region Categoria

                        var categoriaDescr = (caminho.Length > 4 ? caminho[3] : string.Empty).Trim();

                        if (Categoria == null || !Categoria.Nome.Equals(categoriaDescr))
                        {
                            Categoria = new CategoriaDTO();
                            if (!string.IsNullOrEmpty(categoriaDescr))
                            {
                                var categ = categorias.Where(o => o.Nome.Equals(categoriaDescr)).FirstOrDefault();
                                if (categ == null)
                                {
                                    categ = _categoriaServico.Lista(new CategoriaDTO() { Nome = categoriaDescr }).FirstOrDefault();
                                }

                                if (categ == null)
                                {
                                    var categoria = new CategoriaDTO()
                                    {
                                        Nome = categoriaDescr
                                    };

                                    categorias.Add(categoria);
                                    Categoria = categoria;
                                }
                                else
                                {
                                    Categoria = categ;
                                }
                            }
                        }

                        var subcategoriaDescr = string.Empty;
                        if (caminho.Length == 5)
                        {
                            subcategoriaDescr = caminho[4].Trim();
                        }
                        else if (caminho.Length > 5)
                        {
                            subcategoriaDescr = caminho[4].Trim() + ' ' + caminho[5].Trim();
                        }

                        if (Subcategoria == null || !Subcategoria.Nome.Equals(subcategoriaDescr))
                        {
                            Subcategoria = new CategoriaDTO();
                            if (!string.IsNullOrEmpty(subcategoriaDescr))
                            {
                                var subcateg = categorias.Where(o => o.Nome.Equals(subcategoriaDescr)).FirstOrDefault();
                                if (subcateg == null)
                                {
                                    subcateg = _categoriaServico.Lista(new CategoriaDTO() { Nome = subcategoriaDescr }).FirstOrDefault();
                                }

                                if (subcateg == null)
                                {
                                    var subcategoria = new CategoriaDTO()
                                    {
                                        Nome = subcategoriaDescr
                                    };

                                    categorias.Add(subcategoria);
                                    Subcategoria = subcategoria;
                                }
                                else
                                {
                                    Subcategoria = subcateg;
                                }
                            }
                        }

                        #endregion

                        continue;
                    }

                    if (linha.StartsWith("fonte:"))
                    {
                        continue;
                    }

                    if (linha.StartsWith("Município"))
                    {
                        var cabecalho = linha.Split(';');
                        for (int i = 4; i < cabecalho.Length; i++)
                        {
                            var valorAno = cabecalho[i].Replace("\"", "");

                            var match = Regex.Match(cabecalho[i], @"[0-9]{4}");
                            if (match.Success)
                            {
                                #region Ano

                                var ano = match.Value;

                                if (Data == null || Data.Ano != int.Parse(ano))
                                {
                                    Data = new DataDTO();

                                    var dt = datas.Where(o => o.Ano == int.Parse(ano)).FirstOrDefault();
                                    if (dt == null)
                                    {
                                        dt = _dataServico.Lista(new DataDTO() { Ano = int.Parse(ano) }).FirstOrDefault();
                                    }

                                    if (dt == null)
                                    {
                                        var data = new DataDTO()
                                        {
                                            Ano = int.Parse(ano)
                                        };

                                        datas.Add(data);
                                        Data = data;
                                    }
                                    else
                                    {
                                        Data = dt;
                                    }

                                    ordemAnos.Add(Data);
                                }

                                #endregion
                            }
                        }

                        continue;
                    }

                    var registroMunicipio = linha.Split(';');

                    var codIbge = int.Parse(registroMunicipio[1]);

                    #region Município

                    if (Municipio == null || Municipio.CodIBGE != codIbge)
                    {
                        Municipio = new MunicipioDTO();
                        var munic = municipios.Where(o => o.CodIBGE == int.Parse(registroMunicipio[1])).FirstOrDefault();
                        if (munic == null)
                        {
                            munic = _municipioServico.Lista(new MunicipioDTO() { CodIBGE = codIbge }).FirstOrDefault();
                        }

                        if (munic == null)
                        {
                            var municipio = new MunicipioDTO()
                            {
                                CodIBGE = codIbge,
                                Nome = registroMunicipio[0].Trim(),
                                Latitude = decimal.Parse(registroMunicipio[2]),
                                Longitude = decimal.Parse(registroMunicipio[3])
                            };

                            municipios.Add(municipio);
                            Municipio = municipio;
                        }
                        else
                        {
                            Municipio = munic;
                        }
                    }

                    #endregion

                    #region Dados

                    for (int i = 4; i < registroMunicipio.Length; i++)
                    {
                        Data = ordemAnos[i - 4];

                        var existe = dados.Any(o => o.Municipio.Nome == Municipio.Nome
                            && o.TipoEnsino.Nome == TipoEnsino.Nome
                            && o.Categoria.Nome == Categoria.Nome
                            && o.Subcategoria.Nome == Subcategoria.Nome
                            && o.Data.Ano == Data.Ano);

                        if (!existe)
                        {
                            if (Municipio.Id != 0
                                && TipoEnsino.Id != 0
                                && Categoria.Id != 0
                                && Subcategoria.Id != 0
                                && Data.Id != 0)
                            {
                                var d = new DadoDTO()
                                {
                                    IdMunicipio = Municipio.Id,
                                    IdTipoEnsino = TipoEnsino.Id,
                                    IdCategoria = Categoria.Id,
                                    IdSubcategoria = Subcategoria.Id,
                                    IdData = Data.Id
                                };

                                existe = _dadoServico.Existe(d);
                            }
                        }

                        if (!existe)
                        {
                            var dado = new Dado()
                            {
                                TipoEnsino = TipoEnsino,
                                Categoria = Categoria,
                                Subcategoria = Subcategoria,
                                Municipio = Municipio,
                                Data = Data,
                                Valor = registroMunicipio[i].Trim()
                            };

                            dados.Add(dado);
                        }
                    }

                    #endregion
                }

                reader.Close();
            }
        }

        private void IncluiDados()
        {
            var text = string.Empty;
            var count = 1;

            if (municipios.Count > 0)
            {
                text = DateTime.Now.ToString() + " - Municípios a incluir: " + municipios.Count + " | ";
                count = 1;
                Console.Write(text);
                foreach (var municipio in municipios)
                {
                    AtualizaStatus(text.Length, count);
                    _municipioServico.Inclui(municipio);
                    count++;
                }
                Console.WriteLine();
            }

            if (tiposEnsino.Count > 0)
            {
                text = DateTime.Now.ToString() + " - Tipos de ensino a incluir: " + tiposEnsino.Count + " | ";
                count = 1;
                Console.Write(text);
                foreach (var tipoEnsino in tiposEnsino)
                {
                    AtualizaStatus(text.Length, count);
                    _tipoEnsinoServico.Inclui(tipoEnsino);
                    count++;
                }
                Console.WriteLine();
            }

            if (categorias.Count > 0)
            {
                text = DateTime.Now.ToString() + " - Categorias a incluir: " + categorias.Count + " | ";
                count = 1;
                Console.Write(text);
                foreach (var categoria in categorias)
                {
                    AtualizaStatus(text.Length, count);
                    _categoriaServico.Inclui(categoria);
                    count++;
                }
                Console.WriteLine();
            }

            if (datas.Count > 0)
            {
                text = DateTime.Now.ToString() + " - Datas a incluir: " + datas.Count + " | ";
                count = 1;
                Console.Write(text);
                foreach (var data in datas)
                {
                    AtualizaStatus(text.Length, count);
                    _dataServico.Inclui(data);
                    count++;
                }
                Console.WriteLine();
            }

            if (dados.Count > 0)
            {
                var dtos = new List<DadoDTO>();
                foreach (var dado in dados)
                {
                    var d = new DadoDTO()
                    {
                        IdMunicipio = dado.Municipio.Id,
                        IdTipoEnsino = dado.TipoEnsino.Id,
                        IdCategoria = dado.Categoria.Id,
                        IdSubcategoria = dado.Subcategoria.Id,
                        IdData = dado.Data.Id,
                        Valor = dado.Valor
                    };

                    if (d.IdSubcategoria.HasValue && d.IdSubcategoria.Value == 0)
                    {
                        d.IdSubcategoria = null;
                    }

                    dtos.Add(d);
                }

                text = DateTime.Now.ToString() + " - Dados a incluir: " + dtos.Count + " | ";
                count = 0;
                Console.Write(text);
                do
                {
                    AtualizaStatus(text.Length, count);

                    var extrato = dtos.Take(100).ToList();
                    foreach (var dado in extrato)
                    {
                        dtos.Remove(dado);
                    }

                    count += extrato.Count();

                    _dadoServico.Inclui(extrato);
                }
                while (dtos.Count > 0);

                AtualizaStatus(text.Length, count);

                Console.WriteLine();
            }
        }

        private void AtualizaStatus(int tamanho, int quantidade)
        {
            Console.SetCursorPosition(tamanho, Console.CursorTop);
            Console.Write(quantidade);
        }

        private void ExcluirArquivo(FileInfo arquivo)
        {
            arquivo.Delete();
        }
    }
}