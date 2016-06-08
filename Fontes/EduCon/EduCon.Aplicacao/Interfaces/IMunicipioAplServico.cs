using EduCon.Base.Aplicacao.Interfaces;
using EduCon.Objetos.DTOs;

namespace EduCon.Aplicacao.Interfaces
{
    public interface IMunicipioAplServico : IAplServico<MunicipioDTO>
    {
        MunicipioDTO ConsultaPorNome(string municipio);
    }
}