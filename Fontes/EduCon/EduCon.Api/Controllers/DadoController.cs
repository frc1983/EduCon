using System.Collections.Generic;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    [RoutePrefix("api/dado")]
    public class DadoController : ApiController
    {
        private IDadoAplServico _servico;

        public DadoController(IDadoAplServico DadoServico)
        {
            _servico = DadoServico;
        }

        [HttpGet]
        [Route("ConsultaPorId")]
        public DadoDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        [HttpGet]
        [Route("Lista")]
        public IEnumerable<DadoDTO> Lista()
        {
            return _servico.ListaTodos();
        }
    }
}