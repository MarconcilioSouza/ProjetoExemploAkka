using System;
using ConectCar.Framework.Infrastructure.Cqrs.Rest.Commands;
using ConectCar.Framework.Infrastructure.Data.Rest.DataProviders;
using LeitorPassagensPendentesBatch.CommandQuery.Handlers.Request;
using LeitorPassagensPendentesBatch.CommandQuery.Resources;
using RestSharp;

namespace LeitorPassagensPendentesBatch.CommandQuery.Commands
{
    public class EnviarPassagemEdiParaAkkaCommand : RestCommandBase<EnviarPassagensEdiFilter>
    {
        public EnviarPassagemEdiParaAkkaCommand(RestDataSource dataSource) : base(dataSource)
        {
        }

        public override void Execute(EnviarPassagensEdiFilter args)
        {
            try
            {
                RestRequest request = new RestRequest("api/ProcessadorPassagens/Edi", Method.POST) { RequestFormat = DataFormat.Json };
                request.AddBody(args.Passagens);

                var xJson = Newtonsoft.Json.JsonConvert.SerializeObject(args.Passagens);

                var response = DataSource.RestClient.Execute(request);
                if (response.ResponseStatus == ResponseStatus.Completed)
                    Log.Info(string.Format(LeitorPassagensPendentesBatchResource.SucessoEnvio, args.Passagens.Count));
                else
                {
                    string erroMessage = response.ErrorException != null ? response.ErrorException.Message :
                        response.ErrorMessage;

                    Log.Info(string.Format(LeitorPassagensPendentesBatchResource.ErrorRest,  erroMessage));
                }
            }
            catch (Exception e)
            {
                Log.Error(string.Format(LeitorPassagensPendentesBatchResource.ErrorRest, e.Message));
            }
        }
    }
}