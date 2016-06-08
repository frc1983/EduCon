﻿using System.Collections.Generic;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    [RoutePrefix("api/v1/dados")]
    public class DadoController : ApiController
    {
        private IDadoAplServico _servico;

        public DadoController(IDadoAplServico DadoServico)
        {
            _servico = DadoServico;
        }

        [HttpGet]
        [Route("{id:int}")]
        public DadoDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<DadoDTO> Lista([FromUri] DadoDTO dto)
        {
            return _servico.Lista(dto);
        }
    }
}