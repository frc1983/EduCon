using System;
using EduCon.ImportaFee.Infra;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace EduCon.ImportaFee
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var container = InjecaoInicializa.Initialize();
                using (container.BeginExecutionContextScope())
                {
                    var executor = new Executor();
                    executor.Executa();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro inesperado: " + ex.Message);
            }

            Console.ReadKey();
        }
    }
}