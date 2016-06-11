using EduCon.Base.Dominio;
using EduCon.Base.Dominio.Interfaces;
using EduCon.Dominio.Entidades;
using EduCon.Dominio.Interfaces.Servico;

namespace EduCon.Dominio.Servicos
{
    public class DadoServico : Servico<Dado>, IDadoServico
    {
        public DadoServico(IRepositorio<Dado> repositorio) : base(repositorio)
        {
        }
    }
}