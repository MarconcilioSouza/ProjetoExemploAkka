using Akka.Actor;
using Common.Logging;
using ProcessadorPassagensActors.ActorsMessages.Artesp;
using ProcessadorPassagensActors.ActorsMessages.Edi;
using ProcessadorPassagensActors.ActorsUtils;

namespace ProcessadorPassagensActors.Actors.Edi
{
    
    public static class TransacaoEdiActorSystem
    {
        private static IActorRef _actorCoordinator;
        private static ILog _log;
        public static ActorSystem ActorSystem { get; private set; }

        public static void Iniciar()
        {
            _log = LogManager.GetLogger(typeof(TransacaoEdiActorSystem));

            //Cria o ActorSystem
            ActorSystem = ActorSystem.Create(ActorsPath.ActorSystem.Name);
            //Cria o coordenador no contexto
            _actorCoordinator = ActorsEdiCreator.CreateCoordinator(ActorSystem);
            _log.Debug("Iniciando sistema de atores...");

        }

        public static void Processar(CoordinatorEdiMessage mensagem)
        {
            //Envia para o router as mensagens a serem enviadas...
            _actorCoordinator.Tell(mensagem);
            _log.Debug($"Recebendo {mensagem.PassagensPendentesEdi.Count} mensagens...");
        }

        public static void Finalizar()
        {
            //Finaliza a thread principal do Akka.net...
            ActorSystem.Terminate();
            ActorSystem.WhenTerminated.Wait();
        }
    }
}