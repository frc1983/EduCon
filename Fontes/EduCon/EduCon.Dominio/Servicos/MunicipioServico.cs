using EduCon.Base.Dominio;
using EduCon.Base.Dominio.Interfaces;
using EduCon.Dominio.Entidades;
using EduCon.Dominio.Interfaces.Servico;

namespace EduCon.Dominio.Servicos
{
    public class MunicipioServico : Servico<Municipio>, IMunicipioServico
    {
        public MunicipioServico(IRepositorio<Municipio> repositorio) : base(repositorio)
        {
        }

        public Municipio ConsultaPorNome(string municipio)
        {
            return Repositorio.Consulta(x => x.Nome.Equals(municipio.Trim()));
        }
    }
}