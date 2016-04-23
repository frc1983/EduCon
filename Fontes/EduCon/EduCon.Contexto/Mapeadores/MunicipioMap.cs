﻿using System.Data.Entity.ModelConfiguration;
using EduCon.Objetos.Entidades;

namespace EduCon.Contexto.Mapeadores
{
    public class MunicipioMap : EntityTypeConfiguration<Municipio>
    {
        public MunicipioMap()
        {
            ToTable("EDC_MUNICIPIO");

            // Chave primária da entidade
            HasKey(t => t.Id);

            // Mapemento coluna > propriedade
            Property(t => t.Id)
                .HasColumnName("ID")
                .IsRequired();

            Property(o => o.CodIBGE)
                .HasColumnName("COD_IBGE");

            Property(o => o.Nome)
                .HasColumnName("NOME")
                .IsRequired()
                .HasMaxLength(200);

            Property(o => o.Agrupador)
                .HasColumnName("AGRUPADOR");

            Property(o => o.Latitude)
                .HasColumnName("LATITUDE");

            Property(o => o.Longitude)
                .HasColumnName("LONGITUDE");
        }
    }
}