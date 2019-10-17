using ConectCar.Framework.Infrastructure.Cqrs.Rest.Commands;
using ConectCar.Framework.Infrastructure.Data.Rest.DataProviders;
using static LeitorPassagensProcessadasBatch.CommandQuery.Resources.LeitorPassagensProcessadasBatchResource;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Linq;
using LeitorPassagensProcessadasBatch.CommandQuery.Handlers.Request.Park;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Commands.Park
{
    public class ProcessadorPassagemAprovadaParkCommand :
     RestCommandBase<ProcessarAprovadasParkRequest>
    {
        public ProcessadorPassagemAprovadaParkCommand(RestDataSource dataSource) : base(dataSource)
        {
        }

        public override void Execute(ProcessarAprovadasParkRequest args)
        {
            try
            {
                foreach (var passagemAprovadaParkMessage in args.Mensagens.ToList())
                {
                    Log.Info($"Park - RegistroTransacaoId:{passagemAprovadaParkMessage.TransacaoEstacionamento.RegistroTransacaoId} => (aprovado) {JsonConvert.SerializeObject(passagemAprovadaParkMessage)}.");
                }
                var request = new RestRequest("api/Passagens/Park/Aprovadas", Method.POST) { RequestFormat = DataFormat.Json };
                request.AddBody(args.Mensagens);

                var response = DataSource.RestClient.Execute(request);

                Log.Info(response.ResponseStatus == ResponseStatus.Completed
                    ? string.Format(SucessoEnvio, args.Mensagens.Count,
                        args.ConcessionariaId)
                    : string.Format(ErrorRest, response.ErrorException.Message));
            }
            catch (Exception e)
            {
                Log.Error(string.Format(ErrorRest, e.Message));
            }
        }
    }
}
