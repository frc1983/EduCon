using System.Collections.Generic;
using EduCon.Base.Dominio.Interfaces;
using EduCon.Dominio.Entidades;
using EduCon.Objetos.DTOs;

namespace EduCon.Dominio.Interfaces.Repositorio
{
    public interface IDadoRepositorio : IRepositorio<Dado>
    {
        IEnumerable<DadoOLAP> ListaOlap();
    }
}