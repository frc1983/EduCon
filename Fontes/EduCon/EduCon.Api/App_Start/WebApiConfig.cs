using System.Web.Http;
using System.Web.Http.Cors;
using EduCon.Api.Utilitarios;

namespace EduCon.Api
{
    public static class WebApiConfig
    {
        /// <summary>
        /// Configuração dos serviços da Web API.
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // Permite tudo
            var cors = new EnableCorsAttribute("*", "*", "*");
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
        }
    }
}