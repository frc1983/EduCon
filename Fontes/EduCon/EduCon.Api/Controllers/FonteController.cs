using System.Collections.Generic;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    /// <summary>
    /// Interface de consulta de fontes.
    /// </summary>
    [RoutePrefix("api/v1/fontes")]
    public class FonteController : ApiController
    {
        private IFonteAplServico _servico;

        public FonteController(IFonteAplServico FonteServico)
        {
            _servico = FonteServico;
        }

        /// <summary>
        /// Consulta de fonte por código identificador.
        /// </summary>
        /// <param name="id">Código identificador da fonte</param>
        /// <returns>Informações da fonte, caso encontrado</returns>
        [HttpGet]
        [Route("{id:int}")]
        public FonteDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        /// <summary>
        /// Lista de todas as fontes cadastradas.
        /// </summary>
        /// <returns>Coleção com todas as fontes</returns>
        [HttpGet]
        [Route("")]
        public IEnumerable<FonteDTO> Lista()
        {
            return _servico.ListaTodos();
        }
    }
}