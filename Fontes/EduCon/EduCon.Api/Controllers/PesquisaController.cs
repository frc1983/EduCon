using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    [RoutePrefix("api/v1/pesquisa")]
    public class PesquisaController : ApiController
    {
        private IFonteAplServico _servicoFonte;
        private IMunicipioAplServico _servicoMunicipio;
        private ITipoEnsinoAplServico _servicoTipoEnsino;
        private ICategoriaAplServico _servicoCategoria;

        public PesquisaController(IFonteAplServico servicoFonte, IMunicipioAplServico servicoMunicipio, ITipoEnsinoAplServico servicoTipoEnsino, ICategoriaAplServico servicoCategoria)
        {
            _servicoFonte = servicoFonte;
            _servicoMunicipio = servicoMunicipio;
            _servicoTipoEnsino = servicoTipoEnsino;
            _servicoCategoria = servicoCategoria;
        }

        [HttpGet]
        [Route("")]
        public object Pesquisa(string texto)
        {
            var fontes = _servicoFonte.Lista(new FonteDTO() { Nome = texto });
            var municipios = _servicoMunicipio.Lista(new MunicipioDTO() { Nome = texto });
            var tiposEnsino = _servicoTipoEnsino.Lista(new TipoEnsinoDTO() { Nome = texto });
            var categorias = _servicoCategoria.Lista(new CategoriaDTO() { Nome = texto });

            return new
            {
                Fontes = fontes,
                Municipios = municipios,
                TiposEnsino = tiposEnsino,
                Categorias = categorias
            };
        }
    }
}