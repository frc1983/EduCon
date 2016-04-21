using System.Configuration;

namespace EduCon.ImportaFee
{
    internal class ConfigManager
    {
        public static int PeriodoSono
        {
            get { return int.Parse(ConfigurationManager.AppSettings["periodo.processamento"]); }
        }

        public static int PeriodoSonoErro
        {
            get { return int.Parse(ConfigurationManager.AppSettings["periodo.erro"]); }
        }

        public static void AtualizarAppSettings()
        {
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}