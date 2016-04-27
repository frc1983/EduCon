using AutoMapper;
using EduCon.Objetos.DTOs;
using EduCon.Objetos.Entidades;

namespace EduCon.Aplicacao.Mapeamento
{
    public class CategoriaMapeador : Profile
    {
        protected override void Configure()
        {
            // DTO > Entidade
            CreateMap<CategoriaDTO, Categoria>();

            // Entidade > DTO
            CreateMap<Categoria, CategoriaDTO>();
        }
    }
}