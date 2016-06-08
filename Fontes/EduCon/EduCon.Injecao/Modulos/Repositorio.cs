using EduCon.Base.Dominio.Interfaces;
using EduCon.Dominio.Interfaces.Repositorio;
using EduCon.Repositorio;
using EduCon.Repositorio.Base;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace EduCon.Injecao.Modulos
{
    public class Repositorio : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register(typeof(IRepositorio<>), typeof(Repositorio<>), Lifestyle.Scoped);

            container.Register<IMunicipioRepositorio, MunicipioRepositorio>(Lifestyle.Scoped);
            container.Register<IDataRepositorio, DataRepositorio>(Lifestyle.Scoped);
            container.Register<ITipoEnsinoRepositorio, TipoEnsinoRepositorio>(Lifestyle.Scoped);
            container.Register<ICategoriaRepositorio, CategoriaRepositorio>(Lifestyle.Scoped);
            container.Register<IDadoRepositorio, DadoRepositorio>(Lifestyle.Scoped);
        }
    }
}