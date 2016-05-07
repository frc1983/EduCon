using EduCon.Objetos.DTOs;
using EduCon.Utilitarios.Aplicacao.Interfaces;

namespace EduCon.Aplicacao.Interfaces
{
    public interface IMunicipioAplServico : IAplServico<MunicipioDTO>
    {
        MunicipioDTO ConsultaPorNome(string municipio);
    }
}