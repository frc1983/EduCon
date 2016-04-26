using System.Configuration;

namespace EduCon.ImportaFee
{
    internal class ConfigManager
    {
        public static string DiretorioArquivos
        {
            get { return ConfigurationManager.AppSettings["diretorio.arquivos"]; }
        }
    }
}