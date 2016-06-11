using AutoMapper;
using EduCon.Dominio.Entidades;
using EduCon.Objetos.DTOs;

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