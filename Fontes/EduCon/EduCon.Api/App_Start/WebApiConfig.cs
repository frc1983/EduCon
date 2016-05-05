using System.Web.Http;
using System.Web.Http.Cors;
using EduCon.Api.Utilitarios;
using EduCon.Utilitarios.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace EduCon.Api
{
    public static class WebApiConfig
    {
        /// <summary>
        /// Web API configuration and services
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // Permite tudo
            var cors = new EnableCorsAttribute("http://localhost:3000", "*", "*");
            config.EnableCors(cors);

            // Configuração das rotas
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Tratamento de exceções
            config.Filters.Add(new ExceptionFilter());

            // Configura formatações retorno de dados
            var jsonConfigSettings = config.Formatters.JsonFormatter.SerializerSettings;
            jsonConfigSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonConfigSettings.Converters.Add(new StringEnumConverter());
            jsonConfigSettings.Converters.Add(new DecimalConverter());
            jsonConfigSettings.Formatting = Formatting.Indented;
            jsonConfigSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsonConfigSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            jsonConfigSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        }
    }
}
