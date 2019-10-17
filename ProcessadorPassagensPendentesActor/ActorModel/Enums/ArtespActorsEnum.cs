using System;
using System.ComponentModel;
using System.Reflection;

namespace ProcessadorPassagensActors.Enums
{
    public enum ArtespActorsEnum
    {
        [Description("CoordinatorActor")]
        CoordinatorActor = 0,

        [Description("IdentificadorPassagemActor")]
        IdentificadorPassagemActor = 1,

        [Description("ValidadorPassagemExistenteActor")]
        ValidadorPassagemExistenteActor = 2,

        [Description("ValidadorPassagemPendenteActor")]
        ValidadorPassagemPendenteActor = 3,

        [Description("GeradorPassagemActor")]
        GeradorPassagemActor = 4,

        [Description("ValidadorPassagemActor")]
        ValidadorPassagemActor = 5,

        [Description("ValidadorPassagemSistemaActor")]
        ValidadorPassagemSistemaActor = 6,

        [Description("ValidadorPassagemValePedagioActor")]
        ValidadorPassagemValePedagioActor = 7,

        [Description("ValidadorDivergenciaCategoriaPassagemActor")]
        ValidadorDivergenciaCategoriaPassagemActor = 8,

        [Description("GeradorPassagemAprovadaActor")]
        GeradorPassagemAprovadaActor = 9,

        [Description("GeradorPassagemReprovadaActor")]
        GeradorPassagemReprovadaActor = 10,

        [Description("GeradorPassagemInvalidaActor")]
        GeradorPassagemInvalidaActor = 11,

        [Description("ProcessadorPassagemAprovadaActor")]
        ProcessadorPassagemAprovadaActor = 12,

        [Description("ProcessadorPassagemReprovadaActor")]
        ProcessadorPassagemReprovadaActor = 13,

        [Description("ProcessadorPassagemInvalidaActor")]
        ProcessadorPassagemInvalidaActor = 14,

        [Description("ValidadorPassagemPendenteConcessionariaActor")]
        ValidadorPassagemPendenteConcessionariaActor = 15,

        [Description("IdentificadorPassagemAceiteManualReenvioActor")]
        IdentificadorPassagemAceiteManualReenvioActor = 16,

        [Description("ValidadorPassagemPendenteAceiteManualReenvioActor")]
        ValidadorPassagemPendenteAceiteManualReenvioActor = 17,

        [Description("ValidadorPassagemAceiteManualReenvioActor")]
        ValidadorPassagemAceiteManualReenvioActor = 18,

        [Description("ValidadorPassagemSistemaAceiteManualReenvioActor")]
        ValidadorPassagemSistemaAceiteManualReenvioActor = 19
    }
}