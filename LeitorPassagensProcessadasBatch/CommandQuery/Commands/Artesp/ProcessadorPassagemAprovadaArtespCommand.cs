using System;
using ConectCar.Framework.Infrastructure.Cqrs.Rest.Commands;
using ConectCar.Framework.Infrastructure.Data.Rest.DataProviders;
using LeitorPassagensProcessadasBatch.CommandQuery.Handlers.Request.Artesp;
using LeitorPassagensProcessadasBatch.CommandQuery.Resources;
using RestSharp;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Commands.Artesp
{
    public class ProcessadorPassagemAprovadaArtespCommand :
        RestCommandBase<ProcessarAprovadasArtespRequest>
    {
        public ProcessadorPassagemAprovadaArtespCommand(RestDataSource dataSource) : base(dataSource)
        {
        }

        public override void Execute(ProcessarAprovadasArtespRequest args)
        {
            try
            {
                var request = new RestRequest("api/Passagens/Artesp/Aprovadas", Method.POST) { RequestFormat = DataFormat.Json };
                request.AddBody(args.Mensagens);

                //var xJson = Newtonsoft.Json.JsonConvert.SerializeObject(args.Mensagens);

                var response = DataSource.RestClient.Execute(request);

                Log.Info(response.ResponseStatus == ResponseStatus.Completed
                    ? string.Format(LeitorPassagensProcessadasBatchResource.SucessoEnvio, args.Mensagens.Count,
                        args.ConcessionariaId)
                    : string.Format(LeitorPassagensProcessadasBatchResource.ErrorRest, response.ErrorException.Message));
            }
            catch (Exception e)
            {
                Log.Error(string.Format(LeitorPassagensProcessadasBatchResource.ErrorRest, e.Message));
            }
        }
    }
}
