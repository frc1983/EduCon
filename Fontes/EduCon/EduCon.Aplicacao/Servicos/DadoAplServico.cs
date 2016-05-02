using System;
using System.Collections.Generic;
using EduCon.Aplicacao.Interfaces;
using EduCon.Aplicacao.Servicos.Base;
using EduCon.Dominio.Interfaces.Servico;
using EduCon.Objetos.DTOs;
using EduCon.Objetos.Entidades;
using EduCon.Utilitarios.Aplicacao.Utilitarios;

namespace EduCon.Aplicacao.Servicos
{
    public class DadoAplServico : AplServico, IDadoAplServico
    {
        private IDadoServico _servico;

        public DadoAplServico(IDadoServico DadoServico)
        {
            _servico = DadoServico;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Inclui(DadoDTO dto)
        {
            Transacao.Begin();

            var ent = Mapper.Map<Dado>(dto);
            _servico.Inclui(ent);

            Transacao.Commit();
            dto.Id = ent.Id;
        }

        public void Altera(DadoDTO dto)
        {
            Transacao.Begin();

            var ent = _servico.Consulta(dto.Id);
            _servico.Altera(Mapper.Map(dto, ent));

            Transacao.Commit();
        }

        public void Exclui(DadoDTO dto)
        {
            throw new NotImplementedException();
        }

        public DadoDTO Consulta(int id)
        {
            return Mapper.Map<DadoDTO>(_servico.Consulta(id));
        }

        public IEnumerable<DadoDTO> ListaTodos()
        {
            return Mapper.Map<IEnumerable<DadoDTO>>(_servico.ListaTodos());
        }

        public IEnumerable<DadoDTO> Lista(DadoDTO filtro)
        {
            var lista = _servico.Lista(Expressao.CriaExpressao<Dado>(Filtro.Filtros(Mapper.Map<Dado>(filtro))));
            return Mapper.Map<IEnumerable<DadoDTO>>(lista);
        }
    }
}