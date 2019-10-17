using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Framework.Infrastructure.Log;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Responses;
using ProcessadorPassagensActors.CommandQuery.Validators.Validator;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Edi
{
    public class IdentificadorPassagemDuplicadaEdiHandler : Loggable, 
        ICommand<IdentificadorPassagemDuplicadaEdiRequest, IdentificadorPassagemDuplicadaEdiResponse>
    {
        public IdentificadorPassagemDuplicadaEdiResponse Execute(IdentificadorPassagemDuplicadaEdiRequest request)
        {
            Log.Info($"Passagem DetalheTrnId: {request.PassagemPendenteEdi.DetalheTrnId} Data Passagem {request.PassagemPendenteEdi.DataPassagem.ToStringPtBr()}");
            Log.Info($"Passagem DetalheTrnId: {request.PassagemPendenteEdi.DetalheTrnId} - Fluxo: IdentificadorPassagemDuplicadaEdiHandler | Validar DetalheRepetido");
            
            return new IdentificadorPassagemDuplicadaEdiResponse { PassagemDuplicada = request.PassagemPendenteEdi.DetalheRepetido };
        }
    }
}
