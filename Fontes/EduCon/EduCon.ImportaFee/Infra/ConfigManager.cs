using System.Configuration;

namespace EduCon.ImportaFee
{
    internal class ConfigManager
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