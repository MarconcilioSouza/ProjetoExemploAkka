using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorPassagensActors.CommandQuery.Enums
{
    public enum EstacionamentoErros
    {
        [Description("Sucesso")]
        Sucesso = 0,

        [Description("Conveniado não é um estacionamento")]
        ConveniadoNaoEstacionamento = 1,

        [Description("Código do conveniado inválido")]
        ConveniadoInvalido = 2,

        [Description("Tag inválida")]
        TagInvalida = 3,

        [Description("Praça inválida para o conveniado")]
        PracaInvalida = 4,

        [Description("Pista inválida para a praça")]
        PistaInvalida = 5,

        [Description("Data de entrada maior que a data de transação")]
        DataEntradaMaiorTransacao = 6,

        [Description("Valor cobrado inferior à zero")]
        ValorCobradoMenorZero = 7,

        [Description("Valor do desconto inferior à zero")]
        ValorDescontoMenorZero = 8,

        [Description("Tempo de permanência inválido")]
        TempoPermanencia = 9,

        [Description("Configuração de repasse inválida para o conveniado")]
        ConfiguracaoRepasseInvalida = 10,

        [Description("Foi informado valor de desconto porém sem motivo")]
        DescontoInformadoSemMotivo = 11,

        [Description("Data / Hora da Transação fora do Período de Aceitação cadastrado para o Conveniado")]
        TransacaoForaIntervalo = 12,

        [Description("Erro de aplicação")]
        ErroAplicacao = 14,

        [Description("Erro na requisição")]
        ErroRequisicao = 15,

        [Description("Data de transação maior que a data de processamento")]
        DataTransacaoMaiorProcessamento = 16,

        [Description("Data de transação ultrapassa o limite permitido")]
        DataTransacaoUltrapassaLimitePermitido = 17,

        [Description("Valor da transação ultrapassa o limite permitido")]
        ValorTransacaoUltrapassaLimitePermitido = 18,

        [Description("Codigo do conveniado não informado")]
        ConveniadoNaoInformado = 19,

        [Description("Tag não informada")]
        TagNaoInformada = 20,

        [Description("Praca não informada")]
        PracaNaoInformada = 21,

        [Description("Pista não informada")]
        PistaNaoInformada = 22,

        [Description("Transacao repetida")]
        TransacaoRepetida = 23
    }
}
