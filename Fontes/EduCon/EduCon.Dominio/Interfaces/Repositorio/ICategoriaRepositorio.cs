using System.Collections.Generic;
using EduCon.Base.Dominio.Interfaces;
using EduCon.Dominio.Entidades;

namespace EduCon.Dominio.Interfaces.Repositorio
{
    public interface ICategoriaRepositorio : IRepositorio<Categoria>
    {
        IEnumerable<Categoria> ListaCategorias();
        IEnumerable<Categoria> ListaSubcategorias();
    }
}