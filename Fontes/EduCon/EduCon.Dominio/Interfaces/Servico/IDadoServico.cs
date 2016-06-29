using System.Collections.Generic;
using EduCon.Base.Dominio.Interfaces;
using EduCon.Dominio.Entidades;
using EduCon.Objetos.DTOs;

namespace EduCon.Dominio.Interfaces.Servico
{
    public interface IDadoServico : IServico<Dado>
    {
        IEnumerable<DadoOLAP> ListaOlap();
    }
}