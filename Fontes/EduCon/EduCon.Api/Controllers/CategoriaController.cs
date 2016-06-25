using System.Collections.Generic;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    [RoutePrefix("api/v1/categorias")]
    public class CategoriaController : ApiController
    {
        private ICategoriaAplServico _servico;

        public CategoriaController(ICategoriaAplServico CategoriaServico)
        {
            _servico = CategoriaServico;
        }

        [HttpGet]
        [Route("{id:int}")]
        public CategoriaDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<CategoriaDTO> Lista()
        {
            return _servico.ListaTodos();
        }

        [HttpGet]
        [Route("categorias")]
        public IEnumerable<CategoriaDTO> ListaCategorias()
        {
            return _servico.ListaCategorias();
        }

        [HttpGet]
        [Route("subcategorias")]
        public IEnumerable<CategoriaDTO> ListaSubcategorias()
        {
            return _servico.ListaSubcategorias();
        }
    }
}