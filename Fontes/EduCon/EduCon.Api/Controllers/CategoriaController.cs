using System.Collections.Generic;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    public class CategoriaController : ApiController
    {
        private ICategoriaAplServico _servico;

        public CategoriaController(ICategoriaAplServico CategoriaServico)
        {
            _servico = CategoriaServico;
        }

        [HttpGet]
        public CategoriaDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        [HttpGet]
        public IEnumerable<CategoriaDTO> Lista()
        {
            return _servico.ListaTodos();
        }
    }
}