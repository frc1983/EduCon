namespace EduCon.Objetos.Entidades
{
    public class Municipio
    {
        public int Id { get; set; }
        public int CodIBGE { get; set; }
        public int Agrupador { get; set; }
        public string Nome { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}