using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    [Route("api/municipios/")]
    public class MunicipioController : ApiController
    {
        private IMunicipioAplServico _servico;

        public MunicipioController(IMunicipioAplServico municipioServico)
        {
            _servico = municipioServico;
        }

        [HttpGet]
        [Route("porId")]
        public MunicipioDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        [HttpGet]
        [Route("porCodIbge")]
        public MunicipioDTO ConsultaPorCodIBGE(int codIbge)
        {
            return _servico.Lista(new MunicipioDTO { CodIBGE = codIbge }).FirstOrDefault();
        }

        [HttpGet]
        [Route("todos")]
        public IEnumerable<MunicipioDTO> Lista()
        {
            return _servico.ListaTodos();
        }

        [HttpGet]
        [Route("porNome")]
        public IEnumerable<MunicipioDTO> ListaPorNome(string nome)
        {
            return _servico.Lista(new MunicipioDTO { Nome = nome });
        }
    }
}