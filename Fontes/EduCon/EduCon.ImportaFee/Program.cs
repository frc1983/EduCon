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
                    var resposta = string.Empty;
                    do
                    {
                        Console.Write(@"Copiar arquivos do diretório configurado? (S/N) \> ");
                        resposta = Console.ReadLine().ToLower();
                    } while (!resposta.Equals("s") && !resposta.Equals("n"));

                    var copia = resposta.Equals("s");

                    var executor = new Executor();
                    executor.Executa(copia);
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