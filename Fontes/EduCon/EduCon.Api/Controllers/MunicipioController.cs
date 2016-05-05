using System.Collections.Generic;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    [RoutePrefix("api/municipio")]
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

        [Route("ConsultaPorNome")]
        [HttpGet]
        public IEnumerable<MunicipioDTO> ConsultaPorNome(string municipio)
        {
            return _servico.Lista(new MunicipioDTO { Nome = municipio });
        }
    }
}