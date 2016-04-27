using EduCon.Dominio.Interfaces.Servico;
using EduCon.Objetos.Entidades;
using EduCon.Utilitarios.Dominio;
using EduCon.Utilitarios.Dominio.Interfaces;

namespace EduCon.Dominio.Servicos
{
    public class DataServico : Servico<Data>, IDataServico
    {
        public DataServico(IRepositorio<Data> repositorio) : base(repositorio)
        {
        }
    }
}