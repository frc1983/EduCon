using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using EduCon.Contexto.Mapeadores;
using EduCon.Objetos.Entidades;
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
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new MunicipioMap());
            modelBuilder.Configurations.Add(new DataMap());
            modelBuilder.Configurations.Add(new TipoEnsinoMap());
            modelBuilder.Configurations.Add(new CategoriaMapMap());
            modelBuilder.Configurations.Add(new DadoMap());
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

        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Data> Datas { get; set; }
        public DbSet<TipoEnsino> TiposEnsino { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Dado> Dados { get; set; }

        #endregion
    }
}