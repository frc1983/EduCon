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

        [HttpPost]
        [Route("nova/{ano:int}")]
        public HttpResponseMessage Nova(int ano)
        {
            var dto = new ProcessamentoDTO()
            {
                Texto = "\\Educação",
                AnoInicial = ano,
                AnoFinal = ano
            };

            _servico.Inclui(dto);

            return Request.CreateResponse(HttpStatusCode.Created, dto);
        }

        [HttpPut]
        [HttpOptions]
        [Route("reprocessar/{id:int}")]
        public HttpResponseMessage Reprocessar(int id)
        {
            var dto = new ProcessamentoDTO()
            {
                Id = id
            };

            _servico.Reprocessar(dto);

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
