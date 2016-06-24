using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EduCon.Utilitarios.Api
{
    public class DecimalConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(decimal) || objectType == typeof(decimal?));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            if (token.Type == JTokenType.Float || token.Type == JTokenType.Integer)
            {
                return token.ToObject<decimal>();
            }

            if (token.Type == JTokenType.Null && objectType == typeof(decimal?))
            {
                return null;
            }

            if (token.Type == JTokenType.String)
            {
                var value = decimal.Parse(token.ToString());

                // Se tem dígitos significativos após 2 casas decimais, persiste no estado natural.
                if (!HasTwoDigitsOnly(value))
                {
                    return value;
                }

                // Converte para formato brasileiro (pt-BR).
                return decimal.Parse(token.ToString(), CultureInfo.GetCultureInfo("pt-BR"));
            }

            throw new JsonSerializationException("Tipo inesperado: " + token.Type.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value.GetType() == typeof(decimal))
            {
                var decValue = decimal.Parse(value.ToString());

                // Verifica se tem apenas 2 casas decimais.
                if (HasTwoDigitsOnly(decValue))
                {
                    writer.WriteValue(string.Format("{0:N2}", value));
                }
                else
                {
                    // Se tem dígitos significativos após 2 casas decimais, persiste no estado natural.
                    writer.WriteValue(value);
                }
            }

            if (value.GetType() == typeof(decimal?))
            {
                writer.WriteValue(string.Format("{0:N2}", (value as decimal?).Value));
            }
        }

        private bool HasTwoDigitsOnly(decimal value)
        {
            return ((decimal.Round(value, 2) - value) == decimal.Zero);
        }
    }
}