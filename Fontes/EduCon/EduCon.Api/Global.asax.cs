using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using EduCon.Aplicacao;
using EduCon.Utilitarios.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace EduCon.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Método análago de inicialização da aplicação Web.
        /// </summary>
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            Application_StartConfig();
        }

        protected void Application_StartConfig()
        {
            // Remove os formatadores e inclui apenas o de JSON.
            GlobalConfiguration.Configuration.Formatters.Clear();
            GlobalConfiguration.Configuration.Formatters.Add(new JsonMediaTypeFormatter());

            // Configura formatações retorno de dados JSON.
            var jsonConfigSettings = GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;
            jsonConfigSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonConfigSettings.Converters.Add(new StringEnumConverter());
            jsonConfigSettings.Converters.Add(new DecimalConverter());
            jsonConfigSettings.Formatting = Formatting.Indented;
            jsonConfigSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsonConfigSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            jsonConfigSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            
            // Define a mensagem de erro de conversão de valor.
            ModelBinderConfig.TypeConversionErrorMessageProvider = (context, metadata, value) =>
            {
                return string.Format("'{0}' não é um valor válido", value.ToString());
            };

            // Inicializa o SimpleInjector
            App_Start.SimpleInjector.Inicializa();

            // Inicializa as configurações da camada de aplicação.
            InicializaAplicacao.Inicia();
        }
    }
}