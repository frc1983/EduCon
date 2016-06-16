using System;

namespace EduCon.ImportaFee.Infra
{
    public static class ConsoleExp
    {
        public static void Write(string texto, params object[] arg)
        {
            Console.Write(DateTime.Now.ToString() + " - " + string.Format(texto, arg));
        }

        public static void WriteLine(string texto, params object[] arg)
        {
            Console.WriteLine(DateTime.Now.ToString() + " - " + string.Format(texto, arg));
        }
    }
}