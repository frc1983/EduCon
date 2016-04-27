using AutoMapper;
using EduCon.Objetos.DTOs;
using EduCon.Objetos.Entidades;

namespace EduCon.Aplicacao.Mapeamento
{
    public class TipoEnsinoMapeador : Profile
    {
        protected override void Configure()
        {
            // DTO > Entidade
            CreateMap<TipoEnsinoDTO, TipoEnsino>();

            // Entidade > DTO
            CreateMap<TipoEnsino, TipoEnsinoDTO>();
        }
    }
}