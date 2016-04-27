using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EduCon.Aplicacao;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;
using EduCon.Utilitarios.Conversores;
using Microsoft.Practices.ServiceLocation;

namespace EduCon.ImportaFee.Infra
{
    public class Executor
    {
        private IList<DadoDTO> lista;
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
            lista = new List<DadoDTO>();
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

                // Lógica do processamento
                ImportaDados();
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

        private void ImportaDados()
        {
            var pasta = new DirectoryInfo(ConfigManager.DiretorioArquivos);
            foreach (var file in pasta.GetFiles())
            {
                var stream = UTF8.Converte(file.FullName);
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Objetos.Arquivo>(stream);

                var variavel = obj.Variavel.First();

                foreach (var unidadeGeografica in obj.UnidadesGeograficas)
                {
                    foreach (var valor in unidadeGeografica.Valores)
                    {
                        #region Município

                        var codMunicipio = 0;
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
                            codMunicipio = municipio.Id;
                        }
                        else
                        {
                            codMunicipio = munic.Id;
                        }

                        #endregion

                        var caminho = variavel.Caminho.Split('/');

                        #region Tipo de Ensino

                        int codTipoEnsino = 0;

                        var tipoEnsinoDescr = (caminho.Length > 3 ? caminho[2] : string.Empty);

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
                                codMunicipio = tipoEnsino.Id;
                            }
                            else
                            {
                                codTipoEnsino = tipoEns.Id;
                            }
                        }

                        #endregion

                        #region Categoria

                        int codCategoria = 0;
                        int? codSubcategoria = null;

                        var categoriaDescr = (caminho.Length > 4 ? caminho[3] : string.Empty);
                        var subcategoriaDescr = (caminho.Length == 5 ? caminho[4] : string.Empty);

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
                                codCategoria = categoria.Id;
                            }
                            else
                            {
                                codCategoria = categ.Id;
                            }
                        }

                        if (!string.IsNullOrEmpty(subcategoriaDescr))
                        {
                            var categ = _categoriaServico.ListaTodos().Where(o => o.Nome.Equals(subcategoriaDescr)).FirstOrDefault();
                            if (categ == null)
                            {
                                var subcategoria = new CategoriaDTO()
                                {
                                    Nome = subcategoriaDescr.Trim()
                                };

                                _categoriaServico.Inclui(subcategoria);
                                codSubcategoria = subcategoria.Id;
                            }
                            else
                            {
                                codSubcategoria = categ.Id;
                            }
                        }

                        #endregion

                        #region Ano

                        var codData = 0;

                        var dt = _dataServico.ListaTodos().Where(o => o.Ano == int.Parse(valor.Ano)).FirstOrDefault();
                        if (dt == null)
                        {
                            var data = new DataDTO()
                            {
                                Ano = int.Parse(valor.Ano)
                            };

                            _dataServico.Inclui(data);
                            codData = data.Id;
                        }
                        else
                        {
                            codData = dt.Id;
                        }

                        #endregion

                        var dado = new DadoDTO()
                        {
                            IdTipoEnsino = codTipoEnsino,
                            IdCategoria = codCategoria,
                            IdSubcategoria = codSubcategoria,
                            IdMunicipio = codMunicipio,
                            IdData = codData,
                            Valor = valor.Valor
                        };

                        _dadoServico.Inclui(dado);
                    }
                }
            }
        }
    }
}