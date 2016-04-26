using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.Entidades;
using EduCon.Utilitarios.Conversores;
using Microsoft.Practices.ServiceLocation;

namespace EduCon.ImportaFee.Infra
{
    public class Executor
    {
        private IList<Dado> lista;
        private IList<TipoEnsino> tiposEnsino;
        private IList<Categoria> categorias;
        private IList<Municipio> municipios;
        private IList<Data> datas;

        private IMunicipioAplServico _municipioServico;

        public Executor()
        {
            lista = new List<Dado>();
            tiposEnsino = new List<TipoEnsino>();
            categorias = new List<Categoria>();
            municipios = new List<Municipio>();
            datas = new List<Data>();

            _municipioServico = ServiceLocator.Current.GetInstance<IMunicipioAplServico>();
        }

        public void Executa()
        {
            try
            {
                // Lógica do processamento
                ImportaDados();

                ResumoExecucao();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ImportaDados()
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
                            var municipio = new Municipio()
                            {
                                CodIBGE = int.Parse(unidadeGeografica.Ibge),
                                Nome = unidadeGeografica.Nome,
                                Latitude = decimal.Parse(unidadeGeografica.Latitude),
                                Longitude = decimal.Parse(unidadeGeografica.Longitude)
                            });

                            _municipioServico.Inclui(municipio)
                        }
                        else
                        {
                            codMunicipio = munic.Id;
                        }

                        #endregion

                        var caminho = variavel.Caminho.Split('/');

                        #region Tipo de Ensino

                        int codTipoEnsino = 0;

                        var tipoEnsino = (caminho.Length > 3 ? caminho[2] : string.Empty);

                        if (!string.IsNullOrEmpty(tipoEnsino))
                        {
                            var tipoEns = tiposEnsino.Where(o => o.Nome == tipoEnsino).FirstOrDefault();
                            if (tipoEns == null)
                            {
                                tiposEnsino.Add(new TipoEnsino()
                                {
                                    Nome = tipoEnsino
                                });
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

                        var categoria = (caminho.Length > 4 ? caminho[3] : string.Empty);
                        var subcategoria = (caminho.Length > 5 ? caminho[4] : string.Empty);

                        if (!string.IsNullOrEmpty(categoria))
                        {
                            var categ = categorias.Where(o => o.Nome == categoria).FirstOrDefault();
                            if (categ == null)
                            {
                                categorias.Add(new Categoria()
                                {
                                    Nome = categoria
                                });
                            }
                            else
                            {
                                codCategoria = categ.Id;
                            }
                        }

                        if (!string.IsNullOrEmpty(subcategoria))
                        {
                            var categ = categorias.Where(o => o.Nome == subcategoria).FirstOrDefault();
                            if (categ == null)
                            {
                                categorias.Add(new Categoria()
                                {
                                    Nome = subcategoria
                                });
                            }
                            else
                            {
                                codSubcategoria = categ.Id;
                            }
                        }

                        #endregion

                        #region Ano

                        var codData = 0;

                        var data = datas.Where(o => o.Ano == int.Parse(valor.Ano)).FirstOrDefault();
                        if (data == null)
                        {
                            datas.Add(new Data()
                            {
                                Ano = int.Parse(valor.Ano)
                            });
                        }
                        else
                        {
                            codMunicipio = munic.Id;
                        }

                        #endregion

                        lista.Add(new Dado()
                        {
                            IdTipoEnsino = codTipoEnsino,
                            IdCategoria = codCategoria,
                            IdSubcategoria = codSubcategoria,
                            IdMunicipio = codMunicipio,
                            IdData = codData,
                            Valor = valor.Valor
                        });
                    }
                }
            }
        }

        private void ResumoExecucao()
        {
            var total = lista.Count;
            var comValor = lista.Count(o => o.Valor != "-");
            var comPerc = decimal.Round(comValor * 1m / total * 100m, 2);
            var semValor = lista.Count(o => o.Valor == "-");
            var semPerc = decimal.Round(semValor * 1m / total * 100m, 2);

            Console.WriteLine("Com valor: " + comPerc + "% (" + comValor + ")");
            Console.WriteLine("Sem valor: " + semPerc + "% (" + semValor + ")");
            Console.WriteLine("Total: " + lista.Count);
        }
    }
}