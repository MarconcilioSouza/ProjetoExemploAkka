using Akka.Actor;
using Common.Logging;
using ConectCar.Cadastros.Domain.Dto;
using ConectCar.Comercial.Domain.Model;
using ConectCar.Framework.Domain.Model;
using ProcessadorPassagensActors.ActorsMessages.Artesp;
using ProcessadorPassagensActors.ActorsUtils;
using ProcessadorPassagensActors.CommandQuery.Cache;
using System.Collections.Generic;
using System.Linq;

namespace ProcessadorPassagensActors.Actors.Artesp
{

    public static class TransacaoArtespActorSystem
    {
        private static ActorSystem _actorSystem;
        private static IActorRef _actorCoordinator;
        private static ILog _log;

        public static ActorSystem ActorSystem => _actorSystem;

        public static void Iniciar()
        {
            _log = LogManager.GetLogger(typeof(TransacaoArtespActorSystem));

            //Cria o ActorSystem
            _log.Debug("Actor System - Inicializaçao do Sistema de Atores");
            _actorSystem = ActorSystem.Create(ActorsPath.ActorSystem.Name);

            //Cria o coordenador no contexto
            _actorCoordinator = ActorsArtespCreator.CreateCoordinator(_actorSystem);
            _log.Debug("Actor System - Fim da Inicialização do Sistema de Atores");

        }

        public static void Processar(CoordinatorArtespMessage mensagem)
        {
            //Envia para o router as mensagens a serem enviadas...
            _actorCoordinator.Tell(mensagem);
        }

        public static void Finalizar()
        {
            //Finaliza a thread principal do Akka.net...
            _actorSystem.Terminate();
            _actorSystem.WhenTerminated.Wait();
        }
    }
}