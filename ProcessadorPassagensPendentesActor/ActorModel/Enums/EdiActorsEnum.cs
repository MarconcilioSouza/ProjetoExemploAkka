using System;
using System.ComponentModel;
using System.Reflection;

namespace ProcessadorPassagensActors.Enums
{
    public enum EdiActorsEnum
    {
        CoordinatorEdiActor = 0,
        IdentificadorPassagemDuplicadaEdiActor = 1,
        ProcessadorPassagemAprovadaEdiActor = 2,
        ProcessadorPassagemReprovadaEdiActor = 3,
        ValidadorDivergenciaCategoriaEdiActor = 4,
        ValidadorPassagemEdiActor = 5,
        ValidadorPassagemPendenteEdiActor = 6,
        ValidadorPassagemSistemaEdiActor = 7,
        ValidadorPassagemValePedagioEdiActor = 8,
        ValidadorSlaListaNelaEdiActor = 9,
        GeradorPassagemAprovadaEdiActor = 10,
        GeradorPassagemEdiActor = 11,
        GeradorPassagemReprovadaEdiActor = 12

    }
}