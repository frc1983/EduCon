using System.Collections.Generic;

namespace EduCon.Objetos.Entidades
{
    public class Data
    {
        public int Id { get; set; }
        public int Ano { get; set; }

        public ICollection<Dado> Dados { get; set; }
    }
}