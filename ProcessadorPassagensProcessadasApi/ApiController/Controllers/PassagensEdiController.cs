using System;
using ConectCar.Framework.Infrastructure.WebApi.Results;
using ConectCar.Transacoes.Domain.Dto;
using Newtonsoft.Json;
using ProcessadorPassagensProcessadasApi.CommandQuery.Handler;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ConectCar.Framework.Infrastructure.WebApi.Controllers;
using Common.Logging;

namespace ProcessadorPassagensProcessadasApi.Controllers
{
    public class PassagensEdiController : ApiControllerBase
    {
        private readonly PassagemEdiHandler _handler;
        private readonly ResultResponseModelHelper _result;
        private readonly ILog _log = LogManager.GetLogger(typeof(PassagensEdiController));

        public PassagensEdiController()
        {
            _handler = new PassagemEdiHandler();
            _result = new ResultResponseModelHelper(Request);
        }

        [Route("api/Passagens/Edi/Aprovadas")]
        [HttpPost]
        public IHttpActionResult Aprovadas(List<PassagemAprovadaEDIDto> listRequestPassagensAprovadas)
        {
            if (listRequestPassagensAprovadas != null && listRequestPassagensAprovadas.Count > 0)
            {
                _log.Debug($"Recebendo {listRequestPassagensAprovadas.Count} passagens aprovadas no protocolo EDI  para processamento.");
                _log.Trace($"JSON Aprovadas Edi: {JsonConvert.SerializeObject(listRequestPassagensAprovadas)}");
                var response = _handler.Execute(listRequestPassagensAprovadas);
                return Ok(response);
            }

            return Ok();
        }

        [Route("api/Passagens/Edi/Reprovadas")]
        [HttpPost]
        public IHttpActionResult Reprovadas(List<PassagemReprovadaEdiDto> listRequestPassagensReprovadas)
        {
            if (listRequestPassagensReprovadas != null && listRequestPassagensReprovadas.Count > 0)
            {
                _log.Debug($"Recebendo {listRequestPassagensReprovadas.Count} passagens reprovadas no protocolo EDI para processamento.");
                _log.Trace($"JSON Reprovadas EDI: {JsonConvert.SerializeObject(listRequestPassagensReprovadas)}");

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