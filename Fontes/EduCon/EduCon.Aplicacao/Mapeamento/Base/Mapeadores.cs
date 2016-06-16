using AutoMapper;

namespace EduCon.Aplicacao.Mapeamento.Base
{
    internal class Mapeadores
    {
        public static IMapper Mapeador { get; private set; }

        public static void Registra()
        {
            Mapeador = new MapperConfiguration((m) =>
            {
                m.AddProfile<FonteMapeador>();
                m.AddProfile<MunicipioMapeador>();
                m.AddProfile<TipoEnsinoMapeador>();
                m.AddProfile<DataMapeador>();
                m.AddProfile<CategoriaMapeador>();
                m.AddProfile<DadoMapeador>();
            }).CreateMapper();
        }
    }
}