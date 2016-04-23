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
                .HasColumnName("MUNICIPIO_ID")
                .IsRequired();

            Property(t => t.IdTipoEnsino)
                .HasColumnName("TIPO_ENSINO_ID")
                .IsRequired();

            Property(t => t.IdCategoria)
                .HasColumnName("CATEGORIA_ID")
                .IsRequired();

            Property(t => t.IdSubcategoria)
                .HasColumnName("SUBCATEGORIA_ID");

            Property(t => t.IdData)
                .HasColumnName("DATA_ID")
                .IsRequired();

            Property(o => o.Valor)
                .HasColumnName("VALOR")
                .IsRequired()
                .HasMaxLength(200);

            HasRequired(o => o.Municipio);
            HasRequired(o => o.TipoEnsino);
            HasRequired(o => o.Categoria);
            HasOptional(o => o.Subcategoria);
            HasRequired(o => o.Data);
        }
    }
}