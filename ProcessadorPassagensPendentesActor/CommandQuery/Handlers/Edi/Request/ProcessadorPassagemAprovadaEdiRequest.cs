using System.Collections.Generic;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request
{
    public class ProcessadorPassagemAprovadaEdiRequest
    {
        public PassagemAprovadaEDI PassagemAprovadaEdi { get; set; }
        public List<DetalheViagem> DetalheViagens { get; set; }
        public DetalheTrfRecusado DetalheTrfRecusado { get; set; }
        public Evento Evento { get; set; }
    }
}
