using System;
using System.Collections.Generic;
using System.Linq;
using EduCon.Base.Dominio;
using EduCon.Base.Dominio.Interfaces;
using EduCon.Dominio.Entidades;
using EduCon.Dominio.Entidades.Enums;
using EduCon.Dominio.Interfaces.Servico;

namespace EduCon.Dominio.Servicos
{
    public class ProcessamentoServico : Servico<Processamento>, IProcessamentoServico
    {
        public ProcessamentoServico(IRepositorio<Processamento> repositorio) : base(repositorio)
        {
        }

        public override void Inclui(Processamento entidade)
        {
            entidade.Situacao = SituacaoProcessamento.Aguardando;
            base.Inclui(entidade);
        }

        public override void Inclui(IEnumerable<Processamento> entidades)
        {
            entidades.ToList().ForEach(o => o.Situacao = SituacaoProcessamento.Aguardando);
            base.Inclui(entidades);
        }

        public void Processando(Processamento entidade)
        {
            if (entidade.Situacao == SituacaoProcessamento.Aguardando)
            {
                entidade.Situacao = SituacaoProcessamento.Processando;
            }
            else if (entidade.Situacao == SituacaoProcessamento.Reprocessar)
            {
                entidade.Situacao = SituacaoProcessamento.Reprocessando;
            }

            base.Altera(entidade);
        }

        public void Processado(Processamento entidade)
        {
            entidade.Data = DateTime.Now;
            if (entidade.Situacao == SituacaoProcessamento.Processando)
            {
                entidade.Situacao = SituacaoProcessamento.Processado;
            }
            else if (entidade.Situacao == SituacaoProcessamento.Reprocessando)
            {
                entidade.Situacao = SituacaoProcessamento.Reprocessado;
            }

            base.Altera(entidade);
        }

        public void Reprocessar(Processamento entidade)
        {
            entidade.Situacao = SituacaoProcessamento.Reprocessar;
            base.Altera(entidade);
        }
    }
}