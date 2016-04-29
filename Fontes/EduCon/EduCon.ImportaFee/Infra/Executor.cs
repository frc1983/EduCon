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
        private IList<DadoDTO> dados;
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
            dados = new List<DadoDTO>();
            tiposEnsino = new List<TipoEnsinoDTO>();
            categorias = new List<CategoriaDTO>();
            municipios = new List<MunicipioDTO>();
            datas = new List<DataDTO>();

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

                CarregaDados();

                IncluiDados();
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

        private void CarregaDados()
        {
            var pasta = new DirectoryInfo(ConfigManager.DiretorioArquivos);
            foreach (var file in pasta.GetFiles())
            {
                var stream = UTF8.Converte(file.FullName);
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
                            var munic = _municipioServico.ListaTodos().Where(o => o.CodIBGE == int.Parse(unidadeGeografica.Ibge)).FirstOrDefault();
                            if (munic == null)
                            {
                                var municipio = new MunicipioDTO()
                                {
                                    CodIBGE = int.Parse(unidadeGeografica.Ibge),
                                    Nome = unidadeGeografica.Nome.Trim(),
                                    Latitude = decimal.Parse(unidadeGeografica.Latitude),
                                    Longitude = decimal.Parse(unidadeGeografica.Longitude)
                                };

                                _municipioServico.Inclui(municipio);
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

                        var tipoEnsinoDescr = (caminho.Length > 3 ? caminho[2] : string.Empty);

                        if (TipoEnsino == null || !TipoEnsino.Nome.Equals(tipoEnsinoDescr))
                        {
                            TipoEnsino = new TipoEnsinoDTO();
                            if (!string.IsNullOrEmpty(tipoEnsinoDescr))
                            {
                                var tipoEns = _tipoEnsinoServico.ListaTodos().Where(o => o.Nome.Equals(tipoEnsinoDescr)).FirstOrDefault();
                                if (tipoEns == null)
                                {
                                    var tipoEnsino = new TipoEnsinoDTO()
                                    {
                                        Nome = tipoEnsinoDescr.Trim()
                                    };

                                    _tipoEnsinoServico.Inclui(tipoEnsino);
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

                        var categoriaDescr = (caminho.Length > 4 ? caminho[3] : string.Empty);

                        if (Categoria == null || !Categoria.Nome.Equals(categoriaDescr))
                        {
                            Categoria = new CategoriaDTO();
                            if (!string.IsNullOrEmpty(categoriaDescr))
                            {
                                var categ = _categoriaServico.ListaTodos().Where(o => o.Nome.Equals(categoriaDescr)).FirstOrDefault();
                                if (categ == null)
                                {
                                    var categoria = new CategoriaDTO()
                                    {
                                        Nome = categoriaDescr.Trim()
                                    };

                                    _categoriaServico.Inclui(categoria);
                                    Categoria = categoria;
                                }
                                else
                                {
                                    Categoria = categ;
                                }
                            }
                        }

                        var subcategoriaDescr = (caminho.Length == 5 ? caminho[4] : string.Empty);

                        if (Subcategoria == null || !Subcategoria.Nome.Equals(subcategoriaDescr))
                        {
                            Subcategoria = new CategoriaDTO();
                            if (!string.IsNullOrEmpty(subcategoriaDescr))
                            {
                                var subcateg = _categoriaServico.ListaTodos().Where(o => o.Nome.Equals(subcategoriaDescr)).FirstOrDefault();
                                if (subcateg == null)
                                {
                                    var subcategoria = new CategoriaDTO()
                                    {
                                        Nome = subcategoriaDescr.Trim()
                                    };

                                    _categoriaServico.Inclui(subcategoria);
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
                            var dt = _dataServico.ListaTodos().Where(o => o.Ano == int.Parse(valor.Ano)).FirstOrDefault();
                            if (dt == null)
                            {
                                var data = new DataDTO()
                                {
                                    Ano = int.Parse(valor.Ano)
                                };

                                _dataServico.Inclui(data);
                                Data = data;
                            }
                            else
                            {
                                Data = dt;
                            }
                        }

                        #endregion

                        var dado = new DadoDTO()
                        {
                            IdTipoEnsino = TipoEnsino.Id,
                            IdCategoria = Categoria.Id,
                            IdSubcategoria = Subcategoria.Id,
                            IdMunicipio = Municipio.Id,
                            IdData = Data.Id,
                            Valor = valor.Valor
                        };

                        _dadoServico.Inclui(dado);
                    }
                }
            }
        }

        private void IncluiDados()
        {
            //foreach (var unidadeGeografica in obj.UnidadesGeograficas)
            //{
            //    MunicipioDTO Municipio = null;
            //    TipoEnsinoDTO TipoEnsino = null;
            //    CategoriaDTO Categoria = null;
            //    CategoriaDTO Subcategoria = null;
            //    DataDTO Data = null;

            //    foreach (var valor in unidadeGeografica.Valores)
            //    {
            //        #region Município

            //        if (Municipio == null || Municipio.CodIBGE != int.Parse(unidadeGeografica.Ibge))
            //        {
            //            Municipio = new MunicipioDTO();
            //            var munic = _municipioServico.ListaTodos().Where(o => o.CodIBGE == int.Parse(unidadeGeografica.Ibge)).FirstOrDefault();
            //            if (munic == null)
            //            {
            //                var municipio = new MunicipioDTO()
            //                {
            //                    CodIBGE = int.Parse(unidadeGeografica.Ibge),
            //                    Nome = unidadeGeografica.Nome.Trim(),
            //                    Latitude = decimal.Parse(unidadeGeografica.Latitude),
            //                    Longitude = decimal.Parse(unidadeGeografica.Longitude)
            //                };

            //                _municipioServico.Inclui(municipio);
            //                Municipio = municipio;
            //            }
            //            else
            //            {
            //                Municipio = munic;
            //            }
            //        }

            //        #endregion

            //        var caminho = variavel.Caminho.Split('/');

            //        #region Tipo de Ensino

            //        var tipoEnsinoDescr = (caminho.Length > 3 ? caminho[2] : string.Empty);

            //        if (TipoEnsino == null || !TipoEnsino.Nome.Equals(tipoEnsinoDescr))
            //        {
            //            TipoEnsino = new TipoEnsinoDTO();
            //            if (!string.IsNullOrEmpty(tipoEnsinoDescr))
            //            {
            //                var tipoEns = _tipoEnsinoServico.ListaTodos().Where(o => o.Nome.Equals(tipoEnsinoDescr)).FirstOrDefault();
            //                if (tipoEns == null)
            //                {
            //                    var tipoEnsino = new TipoEnsinoDTO()
            //                    {
            //                        Nome = tipoEnsinoDescr.Trim()
            //                    };

            //                    _tipoEnsinoServico.Inclui(tipoEnsino);
            //                    TipoEnsino = tipoEnsino;
            //                }
            //                else
            //                {
            //                    TipoEnsino = tipoEns;
            //                }
            //            }
            //        }

            //        #endregion

            //        #region Categoria

            //        var categoriaDescr = (caminho.Length > 4 ? caminho[3] : string.Empty);

            //        if (Categoria == null || !Categoria.Nome.Equals(categoriaDescr))
            //        {
            //            Categoria = new CategoriaDTO();
            //            if (!string.IsNullOrEmpty(categoriaDescr))
            //            {
            //                var categ = _categoriaServico.ListaTodos().Where(o => o.Nome.Equals(categoriaDescr)).FirstOrDefault();
            //                if (categ == null)
            //                {
            //                    var categoria = new CategoriaDTO()
            //                    {
            //                        Nome = categoriaDescr.Trim()
            //                    };

            //                    _categoriaServico.Inclui(categoria);
            //                    Categoria = categoria;
            //                }
            //                else
            //                {
            //                    Categoria = categ;
            //                }
            //            }
            //        }

            //        var subcategoriaDescr = (caminho.Length == 5 ? caminho[4] : string.Empty);

            //        if (Subcategoria == null || !Subcategoria.Nome.Equals(subcategoriaDescr))
            //        {
            //            Subcategoria = new CategoriaDTO();
            //            if (!string.IsNullOrEmpty(subcategoriaDescr))
            //            {
            //                var subcateg = _categoriaServico.ListaTodos().Where(o => o.Nome.Equals(subcategoriaDescr)).FirstOrDefault();
            //                if (subcateg == null)
            //                {
            //                    var subcategoria = new CategoriaDTO()
            //                    {
            //                        Nome = subcategoriaDescr.Trim()
            //                    };

            //                    _categoriaServico.Inclui(subcategoria);
            //                    Subcategoria = subcategoria;
            //                }
            //                else
            //                {
            //                    Subcategoria = subcateg;
            //                }
            //            }
            //        }

            //        #endregion

            //        #region Ano

            //        if (Data == null || Data.Ano != int.Parse(valor.Ano))
            //        {
            //            Data = new DataDTO();
            //            var dt = _dataServico.ListaTodos().Where(o => o.Ano == int.Parse(valor.Ano)).FirstOrDefault();
            //            if (dt == null)
            //            {
            //                var data = new DataDTO()
            //                {
            //                    Ano = int.Parse(valor.Ano)
            //                };

            //                _dataServico.Inclui(data);
            //                Data = data;
            //            }
            //            else
            //            {
            //                Data = dt;
            //            }
            //        }

            //        #endregion

            //        var dado = new DadoDTO()
            //        {
            //            IdTipoEnsino = TipoEnsino.Id,
            //            IdCategoria = Categoria.Id,
            //            IdSubcategoria = Subcategoria.Id,
            //            IdMunicipio = Municipio.Id,
            //            IdData = Data.Id,
            //            Valor = valor.Valor
            //        };

            //        _dadoServico.Inclui(dado);
            //    }
            //}
        }
    }
}