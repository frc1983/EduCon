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

            var ent = Mapeador.Map<Dado>(dto);
            _servico.Inclui(ent);

            Transacao.Commit();
            dto.Id = ent.Id;
        }

        public void Inclui(IEnumerable<DadoDTO> dtos)
        {
            Transacao.Begin();

            var entidades = new List<Dado>();
            foreach (var dto in dtos)
            {
                entidades.Add(Mapeador.Map<Dado>(dto));
            }
            
            _servico.Inclui(entidades);

            Transacao.Commit();
        }

        public void Altera(DadoDTO dto)
        {
            Transacao.Begin();

            var ent = _servico.Consulta(dto.Id);
            _servico.Altera(Mapeador.Map(dto, ent));

            Transacao.Commit();
        }

        public void Exclui(DadoDTO dto)
        {
            throw new NotImplementedException();
        }

        public DadoDTO Consulta(int id)
        {
            return Mapeador.Map<DadoDTO>(_servico.Consulta(id));
        }

        public IEnumerable<DadoDTO> ListaTodos()
        {
            return Mapeador.Map<IEnumerable<DadoDTO>>(_servico.ListaTodos());
        }

        public IEnumerable<DadoDTO> Lista(DadoDTO filtro)
        {
            var lista = _servico.Lista(Expressao.CriaExpressao<Dado>(Filtro.Filtros(Mapeador.Map<Dado>(filtro))));
            return Mapeador.Map<IEnumerable<DadoDTO>>(lista);
        }

        public IEnumerable<DadoOLAP> ListaOlap()
        {
            return _servico.ListaOlap();
        }

        public bool Existe(DadoDTO filtro)
        {
            return _servico.Existe(Expressao.CriaExpressao<Dado>(Filtro.Filtros(Mapeador.Map<Dado>(filtro))));
        }
    }
}