using System.Collections.Generic;
using System.Data.Entity;
using EduCon.Base.Contexto.Interfaces;

namespace EduCon.Base.Contexto
{
    public class ContextoBase : DbContext, IContexto
    {
        public ContextoBase(string connectionName)
            : base(connectionName)
        {
        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public void InsertRange<T>(IEnumerable<T> entidades) where T : class
        {
            this.BulkInsert(entidades);
        }

        public void DiscardChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                // Se foi modificado, volta os valores iniciais
                if (entry.State == EntityState.Modified)
                {
                    entry.CurrentValues.SetValues(entry.OriginalValues);
                    entry.State = EntityState.Unchanged;
                    continue;
                }

                // Se foi excluído, volta para não modificado
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Unchanged;
                    continue;
                }

                // Se foi incluído, remove do contexto
                if (entry.State == EntityState.Added)
                {
                    entry.State = EntityState.Detached;
                    continue;
                }
            }
        }
    }
}