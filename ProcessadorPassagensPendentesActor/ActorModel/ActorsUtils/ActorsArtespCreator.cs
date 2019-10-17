using System.Collections.Generic;
using Akka.Actor;
using ProcessadorPassagensActors.Enums;
using Akka.Routing;
using ProcessadorPassagensActors.Actors.Artesp;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.ActorsUtils
{
    public static class ActorsArtespCreator
    {
        public static Dictionary<ArtespActorsEnum, IActorRef> CreateCoordinatorArtespChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ArtespActorsEnum, IActorRef>
            {
                {
                    ArtespActorsEnum.IdentificadorPassagemActor,
                    context.ActorOf(Props.Create(() => new IdentificadorPassagemArtespActor()),
                        ActorsPath.IdentificadorPassagemArtespActor.Name)
                }
            };

            return actors;
        }

        public static Dictionary<ArtespActorsEnum, IActorRef> CreateIdentificadorPassagemChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ArtespActorsEnum, IActorRef>
            {
                {
                    ArtespActorsEnum.ValidadorPassagemExistenteActor,
                    context.ActorOf(Props.Create(() => new ValidadorPassagemExistenteArtespActor()),
                        ActorsPath.ValidadorPassagemExistenteArtespActor.Name)
                },
                {
                    ArtespActorsEnum.ValidadorPassagemPendenteConcessionariaActor,
                    context.ActorOf(Props.Create(() => new ValidadorPassagemPendenteConcessionariaArtespActor()),
                        ActorsPath.ValidadorPassagemPendenteConcessionariaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.GeradorPassagemInvalidaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemInvalidaArtespActor()),
                        ActorsPath.GeradorPassagemInvalidaArtespActor.Name)
                }
            };


            return actors;
        }

        public static Dictionary<ArtespActorsEnum, IActorRef> CreateValidadorPassagemExistenteChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ArtespActorsEnum, IActorRef>
            {
                {
                    ArtespActorsEnum.GeradorPassagemAprovadaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemAprovadaArtespActor()),
                        ActorsPath.GeradorPassagemAprovadaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.GeradorPassagemReprovadaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaArtespActor()),
                        ActorsPath.GeradorPassagemReprovadaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.GeradorPassagemInvalidaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemInvalidaArtespActor()),
                        ActorsPath.GeradorPassagemInvalidaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.ValidadorPassagemPendenteConcessionariaActor,
                    context.ActorOf(Props.Create(() => new ValidadorPassagemPendenteConcessionariaArtespActor()),
                        ActorsPath.ValidadorPassagemPendenteConcessionariaArtespActor.Name)
                }
            };



            return actors;
        }

        public static Dictionary<ArtespActorsEnum, IActorRef> CreateValidadorPassagemPendenteChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ArtespActorsEnum, IActorRef>
            {
                {
                    ArtespActorsEnum.GeradorPassagemReprovadaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaArtespActor()),
                        ActorsPath.GeradorPassagemReprovadaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.GeradorPassagemInvalidaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemInvalidaArtespActor()),
                        ActorsPath.GeradorPassagemInvalidaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.GeradorPassagemActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemArtespActor()),
                        ActorsPath.GeradorPassagemArtespActor.Name)
                }
            };


            return actors;
        }

        public static Dictionary<ArtespActorsEnum, IActorRef> CreateGeradorPassagemChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ArtespActorsEnum, IActorRef>
            {
                {
                    ArtespActorsEnum.GeradorPassagemReprovadaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaArtespActor()),
                        ActorsPath.GeradorPassagemReprovadaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.GeradorPassagemInvalidaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemInvalidaArtespActor()),
                        ActorsPath.GeradorPassagemInvalidaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.ValidadorPassagemActor,
                    context.ActorOf(Props.Create(() => new ValidadorPassagemArtespActor()),
                        ActorsPath.ValidadorPassagemArtespActor.Name)
                },
                {
                    ArtespActorsEnum.ValidadorPassagemAceiteManualReenvioActor,
                    context.ActorOf(Props.Create(() => new ValidadorPassagemAceiteManualReenvioArtespActor()),
                        ActorsPath.ValidadorPassagemAceiteManualReenvioArtespActor.Name)
                }
            };


            return actors;
        }

        public static Dictionary<ArtespActorsEnum, IActorRef> CreateValidadorPassagemChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ArtespActorsEnum, IActorRef>
            {
                {
                    ArtespActorsEnum.GeradorPassagemReprovadaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaArtespActor()),
                        ActorsPath.GeradorPassagemReprovadaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.GeradorPassagemAprovadaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemAprovadaArtespActor()),
                        ActorsPath.GeradorPassagemAprovadaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.ProcessadorPassagemAprovadaActor,
                    context.ActorOf(Props.Create(() => new ProcessadorPassagemAprovadaArtespActor()),
                        ActorsPath.ProcessadorPassagemAprovadaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.ValidadorPassagemSistemaActor,
                    context.ActorOf(Props.Create(() => new ValidadorPassagemSistemaArtespActor()),
                        ActorsPath.ValidadorPassagemSistemaArtespActor.Name)
                }
            };


            return actors;
        }

        public static Dictionary<ArtespActorsEnum, IActorRef> CreateValidadorPassagemSistemaChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ArtespActorsEnum, IActorRef>
            {
                {
                    ArtespActorsEnum.GeradorPassagemReprovadaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaArtespActor()),
                        ActorsPath.GeradorPassagemReprovadaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.GeradorPassagemInvalidaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemInvalidaArtespActor()),
                        ActorsPath.GeradorPassagemInvalidaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.ValidadorPassagemValePedagioActor,
                    context.ActorOf(Props.Create(() => new ValidadorPassagemValePedagioArtespActor()),
                        ActorsPath.ValidadorPassagemValePedagioArtespActor.Name)
                }
            };


            return actors;
        }

        public static Dictionary<ArtespActorsEnum, IActorRef> CreateValidadorPassagemValePedagioChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ArtespActorsEnum, IActorRef>
            {
                {
                    ArtespActorsEnum.GeradorPassagemReprovadaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaArtespActor()),
                        ActorsPath.GeradorPassagemReprovadaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.GeradorPassagemInvalidaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemInvalidaArtespActor()),
                        ActorsPath.GeradorPassagemInvalidaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.ValidadorDivergenciaCategoriaPassagemActor,
                    context.ActorOf(Props.Create(() => new ValidadorDivergenciaCategoriaPassagemArtespActor()),
                        ActorsPath.ValidadorDivergenciaCategoriaPassagemArtespActor.Name)
                }
            };


            return actors;
        }


        public static Dictionary<ArtespActorsEnum, IActorRef> CreateValidadorDivergenciaCategoriaPassagemChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ArtespActorsEnum, IActorRef>
            {
                {
                    ArtespActorsEnum.GeradorPassagemReprovadaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaArtespActor()),
                        ActorsPath.GeradorPassagemReprovadaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.GeradorPassagemInvalidaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemInvalidaArtespActor()),
                        ActorsPath.GeradorPassagemInvalidaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.GeradorPassagemAprovadaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemAprovadaArtespActor()),
                        ActorsPath.GeradorPassagemAprovadaArtespActor.Name)
                }
            };


            return actors;
        }

        public static Dictionary<ArtespActorsEnum, IActorRef> CreateGeradorPassagemAprovadaChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ArtespActorsEnum, IActorRef>
            {
                {
                    ArtespActorsEnum.ProcessadorPassagemAprovadaActor,
                    context.ActorOf(Props.Create(() => new ProcessadorPassagemAprovadaArtespActor()),
                        ActorsPath.ProcessadorPassagemAprovadaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.GeradorPassagemInvalidaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemInvalidaArtespActor()),
                        ActorsPath.GeradorPassagemInvalidaArtespActor.Name)
                }
            };



            return actors;
        }

        public static Dictionary<ArtespActorsEnum, IActorRef> CreateGeradorPassagemReprovadaChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ArtespActorsEnum, IActorRef>
            {
                {
                    ArtespActorsEnum.ProcessadorPassagemReprovadaActor,
                    context.ActorOf(Props.Create(() => new ProcessadorPassagemReprovadaArtespActor()),
                        ActorsPath.ProcessadorPassagemReprovadaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.GeradorPassagemInvalidaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemInvalidaArtespActor()),
                        ActorsPath.GeradorPassagemInvalidaArtespActor.Name)
                }
                //{
                //    ArtespActorsEnum.GeradorPassagemReprovadaActor,
                //    context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaArtespActor()),
                //        ActorsPath.GeradorPassagemReprovadaArtespActor.Name)
                //}
                
            };


            return actors;
        }

        public static Dictionary<ArtespActorsEnum, IActorRef> CreateGeradorPassagemInvalidaChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ArtespActorsEnum, IActorRef>
            {
                {
                    ArtespActorsEnum.ProcessadorPassagemInvalidaActor,
                    context.ActorOf(Props.Create(() => new ProcessadorPassagemInvalidaArtespActor()),
                        ActorsPath.ProcessadorPassagemInvalidaArtespActor.Name)
                }
            };


            return actors;
        }


        public static Dictionary<ArtespActorsEnum, IActorRef> CreateValidadorPassagemPendenteConcessionariaChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ArtespActorsEnum, IActorRef>
            {
                {
                    ArtespActorsEnum.GeradorPassagemReprovadaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaArtespActor()),
                        ActorsPath.GeradorPassagemReprovadaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.GeradorPassagemInvalidaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemInvalidaArtespActor()),
                        ActorsPath.GeradorPassagemInvalidaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.ProcessadorPassagemAprovadaActor,
                    context.ActorOf(Props.Create(() => new ProcessadorPassagemAprovadaArtespActor()),
                        ActorsPath.ProcessadorPassagemAprovadaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.IdentificadorPassagemAceiteManualReenvioActor,
                    context.ActorOf(Props.Create(() => new IdentificadorPassagemAceiteManualReenvioArtespActor()),
                        ActorsPath.IdentificadorPassagemAceiteManualReenvioArtespActor.Name)
                }
            };


            return actors;
        }

        public static Dictionary<ArtespActorsEnum, IActorRef> CreateIdentificadorPassagemAceiteManualReenvioChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ArtespActorsEnum, IActorRef>
            {
                {
                    ArtespActorsEnum.ValidadorPassagemPendenteAceiteManualReenvioActor,
                    context.ActorOf(Props.Create(() => new ValidadorPassagemPendenteAceiteManualReenvioArtespActor()),
                        ActorsPath.ValidadorPassagemPendenteAceiteManualReenvioArtespActor.Name)
                },
                {
                    ArtespActorsEnum.ValidadorPassagemPendenteActor,
                    context.ActorOf(Props.Create(() => new ValidadorPassagemPendenteArtespActor()),
                        ActorsPath.ValidadorPassagemPendenteArtespActor.Name)
                },
                {
                    ArtespActorsEnum.GeradorPassagemInvalidaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemInvalidaArtespActor()),
                        ActorsPath.GeradorPassagemInvalidaArtespActor.Name)
                }
            };


            return actors;
        }

        public static Dictionary<ArtespActorsEnum, IActorRef> CreateValidadorPassagemPendenteAceiteManualReenvioChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ArtespActorsEnum, IActorRef>
            {
                {
                    ArtespActorsEnum.GeradorPassagemReprovadaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaArtespActor()),
                        ActorsPath.GeradorPassagemReprovadaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.GeradorPassagemInvalidaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemInvalidaArtespActor()),
                        ActorsPath.GeradorPassagemInvalidaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.GeradorPassagemActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemArtespActor()),
                        ActorsPath.GeradorPassagemArtespActor.Name)
                }
            };


            return actors;
        }

        public static Dictionary<ArtespActorsEnum, IActorRef> CreateValidadorPassagemAceiteManualReenvioChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ArtespActorsEnum, IActorRef>
            {
                {
                    ArtespActorsEnum.GeradorPassagemReprovadaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaArtespActor()),
                        ActorsPath.GeradorPassagemReprovadaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.GeradorPassagemInvalidaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemInvalidaArtespActor()),
                        ActorsPath.GeradorPassagemInvalidaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.ValidadorPassagemSistemaAceiteManualReenvioActor,
                    context.ActorOf(Props.Create(() => new ValidadorPassagemSistemaAceiteManualReenvioArtespActor()),
                        ActorsPath.ValidadorPassagemSistemaAceiteManualReenvioArtespActor.Name)
                }
            };


            return actors;
        }

        public static Dictionary<ArtespActorsEnum, IActorRef> CreateValidadorPassagemSistemaAceiteManualReenvioChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ArtespActorsEnum, IActorRef>
            {
                {
                    ArtespActorsEnum.GeradorPassagemReprovadaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaArtespActor()),
                        ActorsPath.GeradorPassagemReprovadaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.GeradorPassagemInvalidaActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemInvalidaArtespActor()),
                        ActorsPath.GeradorPassagemInvalidaArtespActor.Name)
                },
                {
                    ArtespActorsEnum.ValidadorPassagemValePedagioActor,
                    context.ActorOf(Props.Create(() => new ValidadorPassagemValePedagioArtespActor()),
                        ActorsPath.ValidadorPassagemValePedagioArtespActor.Name)
                }
            };


            return actors;
        }

        public static IActorRef CreateCoordinator(ActorSystem actorSystem)
        {
            var defaultResizer = new DefaultResizer(
                lower: PoolRouterHelper.Lower,
                upper: PoolRouterHelper.Upper, 
                pressureThreshold: 1, 
                rampupRate: 0.2, 
                backoffThreshold: 0.2,
                backoffRate: 0.1,
                messagesPerResize: PoolRouterHelper.MessagesPerResize);

            var pool = new RoundRobinPool(
                nrOfInstances: PoolRouterHelper.NrOfInstances,
                resizer: defaultResizer);            

            return actorSystem.ActorOf(
                Props.Create(() => new CoordinatorArtespActor())
                .WithRouter(pool), ActorsPath.CoordinatorArtespActor.Name);
        }


    }
}