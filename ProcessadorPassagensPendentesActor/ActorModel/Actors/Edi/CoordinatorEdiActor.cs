using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using ProcessadorPassagensActors.ActorsMessages.Edi;
using ProcessadorPassagensActors.ActorsUtils;
using ProcessadorPassagensActors.Enums;

namespace ProcessadorPassagensActors.Actors.Edi
{
    public class CoordinatorEdiActor : ReceiveActor
    {
        private Dictionary<string, IActorRef> _actors;


        protected override void PreStart()
        {
            if (_actors == null)
                _actors = new Dictionary<string, IActorRef>();
        }

        public CoordinatorEdiActor()
        {
            _actors = new Dictionary<string, IActorRef>();
            
            //Recebe lista de trn...
            Receive<CoordinatorEdiMessage>(item => item.FluxoExecucao == EdiActorsEnum.CoordinatorEdiActor, Processar);       
        }

        private void Processar(CoordinatorEdiMessage item)
        {
            const string actorPrefix = "IdentificadorPassagemDuplicadaEdiActor_{0}";
            var conveniados = item.PassagensPendentesEdi.Select(x => x.Conveniado.CodigoProtocolo).Distinct();
            foreach (var conveniado in conveniados)
            {
                _actors.CreateIfNotExists<IdentificadorPassagemDuplicadaEdiActor>(Context, string.Format(actorPrefix, conveniado));
            }
            

            //Iniciando o processamento pelo fluxo de passagem...
            item.PassagensPendentesEdi.ForEach(i =>
            {
                _actors[string.Format(actorPrefix, i.Conveniado.CodigoProtocolo)].Tell(new IdentificadorPassagemDuplicadaEdiMessage
                {
                    PassagemPendenteEdi = i
                });
                
            });
        }
    }
}