using EduCon.Base.Dominio.Interfaces;
using EduCon.Dominio.Entidades;

namespace EduCon.Dominio.Interfaces.Servico
{
    public interface IMunicipioServico : IServico<Municipio>
    {
        Municipio ConsultaPorNome(string municipio);
    }
}