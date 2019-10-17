using AutoMapper;
using ConectCar.Framework.Infrastructure.WebApi.Controllers;
using ConectCar.Framework.Infrastructure.WebApi.Results;
using ConectCar.Transacoes.Domain.Dto;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.Actors.Artesp;
using ProcessadorPassagensActors.Actors.Edi;
using ProcessadorPassagensActors.ActorsMessages.Artesp;
using ProcessadorPassagensActors.ActorsMessages.Edi;
using Newtonsoft.Json;
using ProcessadorPassagensActors.Actors.Park;
using ProcessadorPassagensActors.ActorsMessages.Park;
using ProcessadorPassagensActors.CommandQuery;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;

namespace ProcessadorPassagensActors.Controllers
{
    public class ProcessadorPassagensController : ApiControllerBase
    {
        private readonly ResultResponseModelHelper _result;

        public ProcessadorPassagensController()
        {
            _result = new ResultResponseModelHelper(Request);
        }

        [ActionName("Artesp")]
        [HttpPost]
        public IHttpActionResult Artesp(List<PassagemPendenteArtespDto> request)
        {
            var passagensPendentesArtesp = Mapper.Map(request, new List<PassagemPendenteArtesp>());

            Log.Debug($"Recebendo {passagensPendentesArtesp.Count} mensagens...");

            foreach (var passagemPendenteArtesp in passagensPendentesArtesp)
            {
                var mensagem = new CoordinatorArtespMessage
                {
                    FluxoExecucao = Enums.ArtespActorsEnum.CoordinatorActor,
                    PassagemPendenteArtesp = passagemPendenteArtesp
                };

                //Enviando mensagem para o coordenador...
                Log.Info($"Passagem ID: {mensagem.PassagemPendenteArtesp.MensagemItemId} | Inicio Processamento.");
                TransacaoArtespActorSystem.Processar(mensagem);
            }


            return _result.Ok(true, "Processo iniciado com sucesso");
        }

        [ActionName("Edi")]
        [HttpPost]
        public IHttpActionResult Edi(List<PassagemPendenteEdiDto> request)
        {
            var passagensPendentesEdi = Mapper.Map(request, new List<PassagemPendenteEDI>());

            var mensagem = new CoordinatorEdiMessage
            {
                FluxoExecucao = Enums.EdiActorsEnum.CoordinatorEdiActor,
                PassagensPendentesEdi = passagensPendentesEdi
            };

            TransacaoEdiActorSystem.Processar(mensagem);
            return _result.Ok(true, "Processo iniciado com sucesso");
        }

        [ActionName("Park")]
        [HttpPost]
        public IHttpActionResult Park(List<PassagemPendenteEstacionamentoDto> request)
        {
            var passagensPendentesEstacionamento = new List<PassagemPendenteEstacionamento>();
            foreach (var c in request)
            {
                var p = new PassagemPendenteEstacionamento
                {
                    RegistroTransacaoId =  c.RegistroTransacaoId.TryToLong(),
                    Conveniado = new Conveniado
                    {
                        CodigoProtocolo = c.CodigoConveniado.TryToInt()
                    },
                    Adesao = new Adesao
                    {
                        Tag = new Tag
                        {
                            OBUId = c.Tag.TryToLong()
                        }
                    },
                    Praca = new Praca
                    {
                        CodigoPraca = c.Praca.TryToInt()
                    },
                    Pista = new Pista
                    {
                        CodigoPista = c.Pista.TryToInt()
                    },
                    DataPassagem = c.DataHoraTransacao,
                    DataHoraEntrada = c.DataHoraEntrada,
                    Valor = c.ValorCobrado,
                    ValorDesconto = c.ValorDesconto,
                    MotivoDesconto = c.MotivoDesconto,
                    TempoPermanencia = c.TempoPermanencia,
                    MotivoAtrasoTransmissao = (MotivoAtrasoTransmissaoEstacionamento) c.MotivoAtrasoTransmissao,
                    TipoTransacaoEstacionamento = (TipoTransacaoEstacionamento) (c.TipoTransacao ?? 0),
                    Ticket = c.Ticket,
                    Mensalista = c.Mensalista,
                    Tag = new Tag
                    {
                        OBUId = c.Tag.TryToLong()
                    },
                    Detalhes = new List<DetalhePassagemPendenteEstacionamento>()
                };
                if (c.Detalhes != null && c.Detalhes.Any())
                {
                    c.Detalhes.ForEach((d) =>
                    {
                        p.Detalhes.Add(new DetalhePassagemPendenteEstacionamento
                        {
                            CodigoPraca = d.CodigoPraca,
                            CodigoPista = d.CodigoPista,
                            Data = d.Data
                        });
                    });
                }

                passagensPendentesEstacionamento.Add(p);
            }

            var mensagem = new CoordinatorParkMessage
            {
                FluxoExecucao = Enums.ParkActorsEnum.CoordinatorParkActor,
                PassagensPendentesEstacionamentos = passagensPendentesEstacionamento
            };

            TransacaoParkActorSystem.Processar(mensagem);
            return _result.Ok(true, "Processo iniciado com sucesso");
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            return _result.Ok(true, "Processador de passagens pendentes.");
        }
    }
}
