namespace EduCon.ImportacaoServico.Objetos
{
    public class UnidadesGeograficas
    {
        public string Agrupador { get; set; }
        public string Nome { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Ibge { get; set; }
        public AnoValor[] Valores { get; set; }
    }
}