using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EduCon.Base.Dominio.Interfaces;
using EduCon.Base.Dominio.Interfaces.Validacoes;
using EduCon.Base.Dominio.Validadores;

namespace EduCon.Base.Dominio
{
    public class Servico<T> : IServico<T>
        where T : class
    {
        private readonly IRepositorio<T> _repositorio;

        private readonly IList<IRegraValidacao<T>> _regras;

        private readonly IList<IRegraValidacao<T>> _regrasEspec;

        protected IRepositorio<T> Repositorio
        {
            get { return _repositorio; }
        }

        public Servico(IRepositorio<T> repositorio)
        {
            _repositorio = repositorio;
            _regras = new List<IRegraValidacao<T>>();
            _regrasEspec = new List<IRegraValidacao<T>>();
        }

        #region Inclusão, alteração e exclusão

        public virtual void Inclui(T entidade)
        {
            InsereValidacoes();

            ExecutaValidacoes(entidade, TipoOperacao.Inclusao);

            _repositorio.Inclui(entidade);
        }

        public virtual void Inclui(IEnumerable<T> entidades)
        {
            InsereValidacoes();

            _repositorio.Inclui(entidades);
        }

        public virtual void Altera(T entidade)
        {
            InsereValidacoes();

            ExecutaValidacoes(entidade, TipoOperacao.Alteracao);

            _repositorio.Altera(entidade);
        }

        public virtual void Exclui(T entidade)
        {
            InsereValidacoes();

            ExecutaValidacoes(entidade, TipoOperacao.Exclusao);

            _repositorio.Exclui(entidade);
        }

        #endregion

        #region Consulta

        public virtual T Consulta(int id)
        {
            return _repositorio.Consulta(id);
        }

        public virtual T Consulta(Expression<Func<T, bool>> predicado)
        {
            return _repositorio.Consulta(predicado);
        }

        #endregion

        #region Lista

        public virtual IEnumerable<T> ListaTodos()
        {
            return _repositorio.ListaTodos();
        }

        public virtual IEnumerable<T> Lista(Expression<Func<T, bool>> expressao)
        {
            return _repositorio.Lista(expressao);
        }

        #endregion

        #region Informações

        public virtual bool Existe(Expression<Func<T, bool>> expressao)
        {
            return _repositorio.Existe(expressao);
        }

        #endregion

        #region Regras

        protected void AdicionaRegra(IRegraValidacao<T> regra)
        {
            if (regra == null)
                throw new ArgumentNullException("Regra não informada.");

            _regrasEspec.Add(regra);
        }

        protected void LimpaRegras()
        {
            _regrasEspec.Clear();
        }

        #endregion

        #region Métodos Privados

        private void InsereValidacoes()
        {
            _regras.Clear();
            _regras.Add(new EntidadeNula<T>());

            InsereValidacoesEspecificas();
        }

        private void InsereValidacoesEspecificas()
        {
            foreach (var regra in _regrasEspec)
            {
                _regras.Add(regra);
            }
        }

        private void ExecutaValidacoes(T entidade, TipoOperacao operacao)
        {
            var validador = new Validador<T>(entidade);
            validador.Adiciona(_regras);
            validador.Executa(operacao);
        }

        #endregion
    }
}