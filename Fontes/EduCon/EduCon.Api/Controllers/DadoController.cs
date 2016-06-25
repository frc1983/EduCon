using System.Collections.Generic;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

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
            return _servico.Lista(dto);
        }
    }
}