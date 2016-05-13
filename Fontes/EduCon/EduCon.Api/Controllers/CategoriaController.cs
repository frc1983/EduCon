using System.Collections.Generic;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    [RoutePrefix("api/categoria")]
    public class CategoriaController : ApiController
    {
        private ICategoriaAplServico _servico;

        public CategoriaController(ICategoriaAplServico CategoriaServico)
        {
            _servico = CategoriaServico;
        }

        [HttpGet]
        [Route("ConsultaPorId")]
        public CategoriaDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        [HttpGet]
        [Route("Lista")]
        public IEnumerable<CategoriaDTO> Lista()
        {
            return _servico.ListaTodos();
        }
    }
}