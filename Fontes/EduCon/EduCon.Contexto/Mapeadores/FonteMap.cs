using System.Data.Entity.ModelConfiguration;
using EduCon.Dominio.Entidades;

namespace EduCon.Contexto.Mapeadores
{
    public class FonteMap : EntityTypeConfiguration<Fonte>
    {
        public FonteMap()
        {
            ToTable("EDC_FONTE");

            // Chave primária da entidade
            HasKey(t => t.Id);

            // Mapemento coluna > propriedade
            Property(t => t.Id)
                .HasColumnName("ID")
                .IsRequired();

            Property(o => o.Nome)
                .HasColumnName("NOME")
                .IsRequired()
                .HasMaxLength(200);

            // Navegação
            HasMany(o => o.Dados)
                .WithRequired(o => o.Fonte)
                .HasForeignKey(o => o.IdFonte)
                .WillCascadeOnDelete();
        }
    }
}