using EduCon.Base.Dominio;
using EduCon.Base.Dominio.Interfaces;
using EduCon.Dominio.Entidades;
using EduCon.Dominio.Interfaces.Servico;

namespace EduCon.Dominio.Servicos
{
    public class CategoriaServico : Servico<Categoria>, ICategoriaServico
    {
        public CategoriaServico(IRepositorio<Categoria> repositorio) : base(repositorio)
        {
        }
    }
}