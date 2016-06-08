using EduCon.Base.Contexto.Interfaces;
using EduCon.Base.Dominio;
using EduCon.Base.Dominio.Interfaces;
using EduCon.Contexto;
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