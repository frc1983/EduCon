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
    public class TipoEnsinoAplServico : AplServico, ITipoEnsinoAplServico
    {
        private ITipoEnsinoServico _servico;

        public TipoEnsinoAplServico(ITipoEnsinoServico TipoEnsinoServico)
        {
            _servico = TipoEnsinoServico;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Inclui(TipoEnsinoDTO dto)
        {
            Transacao.Begin();

            var ent = Mapper.Map<TipoEnsino>(dto);
            _servico.Inclui(ent);

            Transacao.Commit();
            dto.Id = ent.Id;
        }

        public void Altera(TipoEnsinoDTO dto)
        {
            Transacao.Begin();

            var ent = _servico.Consulta(dto.Id);
            _servico.Altera(Mapper.Map(dto, ent));

            Transacao.Commit();
        }

        public void Exclui(TipoEnsinoDTO dto)
        {
            throw new NotImplementedException();
        }

        public TipoEnsinoDTO Consulta(int id)
        {
            return Mapper.Map<TipoEnsinoDTO>(_servico.Consulta(id));
        }

        public IEnumerable<TipoEnsinoDTO> ListaTodos()
        {
            return Mapper.Map<IEnumerable<TipoEnsinoDTO>>(_servico.ListaTodos());
        }

        public IEnumerable<TipoEnsinoDTO> Lista(TipoEnsinoDTO filtro)
        {
            var lista = _servico.Lista(Expressao.CriaExpressao<TipoEnsino>(Filtro.Filtros(Mapper.Map<TipoEnsino>(filtro))));
            return Mapper.Map<IEnumerable<TipoEnsinoDTO>>(lista);
        }
    }
}