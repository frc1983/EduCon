using System.ComponentModel;

namespace EduCon.Dominio.Entidades.Enums
{
    public enum SituacaoProcessamento
    {
        [Description("Aguardando processamento")]
        Aguardando = 1,
        [Description("Processando")]
        Processando,
        [Description("Processamento finalizado")]
        Processado,
        [Description("Aguardando reprocessamento")]
        Reprocessar,
        [Description("Reprocessamento")]
        Reprocessando,
        [Description("Reprocessamento finalizado")]
        Reprocessado,
        [Description("Erro no processamento")]
        Erro
    }
}