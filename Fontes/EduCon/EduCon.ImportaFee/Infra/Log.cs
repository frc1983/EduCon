using System;
using System.Diagnostics;
using System.Reflection;

namespace EduCon.ImportaFee
{
    internal class Log
    {
        internal static void RegistraInformacao(MethodBase metodo, string mensagem)
        {
            var tipoEvento = TraceEventType.Information;

            throw new NotImplementedException();
        }

        internal static void RegistraErro(MethodBase metodo, string mensagem, Exception ex)
        {
            var tipoEvento = TraceEventType.Error;

            throw new NotImplementedException();
        }

        private static void Registra(MethodBase metodo, string mensagem, TraceEventType tipoEvento, Exception ex)
        {
            // TODO: Implementar log
        }
    }
}