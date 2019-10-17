using ConectCar.Framework.Infrastructure.Cqrs.Rest.Commands;
using ConectCar.Framework.Infrastructure.Data.Rest.DataProviders;
using LeitorPassagensProcessadasBatch.CommandQuery.Handlers.Request.Park;
using RestSharp;
using System;
using System.Linq;
using static LeitorPassagensProcessadasBatch.CommandQuery.Resources.LeitorPassagensProcessadasBatchResource;


namespace LeitorPassagensProcessadasBatch.CommandQuery.Commands.Park
{
    public class ProcessadorPassagemReprovadaParkCommand :
     RestCommandBase<ProcessarReprovadasParkRequest>
    {
        public ProcessadorPassagemReprovadaParkCommand(RestDataSource dataSource) : base(dataSource)
        {
        }

        public override void Execute(ProcessarReprovadasParkRequest args)
        {
            try
            {
                foreach (var passagemReprovadaParkMessage in args.Mensagens.ToList())
                {
                    Log.Info($"Park - RegistroTransacaoId:{passagemReprovadaParkMessage.TransacaoEstacionamentoRecusada.RegistroTransacaoId} => (reprovado) Enviado para API de persistência.");
                }
                var request = new RestRequest("api/Passagens/Park/Reprovadas", Method.POST) { RequestFormat = DataFormat.Json };
                request.AddBody(args.Mensagens);

                request.AddBody(args.Mensagens);
                var response = DataSource.RestClient.Execute(request);

                Log.Info(response.ResponseStatus == ResponseStatus.Completed
                    ? string.Format(SucessoEnvio, args.Mensagens.Count)
                    : string.Format(ErrorRest, response.ErrorException.Message));
            }
            catch (Exception e)
            {
                Log.Error(string.Format(ErrorRest, e.Message));
            }
        }
    }
}
