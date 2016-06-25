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
                context.Response = context.Request.CreateResponse(HttpStatusCode.NotImplemented);
                return;
            }

            if (context.Exception is InvalidOperationException)
            {
                context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro { Mensagem = context.Exception.Message });
                return;
            }

            // TODO: Logar erro
            //context.Exception;

            // Devolve uma mensagem customizada para o usuário, sob o código de erro no servidor (500).
            var mensagemErro = "Um erro ocorreu ao processar a solicitação. Por favor, tente novamente.";
            mensagemErro += Environment.NewLine + context.Exception.Message;
            mensagemErro += Environment.NewLine + context.Exception.StackTrace;
            context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro { Mensagem = mensagemErro });
        }
    }
}