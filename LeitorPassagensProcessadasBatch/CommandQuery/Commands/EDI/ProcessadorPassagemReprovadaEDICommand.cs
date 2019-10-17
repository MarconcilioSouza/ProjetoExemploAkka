using ConectCar.Framework.Infrastructure.Cqrs.Rest.Commands;
using ConectCar.Framework.Infrastructure.Data.Rest.DataProviders;
using static LeitorPassagensProcessadasBatch.CommandQuery.Resources.LeitorPassagensProcessadasBatchResource;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeitorPassagensProcessadasBatch.CommandQuery.Handlers.Request.EDI;
using Newtonsoft.Json;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Commands.EDI
{
    public class ProcessadorPassagemReprovadaEdiCommand :
     RestCommandBase<ProcessarReprovadasEdiRequest>
    {
        public ProcessadorPassagemReprovadaEdiCommand(RestDataSource dataSource) : base(dataSource)
        {
        }

        public override void Execute(ProcessarReprovadasEdiRequest args)
        {
            try
            {
                var request = new RestRequest("api/Passagens/EDI/Reprovadas", Method.POST) { RequestFormat = DataFormat.Json };
                request.AddBody(args.Mensagens);

                var response = DataSource.RestClient.Execute(request);

                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    Log.Info(string.Format(SucessoEnvio, args.Mensagens.Count));
                }
                else
                {
                    var erroMensage = response.ErrorException?.Message ?? response.ErrorMessage;
                    Log.Info(string.Format(ErrorRest, erroMensage));
                }
                
            }
            catch (Exception e)
            {
                Log.Error(string.Format(ErrorRest, e.Message));
            }
        }
    }
}
