﻿using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace EduCon.Utilitarios.Contexto.Interfaces
{
    public interface IContexto
    {
        DbSet<T> Set<T>() where T : class;
        DbEntityEntry<T> Entry<T>(T entidade) where T : class;

        int SaveChanges();

        void DiscardChanges();

        void Dispose();
    }
}