using AutoMapper;
using EduCon.Dominio.Entidades;
using EduCon.Dominio.Entidades.Enums;
using EduCon.Objetos.DTOs;
using EduCon.Utilitarios.Aplicacao;

namespace EduCon.Aplicacao.Mapeamento
{
    public class ProcessamentoMapeador : Profile
    {
        protected override void Configure()
        {
            // DTO > Entidade
            CreateMap<ProcessamentoDTO, Processamento>()
                .ForMember(d => d.Situacao, o => o.MapFrom(s => (SituacaoProcessamento)s.CodSituacao));

            // Entidade > DTO
            CreateMap<Processamento, ProcessamentoDTO>()
                .ForMember(d => d.CodSituacao, o => o.MapFrom(s => (int)s.Situacao))
                .AfterMap((src, dest) => dest.Situacao = DescricaoEnum.ObtemDescricao(src.Situacao));
        }
    }
}