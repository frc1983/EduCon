using System.Data.Entity.ModelConfiguration;
using EduCon.Objetos.Entidades;

namespace EduCon.Contexto.Mapeadores
{
    public class DataMap : EntityTypeConfiguration<Data>
    {
        public DataMap()
        {
            ToTable("EDC_DATA");

            // Chave primária da entidade
            HasKey(t => t.Id);

            // Mapemento coluna > propriedade
            Property(t => t.Id)
                .HasColumnName("ID")
                .IsRequired();

            Property(o => o.Ano)
                .HasColumnName("ANO")
                .IsRequired();

            // Navegação
            HasMany(o => o.Dados)
                .WithRequired(o => o.Data)
                .HasForeignKey(o => o.IdData)
                .WillCascadeOnDelete();
        }
    }
}