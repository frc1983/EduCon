using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace EduCon.Api.Utilitarios
{
    /// <summary>
    /// Classe que determina o tratamento para exceções não tratadas.
    /// </summary>
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            // Se for uma exception de código não implementado, retornar o código Http respectivo.
            if (context.Exception is NotImplementedException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                return;
            }

            // TODO: Logar erro
            //context.Exception;

            // Devolve uma mensagem customizada para o usuário, sob o código de erro no servidor (500).
            var mensagemErro = "Um erro inesperado aconteceu ao realizar a operação. A equipe responsável será notificada. Tente novamente mais tarde";
            mensagemErro += Environment.NewLine + context.Exception.Message;
            mensagemErro += Environment.NewLine + context.Exception.StackTrace;
            context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Mensagem = mensagemErro });
        }
    }
}