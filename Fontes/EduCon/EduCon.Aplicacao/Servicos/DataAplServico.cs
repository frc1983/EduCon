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
    public class DataAplServico : AplServico, IDataAplServico
    {
        private IDataServico _servico;

        public DataAplServico(IDataServico DataServico)
        {
            _servico = DataServico;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Inclui(DataDTO dto)
        {
            Transacao.Begin();

            var ent = Mapeador.Map<Data>(dto);
            _servico.Inclui(ent);

            Transacao.Commit();
            dto.Id = ent.Id;
        }

        public void Inclui(IEnumerable<DataDTO> dtos)
        {
            Transacao.Begin();

            var entidades = new List<Data>();
            foreach (var dto in dtos)
            {
                entidades.Add(Mapeador.Map<Data>(dto));
            }

            _servico.Inclui(entidades);

            Transacao.Commit();
        }

        public void Altera(DataDTO dto)
        {
            Transacao.Begin();

            var ent = _servico.Consulta(dto.Id);
            _servico.Altera(Mapeador.Map(dto, ent));

            Transacao.Commit();
        }

        public void Exclui(DataDTO dto)
        {
            throw new NotImplementedException();
        }

        public DataDTO Consulta(int id)
        {
            return Mapeador.Map<DataDTO>(_servico.Consulta(id));
        }

        public IEnumerable<DataDTO> ListaTodos()
        {
            return Mapeador.Map<IEnumerable<DataDTO>>(_servico.ListaTodos());
        }

        public IEnumerable<DataDTO> Lista(DataDTO filtro)
        {
            var lista = _servico.Lista(Expressao.CriaExpressao<Data>(Filtro.Filtros(Mapeador.Map<Data>(filtro))));
            return Mapeador.Map<IEnumerable<DataDTO>>(lista);
        }
    }
}