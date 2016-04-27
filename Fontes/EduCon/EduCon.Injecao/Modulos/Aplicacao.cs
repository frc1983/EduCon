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
            container.Register<IDataAplServico, DataAplServico>(Lifestyle.Scoped);
            container.Register<ITipoEnsinoAplServico, TipoEnsinoAplServico>(Lifestyle.Scoped);
            container.Register<ICategoriaAplServico, CategoriaAplServico>(Lifestyle.Scoped);
            container.Register<IDadoAplServico, DadoAplServico>(Lifestyle.Scoped);
        }
    }
}