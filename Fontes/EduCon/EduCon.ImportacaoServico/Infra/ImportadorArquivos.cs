using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using EduCon.Aplicacao.Interfaces;
using EduCon.ImportacaoServico.Objetos;
using EduCon.Objetos.DTOs;
using EduCon.Utilitarios.Conversores;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;

namespace EduCon.ImportacaoServico.Infra
{
    public class ImportadorArquivos
    {
        private DirectoryInfo dirVariaveis;
        private DirectoryInfo dirArquivos;

        private IList<Dado> dados;
        private IList<FonteDTO> fontes;
        private IList<TipoEnsinoDTO> tiposEnsino;
        private IList<CategoriaDTO> categorias;
        private IList<MunicipioDTO> municipios;
        private IList<DataDTO> datas;

        private IFonteAplServico _fonteServico;
        private IMunicipioAplServico _municipioServico;
        private IDataAplServico _dataServico;
        private ITipoEnsinoAplServico _tipoEnsinoServico;
        private ICategoriaAplServico _categoriaServico;
        private IDadoAplServico _dadoServico;

        public ImportadorArquivos()
        {
            _fonteServico = ServiceLocator.Current.GetInstance<IFonteAplServico>();
            _municipioServico = ServiceLocator.Current.GetInstance<IMunicipioAplServico>();
            _dataServico = ServiceLocator.Current.GetInstance<IDataAplServico>();
            _tipoEnsinoServico = ServiceLocator.Current.GetInstance<ITipoEnsinoAplServico>();
            _categoriaServico = ServiceLocator.Current.GetInstance<ICategoriaAplServico>();
            _dadoServico = ServiceLocator.Current.GetInstance<IDadoAplServico>();

            ObtemDiretorios();
        }

        public void ImportaArquivos(ProcessamentoDTO processamento)
        {
            var arquivo = ImportaVariaveis();
            var variaveis = new List<int>();

            // Obtem ids das variáveis a serem importadas
            foreach (var variavel in arquivo.Variavel)
            {
                if (variavel.Caminho.Contains(processamento.Texto))
                {
                    variaveis.Add(variavel.Id);
                }
            }

            // Limpa pasta de arquivos
            dirArquivos.GetFiles().ToList().ForEach(o => o.Delete());

            // Faz o download e descompacta os arquivos
            foreach (var id in variaveis)
            {
                ObtemArquivo(id, processamento.AnoInicial, processamento.AnoFinal);
            }

            processamento.QtdRegistros = ImportaDados();
        }

