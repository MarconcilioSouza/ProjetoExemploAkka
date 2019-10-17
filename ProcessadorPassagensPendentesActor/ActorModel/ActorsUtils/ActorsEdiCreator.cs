using System.Collections.Generic;
using Akka.Actor;
using ProcessadorPassagensActors.Enums;
using Akka.Routing;
using ProcessadorPassagensActors.Actors.Artesp;
using ProcessadorPassagensActors.Actors.Edi;

namespace ProcessadorPassagensActors.ActorsUtils
{
    public static class ActorsEdiCreator
    {
        public static Dictionary<EdiActorsEnum, IActorRef> CreateCoordinatorEdiChildrenActors(
            IUntypedActorContext context)
        {
            var actors = new Dictionary<EdiActorsEnum, IActorRef>
            {
                {
                    EdiActorsEnum.IdentificadorPassagemDuplicadaEdiActor,
                    context.ActorOf(Props.Create(() => new IdentificadorPassagemDuplicadaEdiActor()),
                        ActorsPath.IdentificadorPassagemDuplicadaEdiActor.Name)
                }
            };

            return actors;
        }
        public static IActorRef CreateCoordinator(ActorSystem actorSystem)
        {
            return actorSystem.ActorOf(Props.Create(() => new CoordinatorEdiActor()), ActorsPath.CoordinatorArtespActor.Name);
        }


        public static Dictionary<EdiActorsEnum, IActorRef> CreateIdentificadorPassagemChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<EdiActorsEnum, IActorRef>
            {
                {
                    EdiActorsEnum.GeradorPassagemReprovadaEdiActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaEdiActor()),
                        ActorsPath.GeradorPassagemReprovadaEdiActor.Name)
                },
                {
                    EdiActorsEnum.ValidadorPassagemPendenteEdiActor,
                    context.ActorOf(Props.Create(() => new ValidadorPassagemPendenteEdiActor()),
                        ActorsPath.ValidadorPassagemPendenteEdiActor.Name)
                }
            };
            return actors;
        }
        public static Dictionary<EdiActorsEnum, IActorRef> CreateValidadorPassagemPendenteEdiChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<EdiActorsEnum, IActorRef>
            {
                {
                    EdiActorsEnum.GeradorPassagemReprovadaEdiActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaEdiActor()),
                        ActorsPath.GeradorPassagemReprovadaEdiActor.Name)
                },
                {
                    EdiActorsEnum.GeradorPassagemEdiActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemEdiActor()),
                        ActorsPath.GeradorPassagemEdiActor.Name)
                }
            };
            return actors;
        }

        public static Dictionary<EdiActorsEnum, IActorRef> CreateGeradorPassagemEdiChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<EdiActorsEnum, IActorRef>
            {
                {
                    EdiActorsEnum.GeradorPassagemReprovadaEdiActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaEdiActor()),
                        ActorsPath.GeradorPassagemReprovadaEdiActor.Name)
                },
                {
                    EdiActorsEnum.ValidadorPassagemEdiActor,
                    context.ActorOf(Props.Create(() => new ValidadorPassagemEdiActor()),
                        ActorsPath.ValidadorPassagemEdiActor.Name)
                }
            };
            return actors;
        }

        public static Dictionary<EdiActorsEnum, IActorRef> CreateValidadorPassagemEdiChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<EdiActorsEnum, IActorRef>
            {
                {
                    EdiActorsEnum.GeradorPassagemReprovadaEdiActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaEdiActor()),
                        ActorsPath.GeradorPassagemReprovadaEdiActor.Name)
                },
                {
                    EdiActorsEnum.ValidadorPassagemSistemaEdiActor,
                    context.ActorOf(Props.Create(() => new ValidadorPassagemSistemaEdiActor()),
                        ActorsPath.ValidadorPassagemSistemaEdiActor.Name)
                }
            };
            return actors;
        }

        public static Dictionary<EdiActorsEnum, IActorRef> CreateValidadorPassagemSistemaEdiChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<EdiActorsEnum, IActorRef>
            {
                {
                    EdiActorsEnum.GeradorPassagemReprovadaEdiActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaEdiActor()),
                        ActorsPath.GeradorPassagemReprovadaEdiActor.Name)
                },
                {
                    EdiActorsEnum.ValidadorSlaListaNelaEdiActor,
                    context.ActorOf(Props.Create(() => new ValidadorSlaListaNelaEdiActor()),
                        ActorsPath.ValidadorSlaListaNelaEdiActor.Name)
                }
            };
            return actors;
        }

        public static Dictionary<EdiActorsEnum, IActorRef> CreateValidadorSlaListaNelaEdiChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<EdiActorsEnum, IActorRef>
            {
                {
                    EdiActorsEnum.GeradorPassagemReprovadaEdiActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaEdiActor()),
                        ActorsPath.GeradorPassagemReprovadaEdiActor.Name)
                },
                {
                    EdiActorsEnum.ValidadorDivergenciaCategoriaEdiActor,
                    context.ActorOf(Props.Create(() => new ValidadorDivergenciaCategoriaEdiActor()),
                        ActorsPath.ValidadorDivergenciaCategoriaEdiActor.Name)
                }
            };
            return actors;
        }

        public static Dictionary<EdiActorsEnum, IActorRef> CreateValidadorDivergenciaCategoriaEdiChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<EdiActorsEnum, IActorRef>
            {
                {
                    EdiActorsEnum.GeradorPassagemReprovadaEdiActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaEdiActor()),
                        ActorsPath.GeradorPassagemReprovadaEdiActor.Name)
                },
                {
                    EdiActorsEnum.ValidadorPassagemValePedagioEdiActor,
                    context.ActorOf(Props.Create(() => new ValidadorPassagemValePedagioEdiActor()),
                        ActorsPath.ValidadorPassagemValePedagioEdiActor.Name)
                }
            };
            return actors;
        }

        public static Dictionary<EdiActorsEnum, IActorRef> CreateValidadorPassagemValePedadgioEdiChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<EdiActorsEnum, IActorRef>
            {
                {
                    EdiActorsEnum.GeradorPassagemReprovadaEdiActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaEdiActor()),
                        ActorsPath.GeradorPassagemReprovadaEdiActor.Name)
                },
                {
                    EdiActorsEnum.GeradorPassagemAprovadaEdiActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemAprovadaEdiActor()),
                        ActorsPath.GeradorPassagemAprovadaEdiActor.Name)
                }
            };
            return actors;
        }

        public static Dictionary<EdiActorsEnum, IActorRef> CreateGeradorPassagemAprovadaEdiChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<EdiActorsEnum, IActorRef>
            {
                {
                    EdiActorsEnum.GeradorPassagemReprovadaEdiActor,
                    context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaEdiActor()),
                        ActorsPath.GeradorPassagemReprovadaEdiActor.Name)
                },
                {
                    EdiActorsEnum.ProcessadorPassagemAprovadaEdiActor,
                    context.ActorOf(Props.Create(() => new ProcessadorPassagemAprovadaEdiActor()),
                        ActorsPath.ProcessadorPassagemAprovadaEdiActor.Name)
                }
            };
            return actors;
        }

        public static Dictionary<EdiActorsEnum, IActorRef> CreateGeradorPassagemReprovadaEdiChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<EdiActorsEnum, IActorRef>
            {
                {
                    EdiActorsEnum.ProcessadorPassagemReprovadaEdiActor,
                    context.ActorOf(Props.Create(() => new ProcessadorPassagemReprovadaEdiActor()),
                        ActorsPath.ProcessadorPassagemReprovadaEdiActor.Name)
                }
            };
            return actors;
        }
    }
}