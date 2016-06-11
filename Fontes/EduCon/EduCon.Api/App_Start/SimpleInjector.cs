using System;
using System.Web.Http;
using EduCon.Injecao;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;

namespace EduCon.Api.App_Start
{
    /// <summary>
    /// Classe inicializadora do SimpleInjector.
    /// </summary>
    public static class SimpleInjector
    {
        /// <summary>
        /// Inicializa o container e carrega do resolvedor de dependências para a Web API.
        /// </summary>
        public static void Inicializa()
        {
            var container = new Container();

            try
            {
                container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();

                InicializaContainer(container);

                container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
                container.Verify();

                GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
            }
            catch (Exception)
            {
                container.Dispose();
            }
        }

        /// <summary>
        /// Carrega no container as configurações do projeto de Injeção.
        /// </summary>
        /// <param name="container"></param>
        private static void InicializaContainer(Container container)
        {
            Injeta.RegistraModulos(container);
        }
    }
}