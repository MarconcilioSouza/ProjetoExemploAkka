using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using ProcessadorPassagensActors.ActorsMessages.Artesp;
using ProcessadorPassagensActors.ActorsUtils;
using ProcessadorPassagensActors.Enums;
using System.Collections.Async;
using System.Threading.Tasks;
using ProcessadorPassagensActors.CommandQuery.Cache;
using Common.Logging;

namespace ProcessadorPassagensActors.Actors.Artesp
{
    public class CoordinatorArtespActor : ReceiveActor
    {
        private Dictionary<string, IActorRef> _actors;
        private string _actorPrefix;
        private ILog _log;

        protected Dictionary<ArtespActorsEnum, IActorRef> Workers;

        protected override void PreStart()
        {
            Workers = ActorsArtespCreator.CreateCoordinatorArtespChildrenActors(Context);
            if (_actors == null)
            {
                _actors = new Dictionary<string, IActorRef>();
                //ActorsActivator();
            }
                
        }

        public CoordinatorArtespActor()
        {
            
            _actors = new Dictionary<string, IActorRef>();
            _log = LogManager.GetLogger(typeof(CoordinatorArtespActor));

            // obtem todos os conveniados artesp e gera os atores
            _actorPrefix = "IdentificadorPassagemActor_{0}";
            //ActorsActivator();


            //Recebe lista de passagens...
            Receive<CoordinatorArtespMessage>(item => item.FluxoExecucao == ArtespActorsEnum.CoordinatorActor, Processar);       
        }

        private void ActorsActivator()
        {
            var conveniados = PistaPracaConveniadoArtespCacheRepository.Listar()
                .Select(x => string.Format("{0}{1}{2}",
                    x.CodigoProtocoloArtesp,
                    x.CodigoPraca,
                    x.CodigoPista)).Distinct();

            var qdtAtores = conveniados.Count();
            var countAtores = 1;

            _log.Debug($"Actor System - Início da criação de {qdtAtores} Atores.");


            foreach (var conveniado in conveniados)
            {
                _log.Debug($"Actor System - Criando Ator {conveniado} ({countAtores})");
                _actors.CreateIfNotExists<IdentificadorPassagemArtespActor>(Context, string.Format(_actorPrefix, conveniado));
                countAtores++;
            }

            _log.Debug($"Actor System - Fim da criação de {qdtAtores} Atores");
        }

        private void Processar(CoordinatorArtespMessage item)
        {
            Workers[ArtespActorsEnum.IdentificadorPassagemActor].Tell(new IdentificadorPassagemArtespMessage
            {
                PassagemPendenteArtesp = item.PassagemPendenteArtesp
            });

            //Parallel.ForEach(item.PassagensPendentesArtesp, (i) =>
            //{
            //    var identificador = string.Format("{0}{1}{2}", i.Conveniado.CodigoProtocoloArtesp,
            //        i.Praca.CodigoPraca,
            //        i.Pista.CodigoPista);

            //    Workers[ArtespActorsEnum.IdentificadorPassagemActor].Tell(new IdentificadorPassagemArtespMessage
            //    {
            //        PassagemPendenteArtesp = i
            //    });

            //});


            ////var tags = item.PassagensPendentesArtesp.Select(x => x.Tag.OBUId).Distinct();
            ////foreach (var tag in tags)
            ////{
            ////    _actors.CreateIfNotExists<IdentificadorPassagemArtespActor>(Context, string.Format(actorPrefix, tag));
            ////}

            //var conveniados = item.PassagensPendentesArtesp
            //    .Select(x => string.Format("{0}{1}{2}",
            //        x.Conveniado.CodigoProtocoloArtesp,
            //        x.Praca.CodigoPraca,
            //        x.Pista.CodigoPista)).Distinct();

            //foreach (var conveniado in conveniados)
            //{                
            //    _actors.CreateIfNotExists<IdentificadorPassagemArtespActor>(Context, string.Format(_actorPrefix, conveniado));                
            //}

            //_log.Debug($"Iniciando o processamento de {item.PassagensPendentesArtesp.Count} passagens.");

            ////TODO: colocar processamento paralelo para envio
            ////TODO: criar fila por Conveniado          
            //Parallel.ForEach(item.PassagensPendentesArtesp, (i) =>
            //{
            //    var identificador = string.Format("{0}{1}{2}", i.Conveniado.CodigoProtocoloArtesp,
            //        i.Praca.CodigoPraca,
            //        i.Pista.CodigoPista);

            //    _actors[string.Format(_actorPrefix, identificador)].Tell(new IdentificadorPassagemArtespMessage
            //    {
            //        PassagemPendenteArtesp = i
            //    });
            //});            
        }
    }
}