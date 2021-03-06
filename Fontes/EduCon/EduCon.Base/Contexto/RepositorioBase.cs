﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using EduCon.Base.Contexto.Interfaces;
using EduCon.Base.Dominio.Interfaces;
using Microsoft.Practices.ServiceLocation;

namespace EduCon.Base.Contexto
{
    public class RepositorioBase<T> : IRepositorio<T>, IDisposable
        where T : class
    {
        private readonly IContexto _contexto;
        private readonly IDbSet<T> _dbSet;

        protected IContexto Contexto
        {
            get { return _contexto; }
        }

        protected IDbSet<T> DbSet
        {
            get { return _dbSet; }
        }

        public RepositorioBase()
        {
            _contexto = ServiceLocator.Current.GetInstance<IContexto>();
            _dbSet = _contexto.Set<T>();
        }

        #region Inclusão, alteração e exclusão

        public virtual void Inclui(T entidade)
        {
            DbSet.Add(entidade);
        }

        public virtual void Inclui(IEnumerable<T> entidades)
        {
            Contexto.Configuration.AutoDetectChangesEnabled = false;
            Contexto.Configuration.ValidateOnSaveEnabled = false;

            //foreach (var entidade in entidades)
            //{
            //    DbSet.Add(entidade);
            //}

            Contexto.InsertRange<T>(entidades);

            Contexto.Configuration.AutoDetectChangesEnabled = true;
            Contexto.Configuration.ValidateOnSaveEnabled = true;
        }

        public virtual void Altera(T entidade)
        {
            var entry = Contexto.Entry(entidade);
            DbSet.Attach(entidade);
            entry.State = EntityState.Modified;
        }

        public virtual void Exclui(T entidade)
        {
            DbSet.Remove(entidade);
        }

        #endregion

        #region Consulta

        public T Consulta(int id)
        {
            return DbSet.Find(id);
        }

        public T Consulta(Expression<Func<T, bool>> expressao)
        {
            return DbSet.Where(expressao).SingleOrDefault();
        }

        #endregion

        #region Lista

        public virtual IEnumerable<T> ListaTodos()
        {
            return DbSet.ToList();
        }

        public virtual IEnumerable<T> Lista(Expression<Func<T, bool>> expressao)
        {
            return DbSet.Where(expressao).ToList();
        }

        #endregion

        #region Informações

        public virtual bool Existe(Expression<Func<T, bool>> expressao)
        {
            return DbSet.Any(expressao);
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (Contexto == null)
                return;

            Contexto.Dispose();
        }

        #endregion
    }
}