using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace EduCon.Api.Utilitarios
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is NotImplementedException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                return;
            }

            // TODO: Logar erro
            //context.Exception;

            var mensagemErro = "Um erro inesperado aconteceu ao realizar a operação. A equipe responsável será notificada. Tente novamente mais tarde";
            context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Mensagem = mensagemErro });
        }
    }
}