using ConectCar.Framework.Infrastructure.Cqrs.Rest.Commands;
using ConectCar.Framework.Infrastructure.Data.Rest.DataProviders;
using LeitorPassagensProcessadasBatch.CommandQuery.Handlers.Request.EDI;
using Newtonsoft.Json;
using RestSharp;
using System;
using static LeitorPassagensProcessadasBatch.CommandQuery.Resources.LeitorPassagensProcessadasBatchResource;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Commands.EDI
{
    public class ProcessadorPassagemAprovadaEdiCommand :
     RestCommandBase<ProcessarAprovadasEdiRequest>
    {
        public ProcessadorPassagemAprovadaEdiCommand(RestDataSource dataSource) : base(dataSource)
        {
        }

        public override void Execute(ProcessarAprovadasEdiRequest args)
        {
            try
            {
                var request = new RestRequest("api/Passagens/EDI/Aprovadas", Method.POST) { RequestFormat = DataFormat.Json };
                request.AddBody(args.Mensagens);

               var response = DataSource.RestClient.Execute(request);
                if (response.ResponseStatus == ResponseStatus.Completed)
                    Log.Info(string.Format(SucessoEnvio, args.Mensagens.Count));
                else
                {
                    var erroMessage = response.ErrorException?.Message ?? response.ErrorMessage;
                    Log.Info(string.Format(ErrorRest, erroMessage));
                }
            }
            catch (Exception e)
            {
                Log.Error(string.Format(ErrorRest, e.Message));
            }
        }
    }
}
