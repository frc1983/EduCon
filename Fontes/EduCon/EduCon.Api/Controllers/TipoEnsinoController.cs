using System.Collections.Generic;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    [RoutePrefix("api/tipoensino")]
    public class TipoEnsinoController : ApiController
    {
        private ITipoEnsinoAplServico _servico;

        public TipoEnsinoController(ITipoEnsinoAplServico TipoEnsinoServico)
        {
            _servico = TipoEnsinoServico;
        }

        [HttpGet]
        [Route("ConsultaPorId")]
        public TipoEnsinoDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        [HttpGet]
        [Route("Lista")]
        public IEnumerable<TipoEnsinoDTO> Lista()
        {
            return _servico.ListaTodos();
        }
    }
}