using EduCon.Aplicacao.Interfaces;
using EduCon.Aplicacao.Servicos;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace EduCon.Injecao.Modulos
{
    public class Aplicacao : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register<IMunicipioAplServico, MunicipioAplServico>(Lifestyle.Scoped);
        }
    }
}