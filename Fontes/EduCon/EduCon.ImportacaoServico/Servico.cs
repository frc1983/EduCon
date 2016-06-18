using System;
using System.ServiceProcess;
using System.Threading;
using EduCon.ImportacaoServico.Infra;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace EduCon.ImportacaoServico
{
    public partial class Servico : ServiceBase
    {
        private bool finaliza = false;

        private Thread serviceThread;
        private Importador importador;

        public Servico()
        {
            InitializeComponent();
        }

        #region Eventos

        protected override void OnStart(string[] args)
        {
            try
            {
                finaliza = false;

                // Estado tem que ser Unstarted ou Stopped
                if (serviceThread == null || (serviceThread.ThreadState != ThreadState.Unstarted && serviceThread.ThreadState != ThreadState.Stopped))
                {
                    serviceThread = new Thread(new ThreadStart(ThreadServico));
                    serviceThread.Start();
                }
            }
            catch (Exception ex)
            {
                this.Stop();
            }
        }

        protected override void OnStop()
        {
            Finalizar(Evento.Parar);
        }

        protected override void OnShutdown()
        {
            Finalizar(Evento.Encerrar);
        }

        #endregion

        #region Private Methods

        private void ThreadServico()
        {
            try
            {
                var periodoMinutos = Configuracao.PeriodoMinutos;
                var periodoMinutosErro = Configuracao.PeriodoMinutosErro;

                var periodo = new TimeSpan(0, 0, 0);

                while (!finaliza)
                {
                    var container = Infra.Injecao.Inicializa();
                    using (container.BeginExecutionContextScope())
                    {
                        try
                        {
                            Configuracao.AtualizaConfiguracoes();

                            // Se após o processamento não ocorrer erros dorme por X minutos
                            periodo = new TimeSpan(0, periodoMinutos, 0);
                            importador = importador ?? new Importador();
                            importador.Importa();
                        }
                        catch (Exception ex1)
                        {
                            try
                            {
                                // Se ocorreu erro dorme por X minutos
                                periodo = new TimeSpan(0, periodoMinutosErro, 0);

                                importador = null;
                                GC.Collect();
                            }
                            catch (ThreadInterruptedException)
                            {
                                // Sono foi interrompido pelo shutdown do serviço
                            }
                        }
                    }

                    if (!finaliza)
                    {
                        try
                        {
                            Thread.Sleep(periodo);
                        }
                        catch (ThreadInterruptedException)
                        {
                            // Sono foi interrompido pelo shutdown do serviço
                        }
                    }
                }
            }
            catch (Exception ex2)
            {
                Stop();
            }
        }

        private void Finalizar(Evento evento)
        {
            var sMetodo = string.Empty;
            switch (evento)
            {
                case Evento.Iniciar:
                    sMetodo = "OnStart";
                    break;
                case Evento.Parar:
                    sMetodo = "OnStop";
                    break;
                case Evento.Encerrar:
                    sMetodo = "OnShutdown";
                    break;
                default:
                    break;
            }

            try
            {
                // Sinaliza o final do trabalho para as threads
                finaliza = true;

                // Solicita mais 1 minuto para finalizar tarefas em andamento
                RequestAdditionalTime(60000);

                // Se a thread está dormindo, interrompe o sono para que ela finalize normamente
                if (serviceThread != null && serviceThread.ThreadState == ThreadState.WaitSleepJoin)
                {
                    serviceThread.Interrupt();
                }

                // Aguarda a finalização das threads pelas vias normais por 55 segundos, visando não interromper as cargas no meio, caso contrário faz o abort
                var lEsperaSeg = 55;
                if (serviceThread != null)
                {
                    serviceThread.Join(new TimeSpan(0, 0, lEsperaSeg));
                }

                if (serviceThread != null && (serviceThread.ThreadState == ThreadState.Running || serviceThread.ThreadState == ThreadState.WaitSleepJoin || serviceThread.ThreadState == ThreadState.Background))
                {
                    serviceThread.Abort();
                }

                ExitCode = 0;
            }
            catch (Exception ex)
            {
                ExitCode = 999;
            }
        }

        #endregion
    }
}