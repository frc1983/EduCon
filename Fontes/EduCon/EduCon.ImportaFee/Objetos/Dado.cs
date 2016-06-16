using EduCon.Objetos.DTOs;

namespace EduCon.ImportaFee.Objetos
{
    public class Dado
    {
        public FonteDTO Fonte { get; set; }
        public MunicipioDTO Municipio { get; set; }
        public TipoEnsinoDTO TipoEnsino { get; set; }
        public CategoriaDTO Categoria { get; set; }
        public CategoriaDTO Subcategoria { get; set; }
        public DataDTO Data { get; set; }

        public string Valor { get; set; }
    }
}