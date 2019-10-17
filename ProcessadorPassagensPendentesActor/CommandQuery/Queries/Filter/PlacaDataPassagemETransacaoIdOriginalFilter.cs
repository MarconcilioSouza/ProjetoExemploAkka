using System;

namespace ProcessadorPassagensActors.CommandQuery.Queries.Filter
{
    public class PlacaDataPassagemETransacaoIdOriginalFilter
    {
        public string Placa { get; set; }
        public DateTime DataPassagem { get; set; }
        public long TransacaoIdOriginal { get; set; }
    }
}
