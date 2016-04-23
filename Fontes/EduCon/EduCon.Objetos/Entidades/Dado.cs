namespace EduCon.Objetos.Entidades
{
    public class Dado
    {
        public int Id { get; set; }
        public int IdMunicipio { get; set; }
        public int IdTipoEnsino { get; set; }
        public int IdCategoria { get; set; }
        public int? IdSubcategoria { get; set; }
        public int IdData { get; set; }
        public string Valor { get; set; }

        public Municipio Municipio { get; set; }
        public TipoEnsino TipoEnsino { get; set; }
        public Categoria Categoria { get; set; }
        public Categoria Subcategoria { get; set; }
        public Data Data { get; set; }
    }
}