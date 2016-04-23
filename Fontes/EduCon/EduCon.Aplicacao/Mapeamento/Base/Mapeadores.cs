using AutoMapper;

namespace EduCon.Aplicacao.Mapeamento.Base
{
    internal class Mapeadores
    {
        public static IMapper Mapper { get; private set; }

        public static void Registra()
        {
            Mapper = new MapperConfiguration((m) =>
            {
                m.AddProfile<MunicipioMapeador>();
            }).CreateMapper();
        }
    }
}