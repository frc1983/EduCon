﻿using System.Collections.Generic;
using EduCon.Base.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Aplicacao.Interfaces
{
    public interface ICategoriaAplServico : IAplServico<CategoriaDTO>
    {
        IEnumerable<CategoriaDTO> ListaCategorias();
        IEnumerable<CategoriaDTO> ListaSubcategorias();
    }
}