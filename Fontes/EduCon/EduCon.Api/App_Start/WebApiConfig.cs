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
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            //config.MapHttpAttributeRoutes();

            // Configuração das rotas
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Tratamento de exceções
            config.Filters.Add(new ExceptionFilter());
        }
    }
}
