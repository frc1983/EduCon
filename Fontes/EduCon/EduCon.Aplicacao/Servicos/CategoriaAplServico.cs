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
    public class CategoriaAplServico : AplServico, ICategoriaAplServico
    {
        private ICategoriaServico _servico;

        public CategoriaAplServico(ICategoriaServico CategoriaServico)
        {
            _servico = CategoriaServico;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Inclui(CategoriaDTO dto)
        {
            Transacao.Begin();

            var ent = Mapeador.Map<Categoria>(dto);
            _servico.Inclui(ent);

            Transacao.Commit();
            dto.Id = ent.Id;
        }

        public void Inclui(IEnumerable<CategoriaDTO> dtos)
        {
            Transacao.Begin();

            var entidades = new List<Categoria>();
            foreach (var dto in dtos)
            {
                entidades.Add(Mapeador.Map<Categoria>(dto));
            }

            _servico.Inclui(entidades);

            Transacao.Commit();
        }

        public void Altera(CategoriaDTO dto)
        {
            Transacao.Begin();

            var ent = _servico.Consulta(dto.Id);
            _servico.Altera(Mapeador.Map(dto, ent));

            Transacao.Commit();
        }

        public void Exclui(CategoriaDTO dto)
        {
            throw new NotImplementedException();
        }

        public CategoriaDTO Consulta(int id)
        {
            return Mapeador.Map<CategoriaDTO>(_servico.Consulta(id));
        }

        public IEnumerable<CategoriaDTO> ListaTodos()
        {
            return Mapeador.Map<IEnumerable<CategoriaDTO>>(_servico.ListaTodos());
        }

        public IEnumerable<CategoriaDTO> ListaCategorias()
        {
            return Mapeador.Map<IEnumerable<CategoriaDTO>>(_servico.ListaCategorias());
        }

        public IEnumerable<CategoriaDTO> ListaSubcategorias()
        {
            return Mapeador.Map<IEnumerable<CategoriaDTO>>(_servico.ListaSubcategorias());
        }

        public IEnumerable<CategoriaDTO> Lista(CategoriaDTO filtro)
        {
            var lista = _servico.Lista(Expressao.CriaExpressao<Categoria>(Filtro.Filtros(Mapeador.Map<Categoria>(filtro))));
            return Mapeador.Map<IEnumerable<CategoriaDTO>>(lista);
        }

        public bool Existe(CategoriaDTO filtro)
        {
            return _servico.Existe(Expressao.CriaExpressao<Categoria>(Filtro.Filtros(Mapeador.Map<Categoria>(filtro))));
        }
    }
}