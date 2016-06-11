﻿using AutoMapper;
using EduCon.Dominio.Entidades;
using EduCon.Objetos.DTOs;

namespace EduCon.Aplicacao.Mapeamento
{
    public class MunicipioMapeador : Profile
    {
        protected override void Configure()
        {
            // DTO > Entidade
            CreateMap<MunicipioDTO, Municipio>();

            // Entidade > DTO
            CreateMap<Municipio, MunicipioDTO>();
        }
    }
}