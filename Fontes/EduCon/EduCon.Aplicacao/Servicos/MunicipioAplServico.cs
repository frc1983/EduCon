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
    public class MunicipioAplServico : AplServico, IMunicipioAplServico
    {
        private IMunicipioServico _servico;

        public MunicipioAplServico(IMunicipioServico municipioServico)
        {
            _servico = municipioServico;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Inclui(MunicipioDTO dto)
        {
            Transacao.Begin();

            var ent = Mapper.Map<Municipio>(dto);
            _servico.Inclui(ent);

            Transacao.Commit();
            dto.Id = ent.Id;
        }

        public void Altera(MunicipioDTO dto)
        {
            Transacao.Begin();

            var ent = _servico.Consulta(dto.Id);
            _servico.Altera(Mapper.Map(dto, ent));

            Transacao.Commit();
        }

        public void Exclui(MunicipioDTO dto)
        {
            throw new NotImplementedException();
        }

        public MunicipioDTO Consulta(int id)
        {
            return Mapper.Map<MunicipioDTO>(_servico.Consulta(id));
        }

        public IEnumerable<MunicipioDTO> ListaTodos()
        {
            return Mapper.Map<IEnumerable<MunicipioDTO>>(_servico.ListaTodos());
        }

        public IEnumerable<MunicipioDTO> Lista(MunicipioDTO filtro)
        {
            var lista = _servico.Lista(Expressao.CriaExpressao<Municipio>(Filtro.Filtros(Mapper.Map<Municipio>(filtro))));
            return Mapper.Map<IEnumerable<MunicipioDTO>>(lista);
        }
    }
}