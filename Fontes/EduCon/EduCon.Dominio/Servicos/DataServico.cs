using EduCon.Base.Dominio;
using EduCon.Base.Dominio.Interfaces;
using EduCon.Dominio.Entidades;
using EduCon.Dominio.Interfaces.Servico;

namespace EduCon.Dominio.Servicos
{
    public class DataServico : Servico<Data>, IDataServico
    {
        public DataServico(IRepositorio<Data> repositorio) : base(repositorio)
        {
        }
    }
}