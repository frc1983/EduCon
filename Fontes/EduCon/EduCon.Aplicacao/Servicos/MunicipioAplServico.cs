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

            var ent = Mapeador.Map<Municipio>(dto);
            _servico.Inclui(ent);

            Transacao.Commit();
            dto.Id = ent.Id;
        }

        public void Inclui(IEnumerable<MunicipioDTO> dtos)
        {
            Transacao.Begin();

            var entidades = new List<Municipio>();
            foreach (var dto in dtos)
            {
                entidades.Add(Mapeador.Map<Municipio>(dto));
            }

            _servico.Inclui(entidades);

            Transacao.Commit();
        }

        public void Altera(MunicipioDTO dto)
        {
            Transacao.Begin();

            var ent = _servico.Consulta(dto.Id);
            _servico.Altera(Mapeador.Map(dto, ent));

            Transacao.Commit();
        }

        public void Exclui(MunicipioDTO dto)
        {
            throw new NotImplementedException();
        }

        public MunicipioDTO Consulta(int id)
        {
            return Mapeador.Map<MunicipioDTO>(_servico.Consulta(id));
        }

        public IEnumerable<MunicipioDTO> ListaTodos()
        {
            return Mapeador.Map<IEnumerable<MunicipioDTO>>(_servico.ListaTodos());
        }

        public IEnumerable<MunicipioDTO> Lista(MunicipioDTO filtro)
        {
            var lista = _servico.Lista(Expressao.CriaExpressao<Municipio>(Filtro.Filtros(Mapeador.Map<Municipio>(filtro))));
            return Mapeador.Map<IEnumerable<MunicipioDTO>>(lista);
        }

        public MunicipioDTO ConsultaPorNome(string municipio)
        {
            return Mapeador.Map<MunicipioDTO>(_servico.ConsultaPorNome(municipio));
        }
    }
}