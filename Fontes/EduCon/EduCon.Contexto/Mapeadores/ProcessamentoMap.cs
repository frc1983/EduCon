using System.Data.Entity.ModelConfiguration;
using EduCon.Dominio.Entidades;

namespace EduCon.Contexto.Mapeadores
{
    public class ProcessamentoMap : EntityTypeConfiguration<Processamento>
    {
        public ProcessamentoMap()
        {
            ToTable("EDC_PROCESSAMENTO");

            // Chave primária da entidade
            HasKey(t => t.Id);

            // Mapemento coluna > propriedade
            Property(t => t.Id)
                .HasColumnName("ID")
                .IsRequired();

            Property(o => o.Texto)
                .HasColumnName("TEXTO")
                .HasMaxLength(100);

            Property(o => o.AnoInicial)
                .HasColumnName("ANO_INI")
                .IsRequired();

            Property(o => o.AnoFinal)
                .HasColumnName("ANO_FIM")
                .IsRequired();

            Property(o => o.Situacao)
                .HasColumnName("SITUACAO")
                .IsRequired();

            Property(o => o.Data)
                .HasColumnName("DATA");

            Property(o => o.QtdRegistros)
                .HasColumnName("QTD_REGISTROS");
        }
    }
}