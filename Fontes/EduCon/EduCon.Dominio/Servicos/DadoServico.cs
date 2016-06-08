using EduCon.Base.Dominio;
using EduCon.Base.Dominio.Interfaces;
using EduCon.Dominio.Interfaces.Servico;
using EduCon.Objetos.Entidades;

namespace EduCon.Dominio.Servicos
{
    public class DadoServico : Servico<Dado>, IDadoServico
    {
        public DadoServico(IRepositorio<Dado> repositorio) : base(repositorio)
        {
        }
    }
}