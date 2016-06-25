using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    /// <summary>
    /// Interface de consulta de municípios.
    /// </summary>
    [RoutePrefix("api/v1/municipios")]
    public class MunicipioController : ApiController
    {
        private IMunicipioAplServico _servico;

        public MunicipioController(IMunicipioAplServico municipioServico)
        {
            _servico = municipioServico;
        }

        #region Métodos padrão

        /// <summary>
        /// Consulta de município por código identificador.
        /// </summary>
        /// <param name="id">Código identificador do município</param>
        /// <returns>Informações do município, caso encontrado</returns>
        [HttpGet]
        [Route("{id:int}")]
        public MunicipioDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        /// <summary>
        /// Lista de todos os municípios cadastrados.
        /// </summary>
        /// <returns>Coleção com todos os municípios</returns>
        [HttpGet]
        [Route("")]
        public IEnumerable<MunicipioDTO> Lista()
        {
            return _servico.ListaTodos();
        }

        #endregion

        #region Métodos customizados

        /// <summary>
        /// Consulta de município por código IBGE.
        /// </summary>
        /// <param name="codIbge">Código identificador do IBGE</param>
        /// <returns>Informações de um município, caso encontrado</returns>
        [HttpGet]
        [Route("porCodIbge/{codIbge:int}")]
        public MunicipioDTO ConsultaPorCodIBGE(int codIbge)
        {
            return _servico.Lista(new MunicipioDTO { CodIBGE = codIbge }).FirstOrDefault();
        }

        /// <summary>
        /// Lista municípios por trecho de nome informado.
        /// </summary>
        /// <param name="nome">Nome ou trecho de nome do município</param>
        /// <returns>Coleção de municípios com o nome ou trecho informado, caso encontrado</returns>
        [HttpGet]
        [Route("porNome/{nome}")]
        public IEnumerable<MunicipioDTO> ListaPorNome(string nome)
        {
            return _servico.Lista(new MunicipioDTO { Nome = nome });
        }

        #endregion
    }
}