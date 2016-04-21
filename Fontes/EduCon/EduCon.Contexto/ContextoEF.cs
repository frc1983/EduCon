using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using EduCon.Utilitarios.Contexto;

namespace EduCon.Contexto
{
    public class ContextoEF
        : ContextoBase
    {
        static ContextoEF()
        {
            Database.SetInitializer<ContextoEF>(null);
        }

        public ContextoEF()
            : base("Name=EduConBD")
        {
            Database.Log = GravaLog;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Configurations.Add(new TMap());
        }

        /// <summary>
        /// Loga as queries do Entity
        /// </summary>
        /// <param name="query"></param>
        protected void GravaLog(string query)
        {
            Debug.WriteLine(query);
        }

        #region DbSets

        //public DbSet<T> Ts { get; set; }
        
        #endregion
    }
}