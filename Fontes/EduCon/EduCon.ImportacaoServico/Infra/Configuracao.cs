using System.Configuration;

namespace EduCon.ImportacaoServico.Infra
{
    internal class Configuracao
    {
        public static int PeriodoMinutos
        {
            get { return int.Parse(ConfigurationManager.AppSettings["periodo"]); }
        }

        public static int PeriodoMinutosErro
        {
            get { return int.Parse(ConfigurationManager.AppSettings["periodo.erro"]); }
        }

        public static string UrlVariaveis
        {
            get { return ConfigurationManager.AppSettings["url.variaveis"]; }
        }

        public static string UrlArquivos
        {
            get { return ConfigurationManager.AppSettings["url.arquivos"]; }
        }

        public static void AtualizaConfiguracoes()
        {
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}