using System.Data.Entity.ModelConfiguration;
using EduCon.Objetos.Entidades;

namespace EduCon.Contexto.Mapeadores
{
    public class TipoEnsinoMap : EntityTypeConfiguration<TipoEnsino>
    {
        public TipoEnsinoMap()
        {
            ToTable("EDC_TIPO_ENSINO");

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
        }
    }
}