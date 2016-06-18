using System;
using EduCon.Dominio.Entidades.Enums;

namespace EduCon.Dominio.Entidades
{
    public class Processamento
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public int AnoInicial { get; set; }
        public int AnoFinal { get; set; }
        public DateTime? Data { get; set; }
        public SituacaoProcessamento Situacao { get; set; }
        public int QtdRegistros { get; set; }
    }
}