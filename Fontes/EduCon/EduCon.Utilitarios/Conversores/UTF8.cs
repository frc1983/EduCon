using System.IO;
using System.Text;

namespace EduCon.Utilitarios.Conversores
{
    public class UTF8
    {
        public static string Converte(string caminhoArquivo)
        {
            var encoding = Encoding.Default;
            var textoArquivo = string.Empty;

            using (var reader = new StreamReader(caminhoArquivo, Encoding.Default))
            {
                textoArquivo = reader.ReadToEnd();
                encoding = reader.CurrentEncoding;
                reader.Close();
            }

            if (encoding == Encoding.UTF8)
            {
                return textoArquivo;
            }

            var bytes = encoding.GetBytes(textoArquivo);
            var convertedBytes = Encoding.Convert(encoding, Encoding.UTF8, bytes);
            return Encoding.UTF8.GetString(convertedBytes);
        }
    }
}