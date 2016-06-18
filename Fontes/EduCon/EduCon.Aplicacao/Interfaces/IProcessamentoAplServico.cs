using System.Collections.Generic;
using EduCon.Base.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Aplicacao.Interfaces
{
    public interface IProcessamentoAplServico : IAplServico<ProcessamentoDTO>
    {
        void Processando(ProcessamentoDTO dto);
        void Processado(ProcessamentoDTO dto);
        void Reprocessar(ProcessamentoDTO dto);

        IEnumerable<ProcessamentoDTO> ListaProcessar(bool reprocessar = true);
    }
}