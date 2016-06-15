using System;
using System.Collections.Generic;
using EduCon.Aplicacao.Interfaces;
using EduCon.Aplicacao.Servicos.Base;
using EduCon.Dominio.Entidades;
using EduCon.Dominio.Interfaces.Servico;
using EduCon.Objetos.DTOs;
using EduCon.Utilitarios.Aplicacao;

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

            var ent = Mapeador.Map<TipoEnsino>(dto);
            _servico.Inclui(ent);

            Transacao.Commit();
            dto.Id = ent.Id;
        }

        public void Inclui(IEnumerable<TipoEnsinoDTO> dtos)
        {
            Transacao.Begin();

            var entidades = new List<TipoEnsino>();
            foreach (var dto in dtos)
            {
                entidades.Add(Mapeador.Map<TipoEnsino>(dto));
            }

            _servico.Inclui(entidades);

            Transacao.Commit();
        }

        public void Altera(TipoEnsinoDTO dto)
        {
            Transacao.Begin();

            var ent = _servico.Consulta(dto.Id);
            _servico.Altera(Mapeador.Map(dto, ent));

            Transacao.Commit();
        }

        public void Exclui(TipoEnsinoDTO dto)
        {
            throw new NotImplementedException();
        }

        public TipoEnsinoDTO Consulta(int id)
        {
            return Mapeador.Map<TipoEnsinoDTO>(_servico.Consulta(id));
        }

        public IEnumerable<TipoEnsinoDTO> ListaTodos()
        {
            return Mapeador.Map<IEnumerable<TipoEnsinoDTO>>(_servico.ListaTodos());
        }

        public IEnumerable<TipoEnsinoDTO> Lista(TipoEnsinoDTO filtro)
        {
            var lista = _servico.Lista(Expressao.CriaExpressao<TipoEnsino>(Filtro.Filtros(Mapeador.Map<TipoEnsino>(filtro))));
            return Mapeador.Map<IEnumerable<TipoEnsinoDTO>>(lista);
        }

        public bool Existe(TipoEnsinoDTO filtro)
        {
            return _servico.Existe(Expressao.CriaExpressao<TipoEnsino>(Filtro.Filtros(Mapeador.Map<TipoEnsino>(filtro))));
        }
    }
}