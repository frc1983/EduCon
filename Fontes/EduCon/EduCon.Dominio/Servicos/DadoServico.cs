using System.Collections.Generic;
using EduCon.Base.Dominio;
using EduCon.Dominio.Entidades;
using EduCon.Dominio.Interfaces.Repositorio;
using EduCon.Dominio.Interfaces.Servico;
using EduCon.Objetos.DTOs;

namespace EduCon.Dominio.Servicos
{
    public class DadoServico : Servico<Dado>, IDadoServico
    {
        public DadoServico(IDadoRepositorio repositorio) : base(repositorio)
        {
            
        }

        public IEnumerable<DadoOLAP> ListaOlap()
        {
            return (Repositorio as IDadoRepositorio).ListaOlap();
        }
    }
}