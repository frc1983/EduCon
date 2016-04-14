using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JSONTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var folderPath = @"C:\Users\Lucas\Desktop\Dados";
            var folder = new DirectoryInfo(folderPath);

            var lista = new List<Objeto>();
            var tiposEnsino = new List<TipoEnsino>();
            var categorias = new List<Categoria>();
            var municipios = new List<Municipio>();

            foreach (var file in folder.GetFiles())
            {
                var stream = convertToUTF8(file.FullName);
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Arquivo>(stream);

                var variavel = obj.Variavel.First();

                foreach (var unidadeGeografica in obj.UnidadesGeograficas)
                {
                    foreach (var valor in unidadeGeografica.Valores)
                    {
                        #region Município

                        var codMunicipio = 0;
                        var munic = municipios.Where(o => o.CodIBGE == int.Parse(unidadeGeografica.Ibge)).FirstOrDefault();
                        if (munic == null)
                        {
                            codMunicipio = municipios.Count + 1;

                            municipios.Add(new Municipio()
                            {
                                CodMunicipio = codMunicipio,
                                CodIBGE = int.Parse(unidadeGeografica.Ibge),
                                Nome = unidadeGeografica.Nome,
                                Latitude = unidadeGeografica.Latitude,
                                Longitude = unidadeGeografica.Longitude
                            });
                        }
                        else
                        {
                            codMunicipio = munic.CodMunicipio;
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
                                codTipoEnsino = categorias.Count + 1;

                                tiposEnsino.Add(new TipoEnsino()
                                {
                                    CodTipoEnsino = codTipoEnsino,
                                    Nome = tipoEnsino
                                });
                            }
                            else
                            {
                                codTipoEnsino = tipoEns.CodTipoEnsino;
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
                                codCategoria = categorias.Count + 1;

                                categorias.Add(new Categoria()
                                {
                                    CodCategoria = codCategoria,
                                    Nome = categoria
                                });
                            }
                            else
                            {
                                codCategoria = categ.CodCategoria;
                            }
                        }

                        if (!string.IsNullOrEmpty(subcategoria))
                        {
                            var categ = categorias.Where(o => o.Nome == subcategoria).FirstOrDefault();
                            if (categ == null)
                            {
                                codSubcategoria = categorias.Count + 1;

                                categorias.Add(new Categoria()
                                {
                                    CodCategoria = codSubcategoria.Value,
                                    Nome = subcategoria
                                });
                            }
                            else
                            {
                                codSubcategoria = categ.CodCategoria;
                            }
                        }

                        #endregion

                        lista.Add(new Objeto()
                        {
                            CodObjeto = lista.Count + 1,
                            CodTipoEnsino = codTipoEnsino,
                            CodCategoria = codCategoria,
                            CodSubcategoria = codSubcategoria,
                            CodMunicipio = codMunicipio,
                            Ano = int.Parse(valor.Ano),
                            Valor = valor.Valor
                        });
                    }
                }
            }

            var total = lista.Count;
            var comValor = lista.Count(o => o.Valor != "-");
            var comPerc = decimal.Round(comValor * 1m / total * 100m, 2);
            var semValor = lista.Count(o => o.Valor == "-");
            var semPerc = decimal.Round(semValor * 1m / total * 100m, 2);

            Console.WriteLine("Com valor: " + comPerc + "% (" + comValor + ")");
            Console.WriteLine("Sem valor: " + semPerc + "% (" + semValor + ")");
            Console.WriteLine("Total: " + lista.Count);

            //foreach (var item in lista)
            //{
            //    Console.WriteLine(item);
            //}

            Console.ReadKey();
        }

        public static string convertToUTF8(string filePath)
        {
            var encoding = Encoding.Default;
            var fileText = string.Empty;

            using (var reader = new StreamReader(filePath, Encoding.Default))
            {
                fileText = reader.ReadToEnd();
                encoding = reader.CurrentEncoding;
                reader.Close();
            }

            if (encoding == Encoding.UTF8)
                return fileText;

            var fileBytes = encoding.GetBytes(fileText);
            var convertedBytes = Encoding.Convert(encoding, Encoding.UTF8, fileBytes);
            return Encoding.UTF8.GetString(convertedBytes);
        }
    }

    class Arquivo
    {
        public Variavel[] Variavel { get; set; }
        public UnidadesGeograficas[] UnidadesGeograficas { get; set; }
    }

    class Variavel
    {
        public string Nome { get; set; }
        public string Caminho { get; set; }
        public string Fonte { get; set; }
        public string Descricao { get; set; }
    }

    class UnidadesGeograficas
    {
        public string Agrupador { get; set; }
        public string Nome { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Ibge { get; set; }
        public AnoValor[] Valores { get; set; }
    }

    class AnoValor
    {
        public string Ano { get; set; }
        public string Unidade { get; set; }
        public string Valor { get; set; }
    }

    class Objeto
    {
        public int CodObjeto { get; set; }
        public int CodMunicipio { get; set; }
        public int CodTipoEnsino { get; set; }
        public int CodCategoria { get; set; }
        public int? CodSubcategoria { get; set; }
        public int Ano { get; set; }
        public string Valor { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1} - {2} - {3} - {4} - {5} - {6}",
                CodObjeto,
                CodTipoEnsino,
                CodCategoria,
                CodSubcategoria,
                CodMunicipio,
                Ano,
                Valor);
        }
    }

    class TipoEnsino
    {
        public int CodTipoEnsino { get; set; }
        public string Nome { get; set; }
    }

    class Categoria
    {
        public int CodCategoria { get; set; }
        public string Nome { get; set; }
    }

    class Municipio
    {
        public int CodMunicipio { get; set; }
        public int CodIBGE { get; set; }
        public string Nome { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}