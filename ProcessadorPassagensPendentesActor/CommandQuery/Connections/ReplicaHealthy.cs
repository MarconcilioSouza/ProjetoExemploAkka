using System;

namespace ProcessadorPassagensActors.CommandQuery.Connections
{
    public class ReplicaHealthy
    {

        public string ReplicaServerName { get; set; }
        public string DatabaseName { get; set; }
        public DateTime LastCommitTime { get; set; }
        public DateTime ReplicaCommitTime { get; set; }
        public int LagSeconds { get; set; }

    }
}
