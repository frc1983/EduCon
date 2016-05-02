using System;

namespace EduCon.Utilitarios.Extensoes
{
    public static class Extensoes
    {
        public static bool TemValorInformado(this Type tipo, object valor)
        {
            // É value type
            if (tipo.IsValueType)
            {
                // É Nullable
                if (tipo.IsGenericType)
                {
                    return !(valor == null);
                }

                // Tipos primitivos
                return !(valor.Equals(Activator.CreateInstance(tipo)));
            }

            // É reference type
            string vlrStr = valor as string;
            if (vlrStr != null)
            {
                return !string.IsNullOrEmpty(vlrStr.Trim());
            }

            return !(valor == null);
        }
    }
}