using System;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;
using Microsoft.Practices.ServiceLocation;

namespace EduCon.ImportacaoServico.Infra
{
    public class Importador
    {
        private IProcessamentoAplServico _servico;

        private ImportadorArquivos importadorArquivos;

        public Importador()
        {
            _servico = ServiceLocator.Current.GetInstance<IProcessamentoAplServico>();

            importadorArquivos = new ImportadorArquivos();
        }

        public void Importa()
        {
            ProcessamentoDTO processamento = null;

            var processaveis = _servico.ListaProcessar();

            try
            {
                foreach (var proc in processaveis)
                {
                    _servico.Processando(proc);
                    processamento = _servico.Consulta(proc.Id);

                    importadorArquivos.ImportaArquivos(processamento);

                    _servico.Processado(processamento);
                }
            }
            catch (Exception)
            {
                if (processamento != null)
                {
                    // TODO: Melhorar implementação
                    processamento.CodSituacao = 1;
                    _servico.Altera(processamento);
                }

                throw;
            }
        }
    }
}