using AutoMapper;
using EduCon.Dominio.Entidades;
using EduCon.Objetos.DTOs;

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