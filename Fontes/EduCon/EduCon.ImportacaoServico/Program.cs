using System.ServiceProcess;

namespace EduCon.ImportacaoServico
{
    static class Program
    {
        /// <summary>
        /// Método inicial do serviço.
        /// </summary>
        static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new Servico()
            };

            ServiceBase.Run(servicesToRun);
        }
    }
}