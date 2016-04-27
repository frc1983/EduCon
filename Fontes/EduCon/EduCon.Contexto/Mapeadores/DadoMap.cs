using System.Data.Entity.ModelConfiguration;
using EduCon.Objetos.Entidades;

namespace EduCon.Contexto.Mapeadores
{
    public class DadoMap : EntityTypeConfiguration<Dado>
    {
        public DadoMap()
        {
            ToTable("EDC_DADO");

            // Chave primária da entidade
            HasKey(t => t.Id);

            // Mapemento coluna > propriedade
            Property(t => t.Id)
                .HasColumnName("ID")
                .IsRequired();

            Property(t => t.IdMunicipio)
                .HasColumnName("ID_MUNICIPIO")
                .IsRequired();

            Property(t => t.IdTipoEnsino)
                .HasColumnName("ID_TIPO_ENSINO")
                .IsRequired();

            Property(t => t.IdCategoria)
                .HasColumnName("ID_CATEGORIA")
                .IsRequired();

            Property(t => t.IdSubcategoria)
                .HasColumnName("ID_SUBCATEGORIA");

            Property(t => t.IdData)
                .HasColumnName("ID_DATA")
                .IsRequired();

            Property(o => o.Valor)
                .HasColumnName("VALOR")
                .IsRequired()
                .HasMaxLength(200);

            // Navegação
            HasRequired(b => b.Categoria)
                .WithMany(a => a.Dados)
                .HasForeignKey(b => b.IdCategoria);

            HasOptional(b => b.Subcategoria)
                .WithMany(a => a.Dados)
                .HasForeignKey(b => b.IdSubcategoria);
        }
    }
}