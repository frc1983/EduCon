using EduCon.Utilitarios.Dominio;
using EduCon.Utilitarios.Dominio.Interfaces;
using Microsoft.Practices.ServiceLocation;

namespace EduCon.Utilitarios.Aplicacao
{
    public class AplServico
    {
        public ITransacao Transacao { get; private set; }

        public AplServico()
        {
            Transacao = ServiceLocator.Current.GetInstance<ITransacao>();
        }
    }
}