using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    [RoutePrefix("api/municipio")]
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class MunicipioController : ApiController
    {
        private IMunicipioAplServico _servico;

        public MunicipioController(IMunicipioAplServico municipioServico)
        {
            _servico = municipioServico;
        }

        [HttpGet]
        [Route("ConsultaPorId")]
        public MunicipioDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        [HttpGet]
        [Route("ConsultaPorCodIbge")]
        public MunicipioDTO ConsultaPorCodIBGE(int codIbge)
        {
            return _servico.Lista(new MunicipioDTO { CodIBGE = codIbge }).FirstOrDefault();
        }

        [HttpGet]
        [Route("Lista")]
        public IEnumerable<MunicipioDTO> Lista()
        {
            return _servico.ListaTodos();
        }

        [HttpGet]
        [Route("ListaPorNome")]
        public IEnumerable<MunicipioDTO> ListaPorNome(string nome)
        {
            return _servico.Lista(new MunicipioDTO { Nome = nome });
        }
    }
}
