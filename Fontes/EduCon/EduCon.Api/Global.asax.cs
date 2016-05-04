using System.Web.Http;
using System.Web.Http.ModelBinding;
using EduCon.Aplicacao;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;

namespace EduCon.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            Application_StartConfig();
        }

        protected void Application_StartConfig()
        {
            ModelBinderConfig.TypeConversionErrorMessageProvider = (context, metadata, value) =>
            {
                return string.Format("'{0}' não é um valor válido", value.ToString());
            };

            InicializaAplicacao.Inicia();

            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
            Injecao.Injeta.GetSimpleInjectorModules(container);
        }
    }
}