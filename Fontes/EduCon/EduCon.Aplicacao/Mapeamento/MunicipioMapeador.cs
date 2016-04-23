using AutoMapper;
using EduCon.Objetos.DTOs;
using EduCon.Objetos.Entidades;

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