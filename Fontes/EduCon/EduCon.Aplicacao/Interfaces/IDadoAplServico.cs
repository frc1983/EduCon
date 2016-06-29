using System.Collections.Generic;
using EduCon.Base.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Aplicacao.Interfaces
{
    public interface IDadoAplServico : IAplServico<DadoDTO>
    {
        IEnumerable<DadoOLAP> ListaOlap();
    }
}