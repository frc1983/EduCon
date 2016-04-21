using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCon.ImportaFee.Infra
{
    public class TesteServico
    {
        public static void Main()
        {
            try
            {
                //var container = SimpleInjectorExeInitializer.Initialize();
                //using (container.BeginExecutionContextScope())
                {
                    var executor = new Executor();
                    executor.Executa();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}