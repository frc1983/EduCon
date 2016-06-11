using System.Collections.Generic;

namespace EduCon.Dominio.Entidades
{
    public class TipoEnsino
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public ICollection<Dado> Dados { get; set; }
    }
}