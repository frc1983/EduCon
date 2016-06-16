namespace EduCon.Objetos.DTOs
{
    public class DadoDTO
    {
        public int Id { get; set; }
        public int IdFonte { get; set; }
        public int IdMunicipio { get; set; }
        public int IdTipoEnsino { get; set; }
        public int IdCategoria { get; set; }
        public int? IdSubcategoria { get; set; }
        public int IdData { get; set; }
        public string Valor { get; set; }

        public FonteDTO Fonte { get; set; }
        public MunicipioDTO Municipio { get; set; }
        public TipoEnsinoDTO TipoEnsino { get; set; }
        public CategoriaDTO Categoria { get; set; }
        public CategoriaDTO Subcategoria { get; set; }
        public DataDTO Data { get; set; }
    }
}