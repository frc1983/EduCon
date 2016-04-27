using System;
using System.Collections.Generic;
using EduCon.Aplicacao.Interfaces;
using EduCon.Aplicacao.Servicos.Base;
using EduCon.Dominio.Interfaces.Servico;
using EduCon.Objetos.DTOs;
using EduCon.Objetos.Entidades;

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

            var ent = Mapper.Map<Data>(dto);
            _servico.Inclui(ent);

            Transacao.Commit();
            dto.Id = ent.Id;
        }

        public void Altera(DataDTO dto)
        {
            Transacao.Begin();

            var ent = _servico.Consulta(dto.Id);
            _servico.Altera(Mapper.Map(dto, ent));

            Transacao.Commit();
        }

        public void Exclui(DataDTO dto)
        {
            throw new NotImplementedException();
        }

        public DataDTO Consulta(int id)
        {
            return Mapper.Map<DataDTO>(_servico.Consulta(id));
        }

        public IEnumerable<DataDTO> ListaTodos()
        {
            return Mapper.Map<IEnumerable<DataDTO>>(_servico.ListaTodos());
        }

        public IEnumerable<DataDTO> Lista(DataDTO filtro)
        {
            var lista = _servico.Lista(null);
            return Mapper.Map<IEnumerable<DataDTO>>(lista);
        }
    }
}