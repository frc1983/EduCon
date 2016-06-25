using System.Collections.Generic;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    /// <summary>
    /// Interface de consulta de categorias.
    /// </summary>
    [RoutePrefix("api/v1/categorias")]
    public class CategoriaController : ApiController
    {
        private ICategoriaAplServico _servico;

        public CategoriaController(ICategoriaAplServico CategoriaServico)
        {
            _servico = CategoriaServico;
        }

        /// <summary>
        /// Consulta de categoria por código identificador.
        /// </summary>
        /// <param name="id">Código identificador da categoria</param>
        /// <returns>Informações da categoria, caso encontrado</returns>
        [HttpGet]
        [Route("{id:int}")]
        public CategoriaDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        /// <summary>
        /// Lista de todas as categorias cadastradas.
        /// </summary>
        /// <returns>Coleção com todas as categorias</returns>
        [HttpGet]
        [Route("")]
        public IEnumerable<CategoriaDTO> Lista()
        {
            return _servico.ListaTodos();
        }

        /// <summary>
        /// Lista de categorias classificadas como categorias do dado.
        /// </summary>
        /// <returns>Coleção com categorias</returns>
        [HttpGet]
        [Route("categorias")]
        public IEnumerable<CategoriaDTO> ListaCategorias()
        {
            return _servico.ListaCategorias();
        }

        /// <summary>
        /// Lista de categorias classificadas como subcategorias do dado.
        /// </summary>
        /// <returns>Coleção com subcategorias</returns>
        [HttpGet]
        [Route("subcategorias")]
        public IEnumerable<CategoriaDTO> ListaSubcategorias()
        {
            return _servico.ListaSubcategorias();
        }
    }
}