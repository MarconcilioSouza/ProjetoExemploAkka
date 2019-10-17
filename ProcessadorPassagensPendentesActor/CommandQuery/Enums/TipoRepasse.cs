using System.ComponentModel;

namespace ProcessadorPassagensActors.CommandQuery.Enums
{
    public enum TipoRepasse
    {
        [Description("Quantidade de dias após vencimento")]
        QuantidadeDeDiasAposVencimento = 1,
        [Description("Dia fixo do mês")]
        DiaFixoDoMes = 2,
        [Description("Quantidade de dias após transações")]
        QuantidadeDeDiasAposTransacoes = 3,
        [Description("Quantidade de dias úteis após vencimento")]
        QuantidadeDeDiasUteisAposVencimento = 4,
        [Description("Quantidade de dias corridos após vencimento")]
        QuantidadeDeDiasCorridosAposVencimento = 5
    }
}
