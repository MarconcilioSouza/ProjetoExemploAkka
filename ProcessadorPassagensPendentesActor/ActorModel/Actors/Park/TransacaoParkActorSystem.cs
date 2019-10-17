using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Akka.Actor;
using ProcessadorPassagensActors.ActorsMessages.Edi;
using ProcessadorPassagensActors.ActorsMessages.Park;
using ProcessadorPassagensActors.ActorsUtils;

namespace ProcessadorPassagensActors.Actors.Park
{
    public class TransacaoParkActorSystem
    {
        private static IActorRef _actorCoordinator;
        private static ActorLogger _log;
        public static ActorSystem ActorSystem { get; private set; }

        public static void Iniciar()
        {
            _log = new ActorLogger();

            //Cria o ActorSystem
            ActorSystem = ActorSystem.Create(ActorsPath.ActorSystem.Name);
            //Cria o coordenador no contexto
            _actorCoordinator = ActorsParkCreator.CreateCoordinator(ActorSystem);
            _log.Info("Iniciando sistema de atores...");

        }

        public static void Processar(CoordinatorParkMessage mensagem)
        {
            //Envia para o router as mensagens a serem enviadas...
            _actorCoordinator.Tell(mensagem);
            _log.Info($"Recebendo {mensagem.PassagensPendentesEstacionamentos.Count} mensagens...");
        }

        public static void Finalizar()
        {
            //Finaliza a thread principal do Akka.net...
            ActorSystem.Terminate();
            ActorSystem.WhenTerminated.Wait();
        }
    }
}