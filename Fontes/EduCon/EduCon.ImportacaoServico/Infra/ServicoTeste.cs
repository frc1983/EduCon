using System;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace EduCon.ImportacaoServico.Infra
{
    internal class ServicoTeste
    {
        public static void Main()
        {
            try
            {
                var container = Injecao.Inicializa();
                using (container.BeginExecutionContextScope())
                {
                    var importador = new Importador();
                    importador.Importa();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}