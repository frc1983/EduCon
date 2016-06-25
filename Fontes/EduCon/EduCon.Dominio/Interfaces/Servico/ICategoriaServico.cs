using System.Collections.Generic;
using EduCon.Base.Dominio.Interfaces;
using EduCon.Dominio.Entidades;

namespace EduCon.Dominio.Interfaces.Servico
{
    public interface ICategoriaServico : IServico<Categoria>
    {
        IEnumerable<Categoria> ListaCategorias();
        IEnumerable<Categoria> ListaSubcategorias();
    }
}