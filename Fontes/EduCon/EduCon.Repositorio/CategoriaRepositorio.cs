using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EduCon.Dominio.Entidades;
using EduCon.Dominio.Interfaces.Repositorio;
using EduCon.Repositorio.Base;

namespace EduCon.Repositorio
{
    public class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
    {
        public IEnumerable<Categoria> ListaCategorias()
        {
            return (
                from c in DbSet.AsNoTracking()
                where c.DadosCategoria.Any()
                select c
            ).ToList();
        }

        public IEnumerable<Categoria> ListaSubcategorias()
        {
            return (
                from s in DbSet.AsNoTracking()
                where s.DadosSubcategoria.Any()
                select s
            ).ToList();
        }
    }
}