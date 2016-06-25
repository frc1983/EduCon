using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    /// <summary>
    /// Interface de consulta e inclusão de solicitações de importação.
    /// </summary>
    [RoutePrefix("api/v1/importacoes")]
    public class ImportacaoController : ApiController
    {
        private IProcessamentoAplServico _servico;

        public ImportacaoController(IProcessamentoAplServico processamentoServico)
        {
            _servico = processamentoServico;
        }

        /// <summary>
        /// Insere nova solicitação de importação de dados.
        /// </summary>
        /// <param name="ano">Ano base para realizar a importação</param>
        /// <returns>Objeto com código identificador da solicitação de importação incluída</returns>
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

        /// <summary>
        /// Altera uma solicitação de importação existente para que esta seja reprocessada.
        /// </summary>
        /// <param name="id">Código identificador da importação</param>
        /// <returns>Objeto atualizado da solicitação de importação</returns>
        [HttpPut, HttpOptions]
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

        /// <summary>
        /// Consulta de importação por código identificador.
        /// </summary>
        /// <param name="id">Código identificador da importação</param>
        /// <returns>Informações da importação, caso encontrado</returns>
        [HttpGet]
        [Route("{id:int}")]
        public ProcessamentoDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        /// <summary>
        /// Lista de todas as importações cadastradas.
        /// </summary>
        /// <returns>Coleção com todos as importações</returns>
        [HttpGet]
        [Route("")]
        public IEnumerable<ProcessamentoDTO> Lista()
        {
            return _servico.ListaTodos();
        }
    }
}