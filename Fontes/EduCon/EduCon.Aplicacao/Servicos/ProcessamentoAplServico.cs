using System;
using System.Collections.Generic;
using EduCon.Aplicacao.Interfaces;
using EduCon.Aplicacao.Servicos.Base;
using EduCon.Dominio.Entidades;
using EduCon.Dominio.Entidades.Enums;
using EduCon.Dominio.Interfaces.Servico;
using EduCon.Objetos.DTOs;
using EduCon.Utilitarios.Aplicacao;

namespace EduCon.Aplicacao.Servicos
{
    public class ProcessamentoAplServico : AplServico, IProcessamentoAplServico
    {
        private IProcessamentoServico _servico;

        public ProcessamentoAplServico(IProcessamentoServico processamentoServico)
        {
            _servico = processamentoServico;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Inclui(ProcessamentoDTO dto)
        {
            Transacao.Begin();

            var ent = Mapeador.Map<Processamento>(dto);
            _servico.Inclui(ent);

            Transacao.Commit();
            dto.Id = ent.Id;
        }

        public void Inclui(IEnumerable<ProcessamentoDTO> dtos)
        {
            Transacao.Begin();

            var entidades = new List<Processamento>();
            foreach (var dto in dtos)
            {
                entidades.Add(Mapeador.Map<Processamento>(dto));
            }

            _servico.Inclui(entidades);

            Transacao.Commit();
        }

        public void Altera(ProcessamentoDTO dto)
        {
            Transacao.Begin();

            var ent = _servico.Consulta(dto.Id);
            _servico.Altera(Mapeador.Map(dto, ent));

            Transacao.Commit();
        }

        public void Processando(ProcessamentoDTO dto)
        {
            Transacao.Begin();

            var ent = _servico.Consulta(dto.Id);
            _servico.Processando(ent);

            Transacao.Commit();
        }

        public void Processado(ProcessamentoDTO dto)
        {
            Transacao.Begin();

            var ent = _servico.Consulta(dto.Id);
            _servico.Processado(Mapeador.Map(dto, ent));

            Transacao.Commit();
        }

        public void Reprocessar(ProcessamentoDTO dto)
        {
            Transacao.Begin();

            var ent = _servico.Consulta(dto.Id);
            _servico.Reprocessar(ent);

            Transacao.Commit();
        }

        public void Exclui(ProcessamentoDTO dto)
        {
            throw new NotImplementedException();
        }

        public ProcessamentoDTO Consulta(int id)
        {
            return Mapeador.Map<ProcessamentoDTO>(_servico.Consulta(id));
        }

        public IEnumerable<ProcessamentoDTO> ListaTodos()
        {
            return Mapeador.Map<IEnumerable<ProcessamentoDTO>>(_servico.ListaTodos());
        }

        public IEnumerable<ProcessamentoDTO> ListaProcessar(bool reprocessar = true)
        {
            return Mapeador.Map<IEnumerable<ProcessamentoDTO>>(_servico.Lista(o => o.Situacao == SituacaoProcessamento.Aguardando
                || o.Situacao == SituacaoProcessamento.Reprocessar));
        }

        public IEnumerable<ProcessamentoDTO> Lista(ProcessamentoDTO filtro)
        {
            var lista = _servico.Lista(Expressao.CriaExpressao<Processamento>(Filtro.Filtros(Mapeador.Map<Processamento>(filtro))));
            return Mapeador.Map<IEnumerable<ProcessamentoDTO>>(lista);
        }

        public bool Existe(ProcessamentoDTO filtro)
        {
            return _servico.Existe(Expressao.CriaExpressao<Processamento>(Filtro.Filtros(Mapeador.Map<Processamento>(filtro))));
        }
    }
}