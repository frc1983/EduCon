using System.Collections.Generic;

namespace EduCon.Dominio.Entidades
{
    public class Data
    {
        public int Id { get; set; }
        public int Ano { get; set; }

        public ICollection<Dado> Dados { get; set; }
    }
}