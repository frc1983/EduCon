using System;

namespace EduCon.Objetos.DTOs
{
    public class ProcessamentoDTO
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public int AnoInicial { get; set; }
        public int AnoFinal { get; set; }
        public DateTime? Data { get; set; }

        public int CodSituacao { get; set; }
        public string Situacao { get; set; }

        public int QtdRegistros { get; set; }
    }
}