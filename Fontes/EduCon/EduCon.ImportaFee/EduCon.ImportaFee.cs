using System;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using EduCon.ImportaFee.Infra;

namespace EduCon.ImportaFee
{
    public partial class EduConSvc : ServiceBase
    {
        private bool bFinalizar = false;

        private Thread serviceThread;
        private Executor executor;

        public EduConSvc()
        {
            InitializeComponent();
        }

        #region Events

        protected override void OnStart(string[] args)
        {
            try
            {
                Log.RegistraInformacao(MethodBase.GetCurrentMethod(), "OnStart - Início do método");
                bFinalizar = false;

                // Estado tem que ser Unstarted ou Stopped
                if (serviceThread == null || (serviceThread.ThreadState != ThreadState.Unstarted && serviceThread.ThreadState != ThreadState.Stopped))
                {
                    Log.RegistraInformacao(MethodBase.GetCurrentMethod(), "OnStart - Start da thread do serviço");
                    serviceThread = new Thread(new ThreadStart(ThreadServico));
                    serviceThread.Start();
                }

                if (serviceThread != null)
                {
                    Log.RegistraInformacao(MethodBase.GetCurrentMethod(), "OnStart - Estado da thread do serviço:" + serviceThread.ThreadState.ToString());
                }
            }
            catch (Exception ex)
            {
                Log.RegistraErro(MethodBase.GetCurrentMethod(), "OnStart - Erro", ex);
                Stop();
            }
        }

        protected override void OnStop()
        {
            Finalizar(Event.OnStop);
        }

        protected override void OnShutdown()
        {
            Finalizar(Event.OnShutdown);
        }

        protected enum Event
        {
            OnStart,
            OnStop,
            OnShutdown
        }

        #endregion

        #region Private Methods

        private void ThreadServico()
        {
            try
            {
                Log.RegistraInformacao(MethodBase.GetCurrentMethod(), "Início da thread rotina ThreadServico");

                var periodoSono = ConfigManager.PeriodoSono;
                var periodoSonoErro = ConfigManager.PeriodoSonoErro;

                Log.RegistraInformacao(MethodBase.GetCurrentMethod(), string.Format("Carrega valores do arquivo de configuração. Período de execução: uma vez a cada {0} horas, em caso de erro {1} horas.", periodoSono, periodoSonoErro));

                var periodo = new TimeSpan(0, 0, 0);

                while (!bFinalizar)
                {
                    // Container tem que ser inicializado fora da classe worker porque se ocorrer um erro dentro do worker a 
                    // função de registra mensagem necessita do contexto para construir a mensagem de erro
                    //var container = SimpleInjectorExeInitializer.Initialize(); // TODO: Inicializar contexto do SimpleInjector
                    //using (container.BeginExecutionContextScope())
                    {
                        try
                        {
                            ConfigManager.AtualizarAppSettings();

                            // Se após o processamento não ocorrer erros dorme por X horas
                            periodo = new TimeSpan(periodoSono, 0, 0);
                            executor = executor ?? new Executor();
                            executor.Executa();
                        }
                        catch (Exception ex1)
                        {
                            try
                            {
                                // Se ocorreu erro dorme por X horass
                                periodo = new TimeSpan(periodoSonoErro, 0, 0);

                                executor = null;
                                GC.Collect();

                                Log.RegistraErro(MethodBase.GetCurrentMethod(), "Erro na rotina ThreadServico em ex1", ex1);
                            }
                            catch (ThreadInterruptedException)
                            {
                                // Sono foi interrompido pelo shutdown do serviço
                            }
                        }
                    }

                    if (!bFinalizar)
                    {
                        try
                        {
                            Log.RegistraInformacao(MethodBase.GetCurrentMethod(), string.Format("Aguardando {0} horas(s)", periodo.Hours.ToString()));
                            Thread.Sleep(periodo);
                        }
                        catch (ThreadInterruptedException)
                        {
                            // Sono foi interrompido pelo shutdown do serviço
                        }
                    }
                }

                Log.RegistraInformacao(MethodBase.GetCurrentMethod(), "Finalizando thread rotina ThreadServico");
            }
            catch (Exception ex2)
            {
                Log.RegistraErro(MethodBase.GetCurrentMethod(), "Erro na rotina ThreadServico em ex2", ex2);
                Stop();
            }
        }

        private void Finalizar(Event op)
        {
            var sMetodo = string.Empty;

            switch (op)
            {
                case Event.OnStart:
                    sMetodo = "OnStart";
                    break;
                case Event.OnStop:
                    sMetodo = "OnStop";
                    break;
                case Event.OnShutdown:
                    sMetodo = "OnShutdown";
                    break;
                default:
                    break;
            }

            try
            {
                Log.RegistraInformacao(MethodBase.GetCurrentMethod(), sMetodo + " - Início do método");

                // Sinaliza o final do trabalho para as threads
                bFinalizar = true;

                // Solicita mais 5 minutos para finalizar tarefas em andamento
                RequestAdditionalTime(5 * 1000);
                Log.RegistraInformacao(MethodBase.GetCurrentMethod(), sMetodo + " - Solicitou tempo adicional de 5 minutos");

                if (serviceThread != null)
                {
                    Log.RegistraInformacao(MethodBase.GetCurrentMethod(), sMetodo + " - Estado da thread do serviço:" + serviceThread.ThreadState.ToString());
                }

                // Se a thread está dormindo, interrompe o sono para que ela finalize normamente
                Log.RegistraInformacao(MethodBase.GetCurrentMethod(), sMetodo + " - Sinaliza interrupção da thread do serviço quanto está em Sleep");
                if (serviceThread != null && serviceThread.ThreadState == ThreadState.WaitSleepJoin)
                {
                    serviceThread.Interrupt();
                }

                // Aguarda a finalização das threads pelas vias normais por 60 segundos, visando não interromper as cargas no meio, caso contrário faz o abort
                var espera = 60;
                Log.RegistraInformacao(MethodBase.GetCurrentMethod(), sMetodo + " - Aguardando join da thread do serviço");
                if (serviceThread != null)
                {
                    serviceThread.Join(new TimeSpan(0, 0, espera));
                }

                Log.RegistraInformacao(MethodBase.GetCurrentMethod(), sMetodo + " - Thread finalizada");
                if (serviceThread != null && (serviceThread.ThreadState == ThreadState.Running || serviceThread.ThreadState == ThreadState.WaitSleepJoin || serviceThread.ThreadState == ThreadState.Background))
                {
                    Log.RegistraInformacao(MethodBase.GetCurrentMethod(), sMetodo + " - Thread do serviço continua executando então será abortada");
                    serviceThread.Abort();
                }

                if (serviceThread != null)
                {
                    Log.RegistraInformacao(MethodBase.GetCurrentMethod(), sMetodo + " - Estado da thread do serviço:" + serviceThread.ThreadState.ToString());
                }

                ExitCode = 0;
            }
            catch (Exception ex)
            {
                Log.RegistraErro(MethodBase.GetCurrentMethod(), sMetodo + " - Erro", ex);
                ExitCode = 999;
            }
        }

        #endregion
    }
}