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
            return (
                from dado in DbSet
                join fonte in Contexto.Set<Fonte>() on dado.IdFonte equals fonte.Id
                join municipio in Contexto.Set<Municipio>() on dado.IdMunicipio equals municipio.Id
                join tipoEnsino in Contexto.Set<TipoEnsino>() on dado.IdTipoEnsino equals tipoEnsino.Id
                join categoria in Contexto.Set<Categoria>() on dado.IdCategoria equals categoria.Id
                join subcategoria in Contexto.Set<Categoria>() on dado.IdSubcategoria equals subcategoria.Id
                join data in Contexto.Set<Data>() on dado.IdData equals data.Id
                select new DadoOLAP()
                {
                    Fonte = fonte.Nome,
                    Municipio = municipio.Nome,
                    TipoEnsino = tipoEnsino.Nome,
                    Categoria = categoria.Nome,
                    Subcategoria = subcategoria.Nome,
                    Ano = data.Ano,
                    Valor = dado.Valor
                }
            );
        }
    }
}