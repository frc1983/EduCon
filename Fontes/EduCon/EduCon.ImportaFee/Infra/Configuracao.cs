using System.Configuration;

namespace EduCon.ImportaFee
{
    internal class Configuracao
    {
        public static string UrlVariaveis
        {
            get { return ConfigurationManager.AppSettings["url.variaveis"]; }
        }

        public static string UrlArquivos
        {
            get { return ConfigurationManager.AppSettings["url.arquivos"]; }
        }
    }
}