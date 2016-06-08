﻿using EduCon.Base.Dominio.Interfaces;
using EduCon.Objetos.Entidades;

namespace EduCon.Dominio.Interfaces.Servico
{
    public interface IMunicipioServico : IServico<Municipio>
    {
        Municipio ConsultaPorNome(string municipio);
    }
}