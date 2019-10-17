using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using ProcessadorPassagensActors.ActorsMessages.Park;
using ProcessadorPassagensActors.ActorsUtils;
using ProcessadorPassagensActors.Enums;

namespace ProcessadorPassagensActors.Actors.Park
{
    public class CoordinatorParkActor : ReceiveActor
    {
        private Dictionary<string, IActorRef> _actors;

        protected override void PreStart()
        {
            if (_actors == null)
                _actors = new Dictionary<string, IActorRef>();
        }

        public CoordinatorParkActor()
        {
            _actors = new Dictionary<string, IActorRef>();
            Receive<CoordinatorParkMessage>(item => item.FluxoExecucao == ParkActorsEnum.CoordinatorParkActor, Processar);
        }

        private void Processar(CoordinatorParkMessage item)
        {
            const string actorPrefix = "ValidarPassagemPendenteParkActor_{0}";
            var conveniados = item.PassagensPendentesEstacionamentos.Select(x => x.Conveniado.CodigoProtocolo).Distinct();
            foreach (var conveniado in conveniados)
            {
                _actors.CreateIfNotExists<ValidarPassagemPendenteParkActor>(Context, string.Format(actorPrefix, conveniado));
            }

            //Iniciando o processamento pelo fluxo de passagem...
            item.PassagensPendentesEstacionamentos.ForEach(i =>
            {
                _actors[string.Format(actorPrefix, i.Conveniado.CodigoProtocolo)].Tell(new ValidarPassagemPendenteParkMessage
                {
                    PassagemPendenteEstacionamento = i
                });
            });
        }
    }
}