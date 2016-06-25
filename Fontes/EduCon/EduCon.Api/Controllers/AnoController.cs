using System.Collections.Generic;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    /// <summary>
    /// Interface de consulta de anos.
    /// </summary>
    [RoutePrefix("api/v1/anos")]
    public class AnoController : ApiController
    {
        private IDataAplServico _servico;

        public AnoController(IDataAplServico DataServico)
        {
            _servico = DataServico;
        }

        /// <summary>
        /// Consulta de ano por código identificador.
        /// </summary>
        /// <param name="id">Código identificador do ano</param>
        /// <returns>Informações do ano, caso encontrado</returns>
        [HttpGet]
        [Route("{id:int}")]
        public DataDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        /// <summary>
        /// Lista de todos os anos cadastrados.
        /// </summary>
        /// <returns>Coleção com todos os anos</returns>
        [HttpGet]
        [Route("")]
        public IEnumerable<DataDTO> Lista()
        {
            return _servico.ListaTodos();
        }
    }
}