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

            try
            {
                var container = Infra.Injecao.Inicializa();
                using (container.BeginExecutionContextScope())
                {
                    bool sair = false;
                    do
                    {
                        sair = MontaMenu();
                    } while (!sair);
                }

                Console.WriteLine("Até mais.");
            }
            catch (Exception ex)
            {
                ConsoleExp.WriteLine("\n***** Erro inesperado: " + ex.Message);
            }

            Console.ReadKey();
        }

        static bool MontaMenu()
        {
            bool valido = false;
            int opcao = 0;

            do
            {
                Console.WriteLine("O que você deseja fazer?");
                Console.WriteLine("1) Nova importação");
                Console.WriteLine("2) Continuar importação");
                Console.WriteLine("3) Sair");
                Console.Write("Selecione uma das opções acima: ");
                var op = Console.ReadLine();

                if (!int.TryParse(op, out opcao))
                {
                    Console.Clear();
                    Console.WriteLine("***** Opção incorreta.");
                    continue;
                }

                valido = true;
            } while (!valido);

            var importador = new Importador();

            switch (opcao)
            {
                case 1:
                    importador.ImportaArquivos("\\Educação", null, null);

                    Console.WriteLine("***** Importação concluída com sucesso!");
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();

                    Console.Clear();
                    break;
                case 2:
                    importador.ImportaDados();

                    Console.WriteLine("***** Importação concluída com sucesso!");
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();

                    Console.Clear();
                    break;
                case 3:
                    return true;
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("***** Opção incorreta.");
                    break;
            }

            return false;
        }
    }
}