using Common.Logging;
using ConectCar.Framework.Infrastructure.WebApi.Results;
using ConectCar.Transacoes.Domain.Dto;
using Newtonsoft.Json;
using ProcessadorPassagensProcessadasApi.CommandQuery.Handler;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ProcessadorPassagensProcessadasApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class PassagensArtespController : ApiController
    {
        private PassagemHandler _handler;
        private ResultResponseModelHelper _result;
        private ILog _log = LogManager.GetLogger(typeof(PassagensArtespController));

        public PassagensArtespController()
        {
            _handler = new PassagemHandler();
            _result = new ResultResponseModelHelper(Request);
        }

        [Route("api/Passagens/Artesp/Aprovadas")]
        [HttpPost]
        public IHttpActionResult Aprovadas(List<PassagemAprovadaArtespDto> listRequestPassagensAprovadas)
        {
            var dados = listRequestPassagensAprovadas
                    .GroupBy(c => c.PassagemProcessada.MensagemItemId)
                    .Select(itens => itens.FirstOrDefault())
                    .ToList();

            listRequestPassagensAprovadas = dados;

            if (listRequestPassagensAprovadas!= null && listRequestPassagensAprovadas.Count > 0)
            {
                _log.Debug($"Recebendo {listRequestPassagensAprovadas.Count} passagens aprovadas no protocolo Artesp  para processamento.");
                _log.Trace($"JSON Aprovadas Artesp: {JsonConvert.SerializeObject(listRequestPassagensAprovadas)}");

                var response = _handler.Execute(listRequestPassagensAprovadas);
                return Ok(response);
            }
            
            return Ok();
        }

        [Route("api/Passagens/Artesp/Reprovadas")]
        [HttpPost]
        public IHttpActionResult Reprovadas(List<PassagemReprovadaArtespDto> listRequestPassagensReprovadas)
        {
            var dados = listRequestPassagensReprovadas
                   .GroupBy(c => c.PassagemProcessada.MensagemItemId)
                   .Select(itens => itens.FirstOrDefault())
                   .ToList();

            listRequestPassagensReprovadas = dados;

            if (listRequestPassagensReprovadas != null && listRequestPassagensReprovadas.Count > 0)
            {
                _log.Debug($"Recebendo {listRequestPassagensReprovadas.Count} passagens reprovadas no protocolo Artesp para processamento.");
                _log.Trace($"JSON Reprovadas Artesp: {JsonConvert.SerializeObject(listRequestPassagensReprovadas)}");
                
                var response = _handler.Execute(listRequestPassagensReprovadas);
                return Ok(response);
            }
            return Ok();
        }

        [HttpPost]
        [Route("api/Passagens/Artesp/Invalidas")]
        public IHttpActionResult Invalidas(List<PassagemInvalidaArtespDto> listRequestPassagensInvalidas)
        {
            var dados = listRequestPassagensInvalidas
                   .GroupBy(c => c.MensagemItemId)
                   .Select(itens => itens.FirstOrDefault())
                   .ToList();

            listRequestPassagensInvalidas = dados;

            _log.Debug($"Inválidas: Recebendo {listRequestPassagensInvalidas.Count} mensagens para processamento.");
            // To do Parte do Mensageria
            if (listRequestPassagensInvalidas != null)
            {
                _log.Debug($"  JSON Inválidas: {JsonConvert.SerializeObject(listRequestPassagensInvalidas)}");
                _handler.Execute(listRequestPassagensInvalidas);
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
