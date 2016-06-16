using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using EduCon.Base.Contexto;
using EduCon.Contexto.Mapeadores;
using EduCon.Dominio.Entidades;

namespace EduCon.Contexto
{
    public class ContextoEF
        : ContextoBase
    {
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

            modelBuilder.Configurations.Add(new FonteMap());
            modelBuilder.Configurations.Add(new MunicipioMap());
            modelBuilder.Configurations.Add(new DataMap());
            modelBuilder.Configurations.Add(new TipoEnsinoMap());
            modelBuilder.Configurations.Add(new CategoriaMapMap());
            modelBuilder.Configurations.Add(new DadoMap());
        }

        /// <summary>
        /// Método que intercepta as queries executadas pelo EF e as imprime no console de debug.
        /// </summary>
        /// <param name="query"></param>
        protected void GravaLog(string query)
        {
            Debug.WriteLine(query);
        }

        #region DbSets

        public DbSet<Fonte> Fontes { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Data> Datas { get; set; }
        public DbSet<TipoEnsino> TiposEnsino { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Dado> Dados { get; set; }

        #endregion
    }
}