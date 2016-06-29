using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;
using Newtonsoft.Json;

namespace EduCon.Api.Controllers
{
    /// <summary>
    /// Interface de consulta de dados.
    /// </summary>
    [RoutePrefix("api/v1/dados")]
    public class DadoController : ApiController
    {
        private IDadoAplServico _servico;

        public DadoController(IDadoAplServico DadoServico)
        {
            _servico = DadoServico;
        }

        /// <summary>
        /// Consulta de dado por código identificador.
        /// </summary>
        /// <param name="id">Código identificador do dado</param>
        /// <returns>Informações do dado, caso encontrado</returns>
        [HttpGet]
        [Route("{id:int}")]
        public DadoDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        /// <summary>
        /// Lista de todos os dados cadastrados, conforme o filtro informado.
        /// </summary>
        /// <param name="dto">Objeto de filtro</param>
        /// <returns>Coleção com todos os dados a partir do filtro selecionado</returns>
        [HttpGet]
        [Route("")]
        public IEnumerable<DadoDTO> Lista([FromUri] DadoDTO dto)
        {
            if (dto != null)
            {
                return _servico.Lista(dto);
            }

            return _servico.ListaTodos();
        }

        /// <summary>
        /// Lista para exibição OLAP.
        /// </summary>
        /// <returns>Lista de dados para exibição OLAP</returns>
        [HttpGet]
        [Route("olap")]
        public IEnumerable<DadoOLAP> ListaOlap()
        {
            return _servico.ListaOlap();
        }

        /// <summary>
        /// Realizar o download do arquivo json para exibição OLAP.
        /// </summary>
        /// <returns>Arquivo json com todo os dados para o OLAP</returns>
        [HttpGet]
        [Route("olap/download")]
        public HttpResponseMessage DownloadOlap()
        {
            var content = new StringContent(JsonConvert.SerializeObject(_servico.ListaOlap()));

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Headers.AcceptRanges.Add("bytes");
            response.Content = content;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "dadosOlap.json" };
            return response;
        }
    }
}