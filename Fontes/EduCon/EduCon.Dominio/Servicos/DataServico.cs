using EduCon.Base.Dominio;
using EduCon.Base.Dominio.Interfaces;
using EduCon.Dominio.Interfaces.Servico;
using EduCon.Objetos.Entidades;

namespace EduCon.Dominio.Servicos
{
    public class DataServico : Servico<Data>, IDataServico
    {
        public DataServico(IRepositorio<Data> repositorio) : base(repositorio)
        {
        }
    }
}