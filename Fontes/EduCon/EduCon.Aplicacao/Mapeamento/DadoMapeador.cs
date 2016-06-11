using AutoMapper;
using EduCon.Dominio.Entidades;
using EduCon.Objetos.DTOs;

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