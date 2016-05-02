using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EduCon.Utilitarios.Dominio.Interfaces;

namespace EduCon.Utilitarios.Dominio
{
    public class Servico<T> : IServico<T>
        where T : class
    {
        private readonly IRepositorio<T> _repositorio;

        protected IRepositorio<T> Repositorio
        {
            get { return _repositorio; }
        }

        public Servico(IRepositorio<T> repositorio)
        {
            _repositorio = repositorio;
        }

        #region Inclusão, alteração e exclusão

        public virtual void Inclui(T entidade)
        {
            if (entidade == null)
            {
                throw new ArgumentException("Não é possível incluir uma entidade vazia", "entidade");
            }

            _repositorio.Inclui(entidade);
        }

        public virtual void Inclui(IEnumerable<T> entidades)
        {
            if (entidades == null || entidades.Count() == 0)
            {
                throw new ArgumentException("Não é possível incluir uma lista de entidades vazia", "entidade");
            }

            _repositorio.Inclui(entidades);
        }

        public virtual void Altera(T entidade)
        {
            if (entidade == null)
            {
                throw new ArgumentException("Não é possível alterar uma entidade vazia", "entidade");
            }

            _repositorio.Altera(entidade);
        }

        public virtual void Exclui(T entidade)
        {
            if (entidade == null)
            {
                throw new ArgumentException("Não é possível excluir uma entidade vazia", "entidade");
            }

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
    }
}