using System;
using ConectCar.Framework.Infrastructure.Cqrs.Rest.Commands;
using ConectCar.Framework.Infrastructure.Data.Rest.DataProviders;
using LeitorPassagensPendentesBatch.CommandQuery.Handlers.Request;
using LeitorPassagensPendentesBatch.CommandQuery.Resources;
using RestSharp;

namespace LeitorPassagensPendentesBatch.CommandQuery.Commands
{
    public class EnviarPassagemParaAkkaCommand : RestCommandBase<EnviarPassagensRequest, bool>
    {
        public EnviarPassagemParaAkkaCommand(RestDataSource dataSource) : base(dataSource)
        {
        }

        public override bool Execute(EnviarPassagensRequest args)
        {
            try
            {                
                var request = new RestRequest("api/ProcessadorPassagens/Artesp", Method.POST) { RequestFormat = DataFormat.Json };
                request.AddBody(args.Passagens);
                var response = DataSource.RestClient.Execute(request);

                if(response.ResponseStatus == ResponseStatus.Completed)
                {
                    Log.Debug(string.Format(LeitorPassagensPendentesBatchResource.SucessoEnvio, args.Passagens.Count,
                        args.CodigoProtocoloArtesp));

                    return true;
                }
                else
                {
                    Log.Error(string.Format(LeitorPassagensPendentesBatchResource.ErrorRest, response.ErrorException.Message));
                    return false;
                }
               
            }
            catch (Exception e)
            {
                Log.Error(string.Format(LeitorPassagensPendentesBatchResource.ErrorRest, e.Message));
            }

            return false;

        }
    }
}
