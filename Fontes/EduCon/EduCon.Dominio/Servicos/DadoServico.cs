using EduCon.Dominio.Interfaces.Servico;
using EduCon.Objetos.Entidades;
using EduCon.Utilitarios.Dominio;
using EduCon.Utilitarios.Dominio.Interfaces;

namespace EduCon.Dominio.Servicos
{
    public class DadoServico : Servico<Dado>, IDadoServico
    {
        public DadoServico(IRepositorio<Dado> repositorio) : base(repositorio)
        {
        }
    }
}