        public int ImportaDados()
        {
            var qtdRegistros = 0;

            try
            {
                foreach (var arquivo in dirArquivos.GetFiles())
                {
                    try
                    {
                        ValidaArquivo(arquivo);

                        dados = new List<Dado>();
                        fontes = new List<FonteDTO>();
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

                        qtdRegistros += dados.Count + fontes.Count + tiposEnsino.Count + categorias.Count + municipios.Count + datas.Count;
                    }
                    catch (Exception ex)
                    {
                        //ConsoleWriteLine();
                        //ConsoleExp.WriteLine("Erro ao processar arquivo {0}:", arquivo.Name);
                        //ConsoleWriteLine(ex.Message);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return qtdRegistros;
        }

        private void ObtemDiretorios()
        {
            var diretorioProcessamento = new DirectoryInfo(AppContext.BaseDirectory + @"processamento\");
            if (!diretorioProcessamento.Exists)
            {
                diretorioProcessamento.Create();
                dirVariaveis = diretorioProcessamento.CreateSubdirectory("variaveis");
                dirArquivos = diretorioProcessamento.CreateSubdirectory("arquivos");
            }
            else
            {
                dirVariaveis = diretorioProcessamento.GetDirectories().First(o => o.Name.Equals("variaveis"));
                dirArquivos = diretorioProcessamento.GetDirectories().First(o => o.Name.Equals("arquivos"));
            }
        }

        private ArquivoVariaveis ImportaVariaveis()
        {
            // Limpa diretório de variáveis
            dirVariaveis.GetFiles().ToList().ForEach(o => o.Delete());

            // Busca na url indicada
            var urlVariaveis = Configuracao.UrlVariaveis;
            Download.BaixarEDescompactar(urlVariaveis, dirVariaveis.FullName);
            if (!dirVariaveis.GetFiles().Any())
            {
                throw new Exception("Não foi encontrado o arquivo de variáveis");
            }

            // Recupera o arquivo .json das variáveis
            var arquivoVariaveis = new FileInfo(dirVariaveis.GetFiles().First(o => o.Extension.Equals(".json")).FullName);

            ArquivoVariaveis obj = null;
            using (var reader = new StreamReader(arquivoVariaveis.FullName, Encoding.UTF8))
            {
                var textoArquivo = reader.ReadToEnd();
                obj = JsonConvert.DeserializeObject<ArquivoVariaveis>(textoArquivo);
            }

            ExcluirArquivo(arquivoVariaveis);

            if (obj == null)
            {
                throw new Exception("Nenhuma variável encontrada no arquivo.");
            }

            return obj;
        }

        private void ObtemArquivo(int id, int? anoIni, int? anoFim)
        {
            var paramAno = MontaParametroAno(anoIni, anoFim);
            var url = string.Format(@"{0}/{1}/{2}", Configuracao.UrlArquivos, id, paramAno);

            Download.BaixarEDescompactar(url, dirArquivos.FullName);
        }

        private string MontaParametroAno(int? anoIni, int? anoFim)
        {
            var anos = new List<int>();

            if (!anoIni.HasValue)
            {
                anoIni = 1990;
            }

            if (!anoFim.HasValue)
            {
                anoFim = DateTime.Now.Year;
            }

            if (anoIni.Value > anoFim.Value)
            {
                throw new Exception("Ano inicial não pode ser maior que ano final.");
            }

            for (int i = anoIni.Value; i <= anoFim.Value; i++)
            {
                anos.Add(i);
            }

            var paramAno = string.Join(",", anos);
            if (string.IsNullOrEmpty(paramAno))
            {
                throw new Exception("Ano para pesquisa de arquivos não informado.");
            }

            return paramAno;
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

            #region Fonte

            var Fonte = new FonteDTO();
            if (!string.IsNullOrEmpty(variavel.Fonte.Trim()))
            {
                var font = fontes.Where(o => o.Nome.Equals(variavel.Fonte.Trim())).FirstOrDefault();
                if (font == null)
                {
                    font = _fonteServico.Lista(new FonteDTO() { Nome = variavel.Fonte.Trim() }).FirstOrDefault();
                }

                if (font == null)
                {
                    var fonte = new FonteDTO()
                    {
                        Nome = variavel.Fonte.Trim()
                    };

                    fontes.Add(fonte);
                    Fonte = fonte;
                }
                else
                {
                    Fonte = font;
                }
            }

            #endregion

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

                var tipoEnsinoDescr = (caminho.Length > 4 ? caminho[2] : string.Empty).Trim();

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

                var categoriaDescr = (caminho.Length == 4 ? caminho[2] : (caminho.Length > 4 ? caminho[3] : string.Empty)).Trim();

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

                if (caminho.Length == 4)
                {
                    subcategoriaDescr = caminho[3].Trim();
                }
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

                    #region Dados

                    var existe = dados.Any(o => o.Fonte.Nome == Fonte.Nome
                        && o.Municipio.Nome == Municipio.Nome
                        && o.TipoEnsino.Nome == TipoEnsino.Nome
                        && o.Categoria.Nome == Categoria.Nome
                        && o.Subcategoria.Nome == Subcategoria.Nome
                        && o.Data.Ano == Data.Ano);

                    if (!existe)
                    {
                        if (Fonte.Id != 0
                            && Municipio.Id != 0
                            && TipoEnsino.Id != 0
                            && Categoria.Id != 0
                            && Subcategoria.Id != 0
                            && Data.Id != 0)
                        {
                            var d = new DadoDTO()
                            {
                                IdFonte = Fonte.Id,
                                IdMunicipio = Municipio.Id,
                                IdTipoEnsino = TipoEnsino.Id,
                                IdCategoria = Categoria.Id,
                                IdSubcategoria = Subcategoria.Id,
                                IdData = Data.Id
                            };

                            existe = _dadoServico.Existe(d);

                            if (existe)
                            {
                                var dadoAntigo = _dadoServico.Lista(d).FirstOrDefault();
                                if (dadoAntigo.Valor.Replace(".", "") != valor.Valor.Replace(".", ""))
                                {
                                    dadoAntigo.Valor = valor.Valor.Replace(".", "");
                                    _dadoServico.Altera(dadoAntigo);
                                }
                            }
                        }
                    }

                    if (!existe)
                    {
                        var dado = new Dado()
                        {
                            Fonte = Fonte,
                            TipoEnsino = TipoEnsino,
                            Categoria = Categoria,
                            Subcategoria = Subcategoria,
                            Municipio = Municipio,
                            Data = Data,
                            Valor = valor.Valor.Replace(".", "")
                        };

                        dados.Add(dado);
                    }

                    #endregion
                }
            }
        }

        private void CarregaDadosCsv(FileInfo arquivo)
        {
            FonteDTO Fonte = null;
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
                        var registro = linha.Replace("fonte:", "").Trim();

                        #region Fonte

                        if (Fonte == null || !TipoEnsino.Nome.Equals(registro))
                        {
                            if (!string.IsNullOrEmpty(registro))
                            {
                                var font = fontes.Where(o => o.Nome.Equals(registro)).FirstOrDefault();
                                if (font == null)
                                {
                                    font = _fonteServico.Lista(new FonteDTO() { Nome = registro }).FirstOrDefault();
                                }

                                if (font == null)
                                {
                                    var fonte = new FonteDTO()
                                    {
                                        Nome = registro
                                    };

                                    fontes.Add(fonte);
                                    Fonte = fonte;
                                }
                                else
                                {
                                    Fonte = font;
                                }
                            }
                        }

                        #endregion

                        continue;
                    }

                    if (linha.StartsWith("Município"))
                    {
                        #region Ano

                        var ano = string.Empty;
                        var cabecalho = linha.Split(';');
                        for (int i = 4; i < cabecalho.Length; i++)
                        {
                            var valorAno = cabecalho[i].Replace("\"", "");

                            var match = Regex.Match(cabecalho[i], @"[0-9]{4}");
                            if (match.Success)
                            {
                                ano = match.Value;
                            }
                        }

                        if (string.IsNullOrEmpty(ano))
                        {
                            throw new Exception("Ano não encontrado");
                        }

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

                        var existe = dados.Any(o => o.Fonte.Nome == Fonte.Nome
                            && o.Municipio.Nome == Municipio.Nome
                            && o.TipoEnsino.Nome == TipoEnsino.Nome
                            && o.Categoria.Nome == Categoria.Nome
                            && o.Subcategoria.Nome == Subcategoria.Nome
                            && o.Data.Ano == Data.Ano);

                        if (!existe)
                        {
                            if (Fonte.Id != 0
                                && Municipio.Id != 0
                                && TipoEnsino.Id != 0
                                && Categoria.Id != 0
                                && Subcategoria.Id != 0
                                && Data.Id != 0)
                            {
                                var d = new DadoDTO()
                                {
                                    IdFonte = Fonte.Id,
                                    IdMunicipio = Municipio.Id,
                                    IdTipoEnsino = TipoEnsino.Id,
                                    IdCategoria = Categoria.Id,
                                    IdSubcategoria = Subcategoria.Id,
                                    IdData = Data.Id
                                };

                                existe = _dadoServico.Existe(d);

                                if (existe)
                                {
                                    var dadoAntigo = _dadoServico.Lista(d).FirstOrDefault();
                                    if (dadoAntigo.Valor.Replace(".", "") != registroMunicipio[i].Trim().Replace(".", ""))
                                    {
                                        dadoAntigo.Valor = registroMunicipio[i].Trim().Replace(".", "");
                                        _dadoServico.Altera(dadoAntigo);
                                    }
                                }
                            }
                        }

                        if (!existe)
                        {
                            var dado = new Dado()
                            {
                                Fonte = Fonte,
                                TipoEnsino = TipoEnsino,
                                Categoria = Categoria,
                                Subcategoria = Subcategoria,
                                Municipio = Municipio,
                                Data = Data,
                                Valor = registroMunicipio[i].Trim().Replace(".", "")
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
            foreach (var fonte in fontes)
            {
                _fonteServico.Inclui(fonte);
            }

            foreach (var municipio in municipios)
            {
                _municipioServico.Inclui(municipio);
            }

            foreach (var tipoEnsino in tiposEnsino)
            {
                _tipoEnsinoServico.Inclui(tipoEnsino);
            }

            foreach (var categoria in categorias)
            {
                _categoriaServico.Inclui(categoria);
            }

            foreach (var data in datas)
            {
                _dataServico.Inclui(data);
            }

            if (dados.Count > 0)
            {
                var dtos = new List<DadoDTO>();
                foreach (var dado in dados)
                {
                    var d = new DadoDTO()
                    {
                        IdFonte = dado.Fonte.Id,
                        IdMunicipio = dado.Municipio.Id,
                        IdTipoEnsino = dado.TipoEnsino.Id,
                        IdCategoria = dado.Categoria.Id,
                        IdSubcategoria = dado.Subcategoria.Id,
                        IdData = dado.Data.Id,
                        Valor = dado.Valor
                    };

                    if (d.IdTipoEnsino.HasValue && d.IdTipoEnsino.Value == 0)
                    {
                        d.IdTipoEnsino = null;
                    }

                    if (d.IdSubcategoria.HasValue && d.IdSubcategoria.Value == 0)
                    {
                        d.IdSubcategoria = null;
                    }

                    dtos.Add(d);
                }

                do
                {
                    var extrato = dtos.Take(100).ToList();
                    foreach (var dado in extrato)
                    {
                        dtos.Remove(dado);
                    }

                    _dadoServico.Inclui(extrato);
                }
                while (dtos.Count > 0);
            }
        }

        private void ExcluirArquivo(FileInfo arquivo)
        {
            arquivo.Delete();
        }
    }
}