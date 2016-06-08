using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace EduCon.Utilitarios.Aplicacao
{
    public class Expressao
    {
        private static MethodInfo contem = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        private static MethodInfo comecaCom = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
        private static MethodInfo terminaCom = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });

        public static Expression<Func<T, bool>> CriaExpressao<T>(IList<Filtro> filtros)
        {
            if (filtros.Count == 0)
                return null;

            var parametro = Expression.Parameter(typeof(T));

            Expression exp = null;
            foreach (var filtro in filtros)
            {
                exp = (exp == null ? CriaExpressao<T>(parametro, filtro) : Expression.AndAlso(exp, CriaExpressao<T>(parametro, filtro)));
            }

            return Expression.Lambda<Func<T, bool>>(exp, parametro);
        }

        private static Expression CriaExpressao<T>(ParameterExpression parametro, Filtro filtro)
        {
            var propriedade = Expression.Property(parametro, filtro.Propriedade);

            Expression valor = null;
            if (typeof(T).GetProperty(filtro.Propriedade).PropertyType.IsGenericType)
            {
                valor = Expression.Constant(Convert.ChangeType(filtro.Valor, propriedade.Type.GetGenericArguments()[0]));
                valor = Expression.Convert(valor, propriedade.Type);
            }
            else
            {
                valor = Expression.Constant(filtro.Valor);
            }

            switch (filtro.Operador)
            {
                case EnumOperador.Igual:
                    return Expression.Equal(propriedade, valor);
                case EnumOperador.Diferente:
                    return Expression.NotEqual(propriedade, valor);
                case EnumOperador.Contem:
                    return Expression.Call(propriedade, contem, valor);
                case EnumOperador.Maior:
                    return Expression.GreaterThan(propriedade, valor);
                case EnumOperador.MaiorIgual:
                    return Expression.GreaterThanOrEqual(propriedade, valor);
                case EnumOperador.Menor:
                    return Expression.LessThan(propriedade, valor);
                case EnumOperador.MenorIgual:
                    return Expression.LessThanOrEqual(propriedade, valor);
                case EnumOperador.ComecaCom:
                    return Expression.Call(propriedade, comecaCom, valor);
                case EnumOperador.TerminaCom:
                    return Expression.Call(propriedade, terminaCom, valor);
            }

            return null;
        }
    }
}