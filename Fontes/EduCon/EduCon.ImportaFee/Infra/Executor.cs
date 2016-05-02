using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EduCon.Aplicacao.Interfaces;
using EduCon.ImportaFee.Objetos;
using EduCon.Objetos.DTOs;
using EduCon.Utilitarios.Conversores;
using Microsoft.Practices.ServiceLocation;

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

        public void Executa()
        {
            try
            {
                ValidaDiretorio();

                var pasta = new DirectoryInfo(ConfigManager.DiretorioArquivos);

                var atualArquivo = 1;
                var totalArquivos = pasta.GetFiles().Count();

                Console.WriteLine("Arquivos encontrados: " + totalArquivos);
                foreach (var arquivo in pasta.GetFiles())
                {
                    Console.WriteLine("Processando arquivo {0} de {1}: {2}", atualArquivo, totalArquivos, arquivo.Name);

                    dados = new List<Dado>();
                    tiposEnsino = new List<TipoEnsinoDTO>();
                    categorias = new List<CategoriaDTO>();
                    municipios = new List<MunicipioDTO>();
                    datas = new List<DataDTO>();

                    CarregaDados(arquivo);

                    IncluiDados();

                    atualArquivo++;
                }
            }
            catch (Exception ex)
            {
                throw;
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

        private void CarregaDados(FileInfo arquivo)
        {
            var stream = UTF8.Converte(arquivo.FullName);
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Objetos.Arquivo>(stream);

            var variavel = obj.Variavel.First();

            foreach (var unidadeGeografica in obj.UnidadesGeograficas)
            {
                MunicipioDTO Municipio = null;
                TipoEnsinoDTO TipoEnsino = null;
                CategoriaDTO Categoria = null;
                CategoriaDTO Subcategoria = null;
                DataDTO Data = null;

                foreach (var valor in unidadeGeografica.Valores)
                {
                    #region Município

                    if (Municipio == null || Municipio.CodIBGE != int.Parse(unidadeGeografica.Ibge))
                    {
                        Municipio = new MunicipioDTO();
                        var munic = _municipioServico.Lista(new MunicipioDTO() { CodIBGE = int.Parse(unidadeGeografica.Ibge) }).FirstOrDefault();
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
                            var tipoEns = _tipoEnsinoServico.Lista(new TipoEnsinoDTO() { Nome = tipoEnsinoDescr }).FirstOrDefault();
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
                            var categ = _categoriaServico.Lista(new CategoriaDTO() { Nome = categoriaDescr }).FirstOrDefault();
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
                            var subcateg = _categoriaServico.Lista(new CategoriaDTO() { Nome = subcategoriaDescr }).FirstOrDefault();
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

                    #region Ano

                    if (Data == null || Data.Ano != int.Parse(valor.Ano))
                    {
                        Data = new DataDTO();
                        var dt = _dataServico.Lista(new DataDTO() { Ano = int.Parse(valor.Ano) }).FirstOrDefault();
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

                    var d = new DadoDTO()
                    {
                        IdMunicipio = Municipio.Id,
                        IdTipoEnsino = TipoEnsino.Id,
                        IdCategoria = Categoria.Id,
                        IdSubcategoria = Subcategoria.Id,
                        IdData = Data.Id
                    };

                    var existe = _dadoServico.Lista(d).Any();
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

        private void IncluiDados()
        {
            var text = string.Empty;
            var count = 1;

            if (municipios.Count > 0)
            {
                text = "Municípios a incluir: " + municipios.Count + " | ";
                count = 1;
                Console.Write(text);
                foreach (var municipio in municipios)
                {
                    Console.SetCursorPosition(text.Length, Console.CursorTop);
                    Console.Write(count);

                    var existe = _municipioServico.Lista(new MunicipioDTO() { CodIBGE = municipio.CodIBGE }).Any();
                    if (!existe)
                    {
                        _municipioServico.Inclui(municipio);
                    }
                }
                Console.WriteLine();
            }

            if (tiposEnsino.Count > 0)
            {
                text = "Tipos de ensino a incluir: " + tiposEnsino.Count + " | ";
                count = 1;
                Console.Write(text);
                foreach (var tipoEnsino in tiposEnsino)
                {
                    Console.SetCursorPosition(text.Length, Console.CursorTop);
                    Console.Write(count);

                    var existe = _tipoEnsinoServico.Lista(new TipoEnsinoDTO() { Nome = tipoEnsino.Nome }).Any();
                    if (!existe)
                    {
                        _tipoEnsinoServico.Inclui(tipoEnsino);
                    }
                }
                Console.WriteLine();
            }

            if (categorias.Count > 0)
            {
                text = "Categorias a incluir: " + categorias.Count + " | ";
                count = 1;
                Console.Write(text);
                foreach (var categoria in categorias)
                {
                    Console.SetCursorPosition(text.Length, Console.CursorTop);
                    Console.Write(count);

                    var existe = _categoriaServico.Lista(new CategoriaDTO() { Nome = categoria.Nome }).Any();
                    if (!existe)
                    {
                        _categoriaServico.Inclui(categoria);
                    }
                }
                Console.WriteLine();
            }

            if (datas.Count > 0)
            {
                text = "Datas a incluir: " + datas.Count + " | ";
                count = 1;
                Console.Write(text);
                foreach (var data in datas)
                {
                    Console.SetCursorPosition(text.Length, Console.CursorTop);
                    Console.Write(count);

                    var existe = _dataServico.Lista(new DataDTO() { Ano = data.Ano }).Any();
                    if (!existe)
                    {
                        _dataServico.Inclui(data);
                    }
                }
                Console.WriteLine();
            }

            if (dados.Count > 0)
            {
                text = "Dados a incluir: " + dados.Count + " | ";
                count = 1;
                Console.Write(text);
                foreach (var dado in dados)
                {
                    Console.SetCursorPosition(text.Length, Console.CursorTop);
                    Console.Write(count);
                    var d = new DadoDTO()
                    {
                        IdMunicipio = dado.Municipio.Id,
                        IdTipoEnsino = dado.TipoEnsino.Id,
                        IdCategoria = dado.Categoria.Id,
                        IdSubcategoria = dado.Subcategoria.Id,
                        IdData = dado.Data.Id
                    };

                    if (d.IdSubcategoria.HasValue && d.IdSubcategoria.Value == 0)
                    {
                        d.IdSubcategoria = null;
                    }

                    var existe = _dadoServico.Lista(d).Any();
                    if (!existe)
                    {
                        d.Valor = dado.Valor;

                        _dadoServico.Inclui(d);
                        count++;
                    }
                }
                Console.WriteLine();
            }
        }
    }
}