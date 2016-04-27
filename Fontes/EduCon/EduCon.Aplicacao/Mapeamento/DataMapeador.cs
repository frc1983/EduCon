using AutoMapper;
using EduCon.Objetos.DTOs;
using EduCon.Objetos.Entidades;

namespace EduCon.Aplicacao.Mapeamento
{
    public class DataMapeador : Profile
    {
        protected override void Configure()
        {
            // DTO > Entidade
            CreateMap<DataDTO, Data>();

            // Entidade > DTO
            CreateMap<Data, DataDTO>();
        }
    }
}