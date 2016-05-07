using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using EduCon.Aplicacao;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using EduCon.Utilitarios.Api;
using Newtonsoft.Json;

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
            GlobalConfiguration.Configuration.Formatters.Clear();
            GlobalConfiguration.Configuration.Formatters.Add(new JsonMediaTypeFormatter());
            // Configura formatações retorno de dados
            var jsonConfigSettings = GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;
            jsonConfigSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonConfigSettings.Converters.Add(new StringEnumConverter());
            jsonConfigSettings.Converters.Add(new DecimalConverter());
            jsonConfigSettings.Formatting = Formatting.Indented;
            jsonConfigSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsonConfigSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            jsonConfigSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            
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