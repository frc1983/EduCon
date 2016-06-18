using System;
using System.ComponentModel;
using System.Linq;

namespace EduCon.Utilitarios.Aplicacao
{
    public class DescricaoEnum
    {
        /// <summary>
        /// Recupera o atributo de descrição da opção do enum
        /// </summary>
        /// <param name="tipoEnum"></param>
        /// <returns></returns>
        public static string ObtemDescricao(Enum tipoEnum)
        {
            var campo = tipoEnum.GetType().GetField(tipoEnum.ToString());
            var atributos = (DescriptionAttribute[])campo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return atributos.FirstOrDefault().Description.ToString();
        }
    }
}