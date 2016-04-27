﻿using System.Collections.Generic;

namespace EduCon.Objetos.Entidades
{
    public class TipoEnsino
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public ICollection<Dado> Dados { get; set; }
    }
}