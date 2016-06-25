using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    [RoutePrefix("api/v1/importacoes")]
    public class ImportacaoController : ApiController
    {
        private IProcessamentoAplServico _servico;

        public ImportacaoController(IProcessamentoAplServico processamentoServico)
        {
            _servico = processamentoServico;
        }

        [HttpOptions]
        [Route("nova")]
        public HttpResponseMessage Nova([FromBody]ProcessamentoDTO dto)
        {
            if (dto != null)
            {
                _servico.Inclui(dto);
            }

            return Request.CreateResponse(HttpStatusCode.Created, dto);
        }

        [HttpPut]
        [Route("reprocessar")]
        public HttpResponseMessage Reprocessar([FromBody]ProcessamentoDTO dto)
        {
            if (dto != null)
            {
                _servico.Reprocessar(dto);
            }

            return Request.CreateResponse(HttpStatusCode.OK, dto);
        }

        [HttpGet]
        [Route("{id:int}")]
        public ProcessamentoDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<ProcessamentoDTO> Lista()
        {
            return _servico.ListaTodos();
        }
    }
}
