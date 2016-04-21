﻿using System;
using EduCon.Utilitarios.Contexto.Interfaces;
using EduCon.Utilitarios.Dominio.Interfaces;
using Microsoft.Practices.ServiceLocation;

namespace EduCon.Utilitarios.Dominio
{
    public class Transacao : ITransacao
    {
        private readonly IContexto _contexto;
        private bool _disposed;

        public Transacao()
        {
            _contexto = ServiceLocator.Current.GetInstance<IContexto>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Begin()
        {
            _disposed = false;
        }

        public void Commit()
        {
            _contexto.SaveChanges();
        }

        public void Rollback()
        {
            _contexto.DiscardChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _contexto.Dispose();
            }

            _disposed = true;
        }
    }
}