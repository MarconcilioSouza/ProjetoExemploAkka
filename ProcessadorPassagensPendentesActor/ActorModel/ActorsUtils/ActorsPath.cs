using System;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensActors.Enums;

namespace ProcessadorPassagensActors.ActorsUtils
{
    public static class ActorsPath
    {
        public static readonly ActorMetaData ActorSystem = new ActorMetaData("actorSystem", string.Empty);

        #region Artesp
        public static readonly ActorMetaData CoordinatorArtespActor = new ActorMetaData("coordinatorArtesp", "akka://TransacaoArtesp/user/coordinator");
        public static readonly ActorMetaData IdentificadorPassagemArtespActor = new ActorMetaData("IdentificadorPassagemArtespActor", "akka://TransacaoArtesp/user/coordinator/IdentificadorPassagemActor");
        public static readonly ActorMetaData ValidadorPassagemExistenteArtespActor = new ActorMetaData("ValidadorPassagemExistenteArtespActor", "akka://TransacaoArtesp/user/coordinator/ValidadorPassagemExistenteActor");
        public static readonly ActorMetaData ValidadorPassagemPendenteArtespActor = new ActorMetaData("ValidadorPassagemPendenteArtespActor", "akka://TransacaoArtesp/user/coordinator/ValidadorPassagemPendenteActor");
        public static readonly ActorMetaData GeradorPassagemArtespActor = new ActorMetaData("GeradorPassagemArtespActor", "akka://TransacaoArtesp/user/coordinator/GeradorPassagemActor");
        public static readonly ActorMetaData ValidadorPassagemArtespActor = new ActorMetaData("ValidadorPassagemArtespActor", "akka://TransacaoArtesp/user/coordinator/ValidadorPassagemActor");
        public static readonly ActorMetaData ValidadorPassagemSistemaArtespActor = new ActorMetaData("ValidadorPassagemSistemaArtespActor", "akka://TransacaoArtesp/user/coordinator/ValidadorPassagemSistemaActor");
        public static readonly ActorMetaData ValidadorPassagemValePedagioArtespActor = new ActorMetaData("ValidadorPassagemValePedagioArtespActor", "akka://TransacaoArtesp/user/coordinator/ValidadorPassagemValePedagioActor");
        public static readonly ActorMetaData ValidadorDivergenciaCategoriaPassagemArtespActor = new ActorMetaData("ValidadorDivergenciaCategoriaPassagemArtespActor", "akka://TransacaoArtesp/user/coordinator/ValidadorDivergenciaCategoriaPassagemActor");
        public static readonly ActorMetaData GeradorPassagemAprovadaArtespActor = new ActorMetaData("GeradorPassagemAprovadaArtespActor", "akka://TransacaoArtesp/user/coordinator/GeradorPassagemAprovadaActor");
        public static readonly ActorMetaData GeradorPassagemReprovadaArtespActor = new ActorMetaData("GeradorPassagemReprovadaArtespActor", "akka://TransacaoArtesp/user/coordinator/GeradorPassagemReprovadaActor");
        public static readonly ActorMetaData GeradorPassagemInvalidaArtespActor = new ActorMetaData("GeradorPassagemInvalidaArtespActor", "akka://TransacaoArtesp/user/coordinator/GeradorPassagemInvalidaActor");
        public static readonly ActorMetaData ProcessadorPassagemAprovadaArtespActor = new ActorMetaData("ProcessadorPassagemAprovadaArtespActor", "akka://TransacaoArtesp/user/coordinator/ProcessadorPassagemAprovadaActor");
        public static readonly ActorMetaData ProcessadorPassagemReprovadaArtespActor = new ActorMetaData("ProcessadorPassagemReprovadaArtespActor", "akka://TransacaoArtesp/user/coordinator/ProcessadorPassagemReprovadaActor");
        public static readonly ActorMetaData ProcessadorPassagemInvalidaArtespActor = new ActorMetaData("ProcessadorPassagemInvalidaArtespActor", "akka://TransacaoArtesp/user/coordinator/ProcessadorPassagemInvalidaActor");
        public static readonly ActorMetaData ValidadorPassagemPendenteConcessionariaArtespActor = new ActorMetaData("ValidadorPassagemPendenteConcessionariaArtespActor", "akka://TransacaoArtesp/user/coordinator/ValidadorPassagemPendenteConcessionariaActor");
        public static readonly ActorMetaData IdentificadorPassagemAceiteManualReenvioArtespActor = new ActorMetaData("IdentificadorPassagemAceiteManualReenvioActor", "akka://TransacaoArtesp/user/coordinator/IdentificadorPassagemAceiteManualReenvioActor");
        public static readonly ActorMetaData ValidadorPassagemPendenteAceiteManualReenvioArtespActor = new ActorMetaData("ValidadorPassagemPendenteAceiteManualReenvioArtespActor", "akka://TransacaoArtesp/user/coordinator/ValidadorPassagemPendenteAceiteManualReenvioActor");
        public static readonly ActorMetaData ValidadorPassagemAceiteManualReenvioArtespActor = new ActorMetaData("ValidadorPassagemAceiteManualReenvioArtespActor", "akka://TransacaoArtesp/user/coordinator/ValidadorPassagemAceiteManualReenvioActor");
        public static readonly ActorMetaData ValidadorPassagemSistemaAceiteManualReenvioArtespActor = new ActorMetaData("ValidadorPassagemSistemaAceiteManualReenvioArtespActor", "akka://TransacaoArtesp/user/coordinator/ValidadorPassagemSistemaAceiteManualReenvioActor"); 
        #endregion

