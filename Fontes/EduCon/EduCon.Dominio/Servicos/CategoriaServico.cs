using EduCon.Dominio.Interfaces.Servico;
using EduCon.Objetos.Entidades;
using EduCon.Utilitarios.Dominio;
using EduCon.Utilitarios.Dominio.Interfaces;

namespace EduCon.Dominio.Servicos
{
    public class CategoriaServico : Servico<Categoria>, ICategoriaServico
    {
        public CategoriaServico(IRepositorio<Categoria> repositorio) : base(repositorio)
        {
        }
    }
}