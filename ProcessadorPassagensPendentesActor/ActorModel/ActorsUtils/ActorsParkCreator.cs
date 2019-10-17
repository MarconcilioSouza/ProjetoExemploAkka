using Akka.Actor;
using ProcessadorPassagensActors.Actors.Park;
using ProcessadorPassagensActors.Enums;
using System.Collections.Generic;

namespace ProcessadorPassagensActors.ActorsUtils
{
    public static class ActorsParkCreator
    {
        public static Dictionary<ParkActorsEnum, IActorRef> CreateCoordinatorParkChildrenActors(
            IUntypedActorContext context)
        {
            var actors = new Dictionary<ParkActorsEnum, IActorRef>
            {
                {
                    ParkActorsEnum.ValidarPassagemPendenteParkActor,
                    context.ActorOf(Props.Create(() => new ValidarPassagemPendenteParkActor()),
                        ActorsPath.ValidarPassagemPendenteParkActor.Name)
                }
            };

            return actors;
        }

        public static IActorRef CreateCoordinator(ActorSystem actorSystem)
        {
            return actorSystem.ActorOf(Props.Create(() => new CoordinatorParkActor()), ActorsPath.CoordinatorParkActor.Name);
        }

        public static Dictionary<ParkActorsEnum, IActorRef> CreateValidarPassagemPendenteParkActorChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ParkActorsEnum, IActorRef>
            {
                {
                    ParkActorsEnum.GerarPassagemParkActor,
                    context.ActorOf(Props.Create(() => new GerarPassagemParkActor()),
                        ActorsPath.GerarPassagemParkActor.Name)
                },
                {
                    ParkActorsEnum.GerarPassagemReprovadaParkActor,
                    context.ActorOf(Props.Create(() => new GerarPassagemReprovadaParkActor()),
                        ActorsPath.GerarPassagemReprovadaParkActor.Name)
                }
            };
            return actors;
        }

        public static Dictionary<ParkActorsEnum, IActorRef> CreateGerarPassagemPendenteParkChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ParkActorsEnum, IActorRef>
            {
                {
                    ParkActorsEnum.ValidarPassagemParkActor,
                    context.ActorOf(Props.Create(() => new ValidarPassagemParkActor()),
                        ActorsPath.ValidarPassagemParkActor.Name)
                },
                {
                    ParkActorsEnum.GerarPassagemReprovadaParkActor,
                    context.ActorOf(Props.Create(() => new GerarPassagemReprovadaParkActor()),
                        ActorsPath.GerarPassagemReprovadaParkActor.Name)
                }
            };
            return actors;
        }

        public static Dictionary<ParkActorsEnum, IActorRef> CreateGerarPassagemParkChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ParkActorsEnum, IActorRef>
            {
                {
                    ParkActorsEnum.ValidarPassagemParkActor,
                    context.ActorOf(Props.Create(() => new ValidarPassagemParkActor()),
                        ActorsPath.ValidarPassagemParkActor.Name)
                },
                {
                    ParkActorsEnum.GerarPassagemReprovadaParkActor,
                    context.ActorOf(Props.Create(() => new GerarPassagemReprovadaParkActor()),
                        ActorsPath.GerarPassagemReprovadaParkActor.Name)
                }
            };
            return actors;
        }

        public static Dictionary<ParkActorsEnum, IActorRef> CreateValidarPassagemParkActorChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ParkActorsEnum, IActorRef>
            {
                {
                    ParkActorsEnum.ValidarPassagemSistemaParkActor,
                    context.ActorOf(Props.Create(() => new ValidarPassagemSistemaParkActor()),
                        ActorsPath.ValidarPassagemSistemaParkActor.Name)
                },
                {
                    ParkActorsEnum.GerarPassagemReprovadaParkActor,
                    context.ActorOf(Props.Create(() => new GerarPassagemReprovadaParkActor()),
                        ActorsPath.GerarPassagemReprovadaParkActor.Name)
                }
            };
            return actors;
        }

        public static Dictionary<ParkActorsEnum, IActorRef> CreateValidarPassagemSistemaParkActorChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ParkActorsEnum, IActorRef>
            {
                {
                    ParkActorsEnum.GerarPassagemAprovadaParkActor,
                    context.ActorOf(Props.Create(() => new GerarPassagemAprovadaParkActor()),
                        ActorsPath.GerarPassagemAprovadaParkActor.Name)
                },
                {
                    ParkActorsEnum.GerarPassagemReprovadaParkActor,
                    context.ActorOf(Props.Create(() => new GerarPassagemReprovadaParkActor()),
                        ActorsPath.GerarPassagemReprovadaParkActor.Name)
                }
            };
            return actors;
        }

        public static Dictionary<ParkActorsEnum, IActorRef> CreateGerarPassagemAprovadaParkActorChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ParkActorsEnum, IActorRef>
            {
                {
                    ParkActorsEnum.ProcessarPassagemAprovadaParkActor,
                    context.ActorOf(Props.Create(() => new ProcessarPassagemAprovadaParkActor()),
                        ActorsPath.ProcessarPassagemAprovadaParkActor.Name)
                },
                {
                    ParkActorsEnum.GerarPassagemReprovadaParkActor,
                    context.ActorOf(Props.Create(() => new GerarPassagemReprovadaParkActor()),
                        ActorsPath.GerarPassagemReprovadaParkActor.Name)
                }
            };
            return actors;
        }

        public static Dictionary<ParkActorsEnum, IActorRef> CreateGerarPassagemReprovadaParkActorChildrenActors(IUntypedActorContext context)
        {
            var actors = new Dictionary<ParkActorsEnum, IActorRef>
            {
                {
                    ParkActorsEnum.ProcessarPassagemReprovadaParkActor,
                    context.ActorOf(Props.Create(() => new ProcessarPassagemReprovadaParkActor()),
                        ActorsPath.ProcessarPassagemReprovadaParkActor.Name)
                }
            };
            return actors;
        }        
    }
}