        #region EDI
        public static readonly ActorMetaData CoordinatorEdiActor = new ActorMetaData("coordinatorEdi", "");
        public static readonly ActorMetaData IdentificadorPassagemDuplicadaEdiActor = new ActorMetaData("IdentificadorPassagemDuplicadaEdiActor", "");
        public static readonly ActorMetaData ValidadorPassagemPendenteEdiActor = new ActorMetaData("ValidadorPassagemPendenteEdiActor", "");
        public static readonly ActorMetaData GeradorPassagemEdiActor = new ActorMetaData("GeradorPassagemEdiActor", "");
        public static readonly ActorMetaData ValidadorPassagemEdiActor = new ActorMetaData("ValidadorPassagemEdiActor", "");
        public static readonly ActorMetaData ValidadorPassagemSistemaEdiActor = new ActorMetaData("ValidadorPassagemSistemaEdiActor", "");
        public static readonly ActorMetaData ValidadorSlaListaNelaEdiActor = new ActorMetaData("ValidadorSlaListaNelaEdiActor", "");
        public static readonly ActorMetaData ValidadorDivergenciaCategoriaEdiActor = new ActorMetaData("ValidadorDivergenciaCategoriaEdiActor", "");
        public static readonly ActorMetaData ValidadorPassagemValePedagioEdiActor = new ActorMetaData("ValidadorPassagemValePedagioEdiActor", "");
        public static readonly ActorMetaData GeradorPassagemAprovadaEdiActor = new ActorMetaData("GeradorPassagemAprovadaEdiActor", "");
        public static readonly ActorMetaData GeradorPassagemReprovadaEdiActor = new ActorMetaData("GeradorPassagemReprovadaEdiActor", "");
        public static readonly ActorMetaData ProcessadorPassagemAprovadaEdiActor = new ActorMetaData("ProcessadorPassagemAprovadaEdiActor", "");
        public static readonly ActorMetaData ProcessadorPassagemReprovadaEdiActor = new ActorMetaData("ProcessadorPassagemReprovadaEdiActor", "");
        #endregion

        #region Park
        public static readonly ActorMetaData CoordinatorParkActor = new ActorMetaData(ParkActorsEnum.CoordinatorParkActor.ToString(), "");
        public static readonly ActorMetaData ValidarPassagemPendenteParkActor = new ActorMetaData(ParkActorsEnum.ValidarPassagemPendenteParkActor.ToString(), "");
        public static readonly ActorMetaData GerarPassagemParkActor = new ActorMetaData(ParkActorsEnum.GerarPassagemParkActor.ToString(), "");
        public static readonly ActorMetaData ValidarPassagemParkActor = new ActorMetaData(ParkActorsEnum.ValidarPassagemParkActor.ToString(), "");
        public static readonly ActorMetaData ValidarPassagemSistemaParkActor = new ActorMetaData(ParkActorsEnum.ValidarPassagemSistemaParkActor.ToString(), "");
        public static readonly ActorMetaData GerarPassagemAprovadaParkActor = new ActorMetaData(ParkActorsEnum.GerarPassagemAprovadaParkActor.ToString(), "");
        public static readonly ActorMetaData GerarPassagemReprovadaParkActor = new ActorMetaData(ParkActorsEnum.GerarPassagemReprovadaParkActor.ToString(), "");
        public static readonly ActorMetaData GerarPassagemInvalidaParkActor = new ActorMetaData(ParkActorsEnum.GerarPassagemInvalidaParkActor.ToString(), "");
        public static readonly ActorMetaData ProcessarPassagemAprovadaParkActor = new ActorMetaData(ParkActorsEnum.ProcessarPassagemAprovadaParkActor.ToString(), "");
        public static readonly ActorMetaData ProcessarPassagemReprovadaParkActor = new ActorMetaData(ParkActorsEnum.ProcessarPassagemReprovadaParkActor.ToString(), "");
        public static readonly ActorMetaData ProcessarPassagemInvalidaParkActor = new ActorMetaData(ParkActorsEnum.ProcessarPassagemInvalidaParkActor.ToString(), "");
        #endregion

    }


    /// <summary>
    /// Meta-data class
    /// </summary>
    public class ActorMetaData
    {
        public ActorMetaData(string name, string path)
        {
            Name = name;
            Path = path;
        }
        public string Name { get; private set; }

        public string Path { get; private set; }
    }
}