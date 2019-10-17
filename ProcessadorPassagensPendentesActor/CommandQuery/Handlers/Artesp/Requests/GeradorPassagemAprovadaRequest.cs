using System.Collections.Generic;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests
{
   public class GeradorPassagemAprovadaRequest
    {
       /// <summary>
       /// passagem
       /// </summary>
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }
        public List<DetalheViagem> ViagensAgendadas { get; set; }

    }
}
