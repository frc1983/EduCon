using System.Collections.Generic;
using System.Linq;
using EduCon.Dominio.Entidades;
using EduCon.Dominio.Interfaces.Repositorio;
using EduCon.Objetos.DTOs;
using EduCon.Repositorio.Base;

namespace EduCon.Repositorio
{
    public class DadoRepositorio : Repositorio<Dado>, IDadoRepositorio
    {
        public IEnumerable<DadoOLAP> ListaOlap()
        {
            var lista = (
               from dado in DbSet
               join fonte in Contexto.Set<Fonte>() on dado.IdFonte equals fonte.Id
               join municipio in Contexto.Set<Municipio>() on dado.IdMunicipio equals municipio.Id
               join tipoEnsino in Contexto.Set<TipoEnsino>() on dado.IdTipoEnsino equals tipoEnsino.Id
               join categoria in Contexto.Set<Categoria>() on dado.IdCategoria equals categoria.Id
               join subcategoria in Contexto.Set<Categoria>() on dado.IdSubcategoria equals subcategoria.Id
               join data in Contexto.Set<Data>() on dado.IdData equals data.Id
               where !categoria.Nome.Contains("Taxa")
               select new DadoRetOLAP()
               {
                   Fonte = fonte.Nome,
                   Municipio = municipio.Nome,
                   TipoEnsino = tipoEnsino.Nome,
                   Categoria = categoria.Nome,
                   Subcategoria = subcategoria.Nome,
                   Ano = data.Ano.ToString(),
                   Valor = dado.Valor
               }
           ).ToList();

            return lista.Select(o => new DadoOLAP()
            {
                Fonte = o.Fonte,
                Municipio = o.Municipio,
                TipoEnsino = o.TipoEnsino,
                Categoria = o.Categoria,
                Subcategoria = o.Subcategoria,
                Ano = o.Ano,
                Valor = int.Parse(o.Valor)
            });
        }

        private class DadoRetOLAP
        {
            public string Fonte { get; set; }
            public string Municipio { get; set; }
            public string TipoEnsino { get; set; }
            public string Categoria { get; set; }
            public string Subcategoria { get; set; }
            public string Ano { get; set; }
            public string Valor { get; set; }
        }
    }
}