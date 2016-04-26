using EduCon.Aplicacao.Mapeamento.Base;

namespace EduCon.Aplicacao
{
    public class InicializaAplicacao
    {
        public static void Inicia()
        {
            Mapeadores.Registra();
        }
    }
}