using System.Collections.Generic;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    public class TipoEnsinoController : ApiController
    {
        private ITipoEnsinoAplServico _servico;

        public TipoEnsinoController(ITipoEnsinoAplServico TipoEnsinoServico)
        {
            _servico = TipoEnsinoServico;
        }

        [HttpGet]
        public TipoEnsinoDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        [HttpGet]
        public IEnumerable<TipoEnsinoDTO> Lista()
        {
            return _servico.ListaTodos();
        }
    }
}