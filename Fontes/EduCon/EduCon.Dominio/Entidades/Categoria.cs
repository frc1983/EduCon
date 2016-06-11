using System.Collections.Generic;

namespace EduCon.Dominio.Entidades
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public virtual ICollection<Dado> DadosCategoria { get; set; }
        public virtual ICollection<Dado> DadosSubcategoria { get; set; }
    }
}