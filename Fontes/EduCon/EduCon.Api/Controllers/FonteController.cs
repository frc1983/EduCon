using System.Collections.Generic;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    [RoutePrefix("api/v1/fontes")]
    public class FonteController : ApiController
    {
        private IFonteAplServico _servico;

        public FonteController(IFonteAplServico FonteServico)
        {
            _servico = FonteServico;
        }

        [HttpGet]
        [Route("{id:int}")]
        public FonteDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<FonteDTO> Lista()
        {
            return _servico.ListaTodos();
        }
    }
}