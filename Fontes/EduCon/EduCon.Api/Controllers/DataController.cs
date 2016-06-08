using System.Collections.Generic;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    [RoutePrefix("api/v1/datas")]
    public class DataController : ApiController
    {
        private IDataAplServico _servico;

        public DataController(IDataAplServico DataServico)
        {
            _servico = DataServico;
        }

        [HttpGet]
        [Route("{id:int}")]
        public DataDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<DataDTO> Lista()
        {
            return _servico.ListaTodos();
        }
    }
}