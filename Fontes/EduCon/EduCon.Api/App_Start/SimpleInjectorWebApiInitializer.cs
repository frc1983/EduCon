[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(EduCon.Api.App_Start.SimpleInjectorWebApiInitializer), "Initialize")]

namespace EduCon.Api.App_Start
{
    using System;
    using System.Web.Http;
    using Injecao;
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;

    /// <summary>
    /// Classe inicializadora do SimpleInjector.
    /// </summary>
    public static class SimpleInjectorWebApiInitializer
    {
        /// <summary>
        /// Inicializa o container e carrega do resolvedor de dependências para a Web API.
        /// </summary>
        public static void Initialize()
        {
            var container = new Container();

            try
            {
                container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();

                InitializeContainer(container);

                container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
                container.Verify();

                GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
            }
            catch (Exception ex)
            {
                container.Dispose();
            }
        }

        /// <summary>
        /// Carrega no container as configurações do projeto de Injeção.
        /// </summary>
        /// <param name="container"></param>
        private static void InitializeContainer(Container container)
        {
            Injeta.GetSimpleInjectorModules(container);
        }
    }
}