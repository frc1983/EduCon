using System.Collections.Generic;
using System.Linq;
using EduCon.Utilitarios.Extensoes;

namespace EduCon.Utilitarios.Aplicacao.Utilitarios
{
    public class Filtro
    {
        public string Propriedade { get; set; }
        public object Valor { get; set; }

        public EnumOperador Operador { get; set; }

        public Filtro()
        {
            Operador = EnumOperador.Igual;
        }

        public static IList<Filtro> Filtros(object objeto, bool ignorarColecoes = true)
        {
            var filtros = new List<Filtro>();

            if (objeto == null)
                return filtros;

            foreach (var propriedade in objeto.GetType().GetProperties())
            {
                if (ignorarColecoes
                    && (
                        propriedade.PropertyType.IsGenericType && typeof(ICollection<>).IsAssignableFrom(propriedade.PropertyType.GetGenericTypeDefinition())
                    )
                    || propriedade.PropertyType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>))
                )
                {
                    continue;
                }

                if (!propriedade.PropertyType.TemValorInformado(propriedade.GetValue(objeto, null)))
                {
                    continue;
                }

                if (propriedade.PropertyType.Equals(typeof(string)))
                {
                    filtros.Add(new Filtro() { Propriedade = propriedade.Name, Valor = propriedade.GetValue(objeto, null), Operador = EnumOperador.Contem });
                }
                else
                {
                    filtros.Add(new Filtro() { Propriedade = propriedade.Name, Valor = propriedade.GetValue(objeto, null) });
                }
            }

            return filtros;
        }
    }
}