using System.Collections.Generic;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    [RoutePrefix("api/data")]
    public class DataController : ApiController
    {
        private IDataAplServico _servico;

        public DataController(IDataAplServico DataServico)
        {
            _servico = DataServico;
        }

        [HttpGet]
        [Route("ConsultaPorId")]
        public DataDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        [HttpGet]
        [Route("Lista")]
        public IEnumerable<DataDTO> Lista()
        {
            return _servico.ListaTodos();
        }
    }
}