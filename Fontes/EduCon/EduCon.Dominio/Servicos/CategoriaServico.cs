using System.Collections.Generic;
using EduCon.Base.Dominio;
using EduCon.Dominio.Entidades;
using EduCon.Dominio.Interfaces.Repositorio;
using EduCon.Dominio.Interfaces.Servico;

namespace EduCon.Dominio.Servicos
{
    public class CategoriaServico : Servico<Categoria>, ICategoriaServico
    {
        public CategoriaServico(ICategoriaRepositorio repositorio) : base(repositorio)
        {

        }

        public IEnumerable<Categoria> ListaCategorias()
        {
            return (Repositorio as ICategoriaRepositorio).ListaCategorias();
        }

        public IEnumerable<Categoria> ListaSubcategorias()
        {
            return (Repositorio as ICategoriaRepositorio).ListaSubcategorias();
        }
    }
}