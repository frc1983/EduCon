using EduCon.Base.Dominio;
using EduCon.Base.Dominio.Interfaces;
using EduCon.Dominio.Interfaces.Servico;
using EduCon.Objetos.Entidades;

namespace EduCon.Dominio.Servicos
{
    public class CategoriaServico : Servico<Categoria>, ICategoriaServico
    {
        public CategoriaServico(IRepositorio<Categoria> repositorio) : base(repositorio)
        {
        }
    }
}