using EduCon.Dominio.Interfaces.Servico;
using EduCon.Objetos.Entidades;
using EduCon.Utilitarios.Dominio;
using EduCon.Utilitarios.Dominio.Interfaces;

namespace EduCon.Dominio.Servicos
{
    public class TipoEnsinoServico : Servico<TipoEnsino>, ITipoEnsinoServico
    {
        public TipoEnsinoServico(IRepositorio<TipoEnsino> repositorio) : base(repositorio)
        {
        }
    }
}