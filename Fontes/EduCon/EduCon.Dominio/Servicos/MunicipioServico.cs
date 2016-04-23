using EduCon.Dominio.Interfaces.Servico;
using EduCon.Objetos.Entidades;
using EduCon.Utilitarios.Dominio;
using EduCon.Utilitarios.Dominio.Interfaces;

namespace EduCon.Dominio.Servicos
{
    public class MunicipioServico : Servico<Municipio>, IMunicipioServico
    {
        public MunicipioServico(IRepositorio<Municipio> repositorio) : base(repositorio)
        {
        }
    }
}