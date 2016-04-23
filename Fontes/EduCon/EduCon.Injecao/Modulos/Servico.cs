using EduCon.Dominio.Interfaces.Servico;
using EduCon.Dominio.Servicos;
using EduCon.Utilitarios.Dominio;
using EduCon.Utilitarios.Dominio.Interfaces;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace EduCon.Injecao.Modulos
{
    public class Servico : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register(typeof(IServico<>), typeof(Servico<>), Lifestyle.Scoped);

            container.Register<IMunicipioServico, MunicipioServico>(Lifestyle.Scoped);
        }
    }
}