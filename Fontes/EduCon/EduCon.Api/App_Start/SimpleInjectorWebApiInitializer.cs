[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(EduCon.Api.App_Start.SimpleInjectorWebApiInitializer), "Initialize")]

namespace EduCon.Api.App_Start
{
    using System;
    using System.Web.Http;
    using Injecao;
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;

    public static class SimpleInjectorWebApiInitializer
    {
        /// <summary>
        /// Initialize the container and register it as Web API Dependency Resolver.
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

        private static void InitializeContainer(Container container)
        {
            Injeta.GetSimpleInjectorModules(container);
        }
    }
}