using System;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace EduCon.ImportaFee.Infra
{
    public class Download
    {
        public static void BaixarEDescompactar(string url, string diretorio)
        {
            using (var wc = new WebClient())
            {
                var nomeCompleto = diretorio + Guid.NewGuid().ToString();

                wc.DownloadFile(url, nomeCompleto);

                ZipFile.ExtractToDirectory(nomeCompleto, diretorio);

                var zip = new FileInfo(nomeCompleto);
                zip.Delete();
            }
        }
    }
}