using AutoMapper;
using EduCon.Dominio.Entidades;
using EduCon.Objetos.DTOs;

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