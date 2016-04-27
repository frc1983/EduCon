using AutoMapper;
using EduCon.Aplicacao.Mapeamento.Base;
using EduCon.Utilitarios.Dominio.Interfaces;
using Microsoft.Practices.ServiceLocation;

namespace EduCon.Aplicacao.Servicos.Base
{
    public class AplServico
    {
        protected IMapper Mapper { get { return Mapeadores.Mapper; } }

        protected ITransacao Transacao { get; private set; }

        public AplServico()
        {
            Transacao = ServiceLocator.Current.GetInstance<ITransacao>();
        }
    }
}