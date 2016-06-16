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
    public class FonteAplServico : AplServico, IFonteAplServico
    {
        private IFonteServico _servico;

        public FonteAplServico(IFonteServico FonteServico)
        {
            _servico = FonteServico;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Inclui(FonteDTO dto)
        {
            Transacao.Begin();

            var ent = Mapeador.Map<Fonte>(dto);
            _servico.Inclui(ent);

            Transacao.Commit();
            dto.Id = ent.Id;
        }

        public void Inclui(IEnumerable<FonteDTO> dtos)
        {
            Transacao.Begin();

            var entidades = new List<Fonte>();
            foreach (var dto in dtos)
            {
                entidades.Add(Mapeador.Map<Fonte>(dto));
            }

            _servico.Inclui(entidades);

            Transacao.Commit();
        }

        public void Altera(FonteDTO dto)
        {
            Transacao.Begin();

            var ent = _servico.Consulta(dto.Id);
            _servico.Altera(Mapeador.Map(dto, ent));

            Transacao.Commit();
        }

        public void Exclui(FonteDTO dto)
        {
            throw new NotImplementedException();
        }

        public FonteDTO Consulta(int id)
        {
            return Mapeador.Map<FonteDTO>(_servico.Consulta(id));
        }

        public IEnumerable<FonteDTO> ListaTodos()
        {
            return Mapeador.Map<IEnumerable<FonteDTO>>(_servico.ListaTodos());
        }

        public IEnumerable<FonteDTO> Lista(FonteDTO filtro)
        {
            var lista = _servico.Lista(Expressao.CriaExpressao<Fonte>(Filtro.Filtros(Mapeador.Map<Fonte>(filtro))));
            return Mapeador.Map<IEnumerable<FonteDTO>>(lista);
        }

        public bool Existe(FonteDTO filtro)
        {
            return _servico.Existe(Expressao.CriaExpressao<Fonte>(Filtro.Filtros(Mapeador.Map<Fonte>(filtro))));
        }
    }
}