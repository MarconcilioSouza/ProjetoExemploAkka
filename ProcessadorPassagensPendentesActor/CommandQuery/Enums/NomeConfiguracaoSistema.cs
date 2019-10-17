using System;
using System.ComponentModel;
using System.Reflection;

namespace ProcessadorPassagensActors.CommandQuery.Enums
{
    public enum NomeConfiguracaoSistema
    {
        [Description("MaximoMensagensPorTaskQueueServiceBus")]
        MaximoMensagensPorTaskQueueServiceBus,

        [Description("OsaId")]
        OsaID,

        [Description("QuantidadeDiasLimiteEstornoPassagem")]
        QuantidadeDiasLimiteEstornoPassagem,

        [Description("SaldoMinimoNegativo")]
        SaldoMinimoNegativo,

        [Description("TempoAceitePassagem")]
        TempoAceitePassagem,

        [Description("DiasLimiteEstornoPassagem")]
        DiasLimiteEstornoPassagem,

        [Description("HorarioDePassagem")]
        HorarioDePassagem,

        [Description("ConfiguracaoItemPendenteProcessamentoTtlEmMinutos")]
        ConfiguracaoItemPendenteProcessamentoTtlEmMinutos,

        [Description("QuantidadeTentativasProcessamentoMensagemItem")]
        QuantidadeTentativasProcessamentoMensagemItem,

        [Description("QuantidadeDePassagensParaConfirmacaoDeCategoria")]
        QuantidadeDePassagensParaConfirmacaoDeCategoria,

        [Description("QuantidadeLimiteDeDivergenciasParaReiniciarConfirmacaoDeCategoria")]
        QuantidadeLimiteDeDivergenciasParaReiniciarConfirmacaoDeCategoria

    }
}
