using EduCon.Contexto;
using EduCon.Utilitarios.Contexto.Interfaces;
using EduCon.Utilitarios.Dominio;
using EduCon.Utilitarios.Dominio.Interfaces;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace EduCon.Injecao.Modulos
{
    public class Contexto : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register<IContexto, ContextoEF>(Lifestyle.Scoped);
            container.Register(typeof(ITransacao), typeof(Transacao), Lifestyle.Scoped);
        }
    }
}