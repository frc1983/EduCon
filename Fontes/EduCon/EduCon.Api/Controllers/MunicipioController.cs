using System.Collections.Generic;
using System.Web.Http;
using EduCon.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Api.Controllers
{
    public class MunicipioController : ApiController
    {
        private IMunicipioAplServico _servico;

        public MunicipioController(IMunicipioAplServico municipioServico)
        {
            _servico = municipioServico;
        }

        [HttpPost]
        public void Inclui([FromBody]MunicipioDTO dto)
        {
            _servico.Inclui(dto);
        }

        [HttpPut]
        public void Altera(int id, [FromBody]MunicipioDTO dto)
        {
            _servico.Inclui(dto);
        }

        [HttpDelete]
        public void Exclui(int id)
        {
            var dto = new MunicipioDTO { Id = id };
            _servico.Exclui(dto);
        }

        [HttpGet]
        public MunicipioDTO Consulta(int id)
        {
            return _servico.Consulta(id);
        }

        [HttpGet]
        public IEnumerable<MunicipioDTO> Lista()
        {
            return _servico.ListaTodos();
        }
    }
}