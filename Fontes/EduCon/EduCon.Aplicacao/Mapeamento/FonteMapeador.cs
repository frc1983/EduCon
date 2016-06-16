using AutoMapper;
using EduCon.Dominio.Entidades;
using EduCon.Objetos.DTOs;

namespace EduCon.Aplicacao.Mapeamento
{
    public class FonteMapeador : Profile
    {
        protected override void Configure()
        {
            // DTO > Entidade
            CreateMap<FonteDTO, Fonte>();

            // Entidade > DTO
            CreateMap<Fonte, FonteDTO>();
        }
    }
}