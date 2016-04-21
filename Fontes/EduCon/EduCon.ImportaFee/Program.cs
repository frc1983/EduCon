using System.ServiceProcess;

namespace EduCon.ImportaFee
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new EduConSvc()
            };

            ServiceBase.Run(servicesToRun);
        }
    }
}