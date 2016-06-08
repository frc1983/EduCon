using EduCon.Base.Dominio;
using EduCon.Base.Dominio.Interfaces;
using EduCon.Dominio.Interfaces.Servico;
using EduCon.Objetos.Entidades;

namespace EduCon.Dominio.Servicos
{
    public class TipoEnsinoServico : Servico<TipoEnsino>, ITipoEnsinoServico
    {
        public TipoEnsinoServico(IRepositorio<TipoEnsino> repositorio) : base(repositorio)
        {
        }
    }
}