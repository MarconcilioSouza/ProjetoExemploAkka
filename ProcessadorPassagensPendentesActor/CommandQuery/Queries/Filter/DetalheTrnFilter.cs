using System;

namespace ProcessadorPassagensActors.CommandQuery.Queries.Filter
{
    public class DetalheTrnFilter
    {
        public DateTime Data { get; set; }
        public long OBUId { get; set; }
        public int? CodigoPraca { get; set; }
    }
}
