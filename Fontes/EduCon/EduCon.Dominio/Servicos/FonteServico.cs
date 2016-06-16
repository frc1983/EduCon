using EduCon.Base.Dominio;
using EduCon.Base.Dominio.Interfaces;
using EduCon.Dominio.Entidades;
using EduCon.Dominio.Interfaces.Servico;

namespace EduCon.Dominio.Servicos
{
    public class FonteServico : Servico<Fonte>, IFonteServico
    {
        public FonteServico(IRepositorio<Fonte> repositorio) : base(repositorio)
        {
        }
    }
}