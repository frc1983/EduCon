using AutoMapper;
using EduCon.Objetos.DTOs;
using EduCon.Objetos.Entidades;

namespace EduCon.Aplicacao.Mapeamento
{
    public class DadoMapeador : Profile
    {
        protected override void Configure()
        {
            // DTO > Entidade
            CreateMap<DadoDTO, Dado>();

            // Entidade > DTO
            CreateMap<Dado, DadoDTO>();
        }
    }
}