using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Enums;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Park.Request
{
    public class ProcessarPassagemReprovadaParkRequest
    {
        public PassagemReprovadaEstacionamento PassagemReprovadaEstacionamento { get; set; }
        
    }
}