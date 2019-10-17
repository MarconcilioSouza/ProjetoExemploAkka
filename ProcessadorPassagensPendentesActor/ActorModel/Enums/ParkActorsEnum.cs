using System;
using System.ComponentModel;
using System.Reflection;

namespace ProcessadorPassagensActors.Enums
{
    public enum ParkActorsEnum
    {
        CoordinatorParkActor = 0,
        ValidarPassagemPendenteParkActor = 1,
        GerarPassagemParkActor = 2,
        ValidarPassagemParkActor = 3,
        ValidarPassagemSistemaParkActor = 4,
        GerarPassagemAprovadaParkActor = 5,
        GerarPassagemReprovadaParkActor = 6,
        GerarPassagemInvalidaParkActor = 7,
        ProcessarPassagemAprovadaParkActor = 8,
        ProcessarPassagemReprovadaParkActor = 9,
        ProcessarPassagemInvalidaParkActor = 10
    }
}