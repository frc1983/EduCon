using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    [RoutePrefix("api/v1/municipios")]
    public class MunicipioController : ApiController
    {
        private IMunicipioAplServico _servico;

        public MunicipioController(IMunicipioAplServico municipioServico)
        {
            _servico = municipioServico;
        }

        #region Métodos padrão

        [HttpGet]
        [Route("{id:int}")]
        public MunicipioDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<MunicipioDTO> Lista()
        {
            return _servico.ListaTodos();
        }

        #endregion

        #region Métodos customizados

        [HttpGet]
        [Route("porCodIbge/{codIbge:int}")]
        public MunicipioDTO ConsultaPorCodIBGE(int codIbge)
        {
            return _servico.Lista(new MunicipioDTO { CodIBGE = codIbge }).FirstOrDefault();
        }

        [HttpGet]
        [Route("porNome/{nome}")]
        public IEnumerable<MunicipioDTO> ListaPorNome(string nome)
        {
            return _servico.Lista(new MunicipioDTO { Nome = nome });
        }

        #endregion
    }
}