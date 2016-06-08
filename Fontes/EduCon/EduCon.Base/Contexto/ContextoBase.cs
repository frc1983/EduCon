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

        public void DiscardChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.CurrentValues.SetValues(entry.OriginalValues);
                    entry.State = EntityState.Unchanged;
                    continue;
                }

                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Unchanged;
                    continue;
                }

                if (entry.State == EntityState.Added)
                {
                    entry.State = EntityState.Detached;
                    continue;
                }
            }
        }
    }
}