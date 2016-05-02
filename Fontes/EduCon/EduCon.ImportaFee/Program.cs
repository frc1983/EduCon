using System;
using EduCon.ImportaFee.Infra;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace EduCon.ImportaFee
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("> EduCon.ImportaFee");
            Console.WriteLine("Iniciando rotina de carregamento...");

            try
            {
                var container = InjecaoInicializa.Initialize();
                using (container.BeginExecutionContextScope())
                {
                    var executor = new Executor();
                    executor.Executa();
                }

                Console.WriteLine("Fim da execução.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro inesperado: " + ex.Message);
            }

            Console.ReadKey();
        }
    }
}