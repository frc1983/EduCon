using EduCon.Base.Dominio.Interfaces;
using EduCon.Dominio.Entidades;

namespace EduCon.Dominio.Interfaces.Servico
{
    public interface IProcessamentoServico : IServico<Processamento>
    {
        void Processando(Processamento entidade);
        void Processado(Processamento entidade);
        void Reprocessar(Processamento entidade);
    }
}