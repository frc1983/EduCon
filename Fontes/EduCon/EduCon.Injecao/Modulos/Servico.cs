using EduCon.Base.Dominio;
using EduCon.Base.Dominio.Interfaces;
using EduCon.Dominio.Interfaces.Servico;
using EduCon.Dominio.Servicos;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace EduCon.Injecao.Modulos
{
    public class Servico : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register(typeof(IServico<>), typeof(Servico<>), Lifestyle.Scoped);

            container.Register<IFonteServico, FonteServico>(Lifestyle.Scoped);
            container.Register<IMunicipioServico, MunicipioServico>(Lifestyle.Scoped);
            container.Register<IDataServico, DataServico>(Lifestyle.Scoped);
            container.Register<ITipoEnsinoServico, TipoEnsinoServico>(Lifestyle.Scoped);
            container.Register<ICategoriaServico, CategoriaServico>(Lifestyle.Scoped);
            container.Register<IDadoServico, DadoServico>(Lifestyle.Scoped);

            container.Register<IProcessamentoServico, ProcessamentoServico>(Lifestyle.Scoped);
        }
    }
}