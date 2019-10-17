using System;
using ConectCar.Framework.Infrastructure.Cqrs.Rest.Commands;
using ConectCar.Framework.Infrastructure.Data.Rest.DataProviders;
using LeitorPassagensPendentesBatch.CommandQuery.Handlers.Request;
using LeitorPassagensPendentesBatch.CommandQuery.Resources;
using RestSharp;

namespace LeitorPassagensPendentesBatch.CommandQuery.Commands
{
    public class EnviarPassagemParkParaAkkaCommand : RestCommandBase<EnviarPassagensParkFilter>
    {
        public EnviarPassagemParkParaAkkaCommand(RestDataSource dataSource) : base(dataSource)
        {
        }

        public override void Execute(EnviarPassagensParkFilter args)
        {
            try
            {
                RestRequest request = new RestRequest("api/ProcessadorPassagens/Park", Method.POST) { RequestFormat = DataFormat.Json };
                request.AddBody(args.Passagens);

                var xJson = Newtonsoft.Json.JsonConvert.SerializeObject(args.Passagens);

                var response = DataSource.RestClient.Execute(request);

                Log.Info(response.ResponseStatus == ResponseStatus.Completed
                    ? string.Format(LeitorPassagensPendentesBatchResource.SucessoEnvio, args.Passagens.Count)
                    : string.Format(LeitorPassagensPendentesBatchResource.ErrorRest, response.ErrorException.Message));
            }
            catch (Exception e)
            {
                Log.Error(string.Format(LeitorPassagensPendentesBatchResource.ErrorRest, e.Message));
            }
        }
    }
}