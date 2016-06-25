using System.Collections.Generic;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    /// <summary>
    /// Interface de consulta de tipos de ensino.
    /// </summary>
    [RoutePrefix("api/v1/tiposEnsino")]
    public class TipoEnsinoController : ApiController
    {
        private ITipoEnsinoAplServico _servico;

        public TipoEnsinoController(ITipoEnsinoAplServico TipoEnsinoServico)
        {
            _servico = TipoEnsinoServico;
        }

        /// <summary>
        /// Consulta de tipo de ensino por código identificador.
        /// </summary>
        /// <param name="id">Código identificador do tipo de ensino</param>
        /// <returns>Informações do tipo de ensino, caso encontrado</returns>
        [HttpGet]
        [Route("{id:int}")]
        public TipoEnsinoDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        /// <summary>
        /// Lista de todos os tipos de ensino cadastrados.
        /// </summary>
        /// <returns>Coleção com todos os tipos de ensino</returns>
        [HttpGet]
        [Route("")]
        public IEnumerable<TipoEnsinoDTO> Lista()
        {
            return _servico.ListaTodos();
        }
    }
}