using System;
using EduCon.Base.Dominio.Interfaces.Validacoes;
using EduCon.Dominio.Entidades;
using EduCon.Dominio.Entidades.Enums;

namespace EduCon.Dominio.Validacoes
{
    public class ImportacaoReprocessar : IValidaAltera<Processamento>
    {
        private SituacaoProcessamento _situacaoAnterior;

        public ImportacaoReprocessar(SituacaoProcessamento situacaoAnterior)
        {
            _situacaoAnterior = situacaoAnterior;
        }

        public void ValidaAltera(Processamento entidade)
        {
            var podeReprocessar = _situacaoAnterior == SituacaoProcessamento.Processado
               || _situacaoAnterior == SituacaoProcessamento.Reprocessado
               || _situacaoAnterior == SituacaoProcessamento.Erro;

            if (podeReprocessar)
            {
                return;
            }

            if (_situacaoAnterior == SituacaoProcessamento.Aguardando
                || _situacaoAnterior == SituacaoProcessamento.Reprocessar)
                throw new InvalidOperationException("Importação já agendada para processamento/reprocessamento.");

            if (_situacaoAnterior == SituacaoProcessamento.Processando
                || _situacaoAnterior == SituacaoProcessamento.Reprocessando)
                throw new InvalidOperationException("Importação já está em processamento.");
        }
    }
}