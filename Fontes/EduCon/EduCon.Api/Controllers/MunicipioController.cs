﻿using System.Collections.Generic;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    public class MunicipioController : ApiController
    {
        private IMunicipioAplServico _servico;

        public MunicipioController(IMunicipioAplServico municipioServico)
        {
            _servico = municipioServico;
        }

        [HttpGet]
        public MunicipioDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        [HttpGet]
        public IEnumerable<MunicipioDTO> Lista()
        {
            return _servico.ListaTodos();
        }
    }
}