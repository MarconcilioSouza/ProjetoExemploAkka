using System.Collections.Generic;
using Akka.Actor;

namespace ProcessadorPassagensActors.ActorsUtils
{
    public static class ActorsCommonUtil
    {
        public static void CreateIfNotExists<TActor>(this Dictionary<string, IActorRef> actors, IUntypedActorContext context, string key)
            where TActor : ActorBase, new()
        {
            if (!actors.ContainsKey(key))
            {
                IActorRef actorRef = context.ActorOf(Props.Create(() => new TActor()), key);
                actors.Add(key, actorRef);
            }            
        }

        public static void CreateIfNotExists<TActor>(this Dictionary<string, IActorRef> actors, ActorSystem actorSystem, string key)
            where TActor : ActorBase, new()
        {
            if (!actors.ContainsKey(key))
            {
                IActorRef actorRef = actorSystem.ActorOf(Props.Create(() => new TActor()), key);
                actors.Add(key, actorRef);
            }
        }


    }
}