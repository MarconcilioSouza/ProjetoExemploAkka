using Common.Logging;
using ConectCar.Framework.Infrastructure.WebApi.Results;
using ConectCar.Transacoes.Domain.Dto;
using Newtonsoft.Json;
using ProcessadorPassagensProcessadasApi.CommandQuery.Handler;
using System.Collections.Generic;
using System.Web.Http;

namespace ProcessadorPassagensProcessadasApi.Controllers
{
    public class PassagensParkController : ApiController
    {
        private readonly PassagemParkHandler _handler;
        private readonly ResultResponseModelHelper _result;
        private readonly ILog _log = LogManager.GetLogger(typeof(PassagensParkController));

        public PassagensParkController()
        {
            _handler = new PassagemParkHandler();
            _result = new ResultResponseModelHelper(Request);
        }

        [Route("api/Passagens/Park/Aprovadas")]
        [HttpPost]
        public IHttpActionResult Aprovadas(List<PassagemAprovadaEstacionamentoDto> listRequestPassagensAprovadas)
        {
            if (listRequestPassagensAprovadas != null && listRequestPassagensAprovadas.Count > 0)
            {
                _log.Info($"Recebendo {listRequestPassagensAprovadas.Count} passagens aprovadas no protocolo Park para processamento");
                _log.Info($"JSON Aprovadas Park: {JsonConvert.SerializeObject(listRequestPassagensAprovadas)}");

                var response  = _handler.Execute(listRequestPassagensAprovadas);
                return Ok(response);
            }
            return Ok();
        }

        [Route("api/Passagens/Park/Reprovadas")]
        [HttpPost]

        [HttpGet]
        public IHttpActionResult Reprovadas(List<PassagemReprovadaEstacionamentoDto> listRequestPassagensReprovadas)
        {
            if (listRequestPassagensReprovadas != null && listRequestPassagensReprovadas.Count > 0)
            {
                _log.Info($"Recebendo {listRequestPassagensReprovadas.Count} passagens reprovadas no protocolo Park para processamento.");
                _log.Info($"JSON Reprovadas Park: {JsonConvert.SerializeObject(listRequestPassagensReprovadas)}");

                var response = _handler.Execute(listRequestPassagensReprovadas);
                return Ok(response);
            }
            return Ok();
        }
        
        [HttpGet]
        public IHttpActionResult Get()
        {
            return _result.Ok(true, "Processador de passagens processadas.");
        }
    }
}
