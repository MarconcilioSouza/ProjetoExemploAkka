using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorPassagensActors.CommandQuery.Enums
{
    public enum PassagemPendenteEdiValidatorEnum
    {
        ValidarArquivoNulo,
        ValidarPossuiArquivoTrn,
        ValidarPossuiArquivoTrf,
        ValidarPossuiNumeroTag,
        ValidarPassagemManualComNumeroTagInvalida,
        ValidarPassagemListaNela,
        PassagemIsenta,
        PassagemValorZerado,
        ValidarPassagemIsentaComValor,
        ValidarPassagemComValorInvalido,
        PossuiTransacaoAprovadaManualmente,
        PossuiAdesaoAtiva,
        ValidarEmissorTagId,

        ValidarTempoSlaEnvioPassagem,
        ValidarCategoria,
        ValidarTag,
        ValidarAdesao,
        ValidarPistaPraca


    }
}
