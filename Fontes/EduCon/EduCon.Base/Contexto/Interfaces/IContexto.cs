using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace EduCon.Base.Contexto.Interfaces
{
    public interface IContexto
    {
        DbContextConfiguration Configuration { get; }

        DbSet<T> Set<T>() where T : class;
        DbEntityEntry<T> Entry<T>(T entidade) where T : class;

        void InsertRange<T>(IEnumerable<T> entidades) where T : class;

        int SaveChanges();

        void DiscardChanges();

        void Dispose();
    }
}