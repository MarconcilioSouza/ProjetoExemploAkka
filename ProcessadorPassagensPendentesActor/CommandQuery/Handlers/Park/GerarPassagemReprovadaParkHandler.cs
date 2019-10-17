using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Response;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Park
{
    public class GerarPassagemReprovadaParkHandler : Loggable, ICommand<GerarPassagemReprovadaParkRequest, GerarPassagemReprovadaParkResponse>
    {
        public GerarPassagemReprovadaParkResponse Execute(GerarPassagemReprovadaParkRequest _request)
        {

            var _passagemReprovada = new PassagemReprovadaEstacionamento();
            _passagemReprovada.TransacaoEstacionamentoRecusada = new TransacaoEstacionamentoRecusada();
            _passagemReprovada.TransacaoEstacionamentoRecusada.Mensalista = _request.PassagemPendenteEstacionamento.Mensalista;
            _passagemReprovada.TransacaoEstacionamentoRecusada.Ticket = _request.PassagemPendenteEstacionamento.Ticket;
            _passagemReprovada.TransacaoEstacionamentoRecusada.RegistroTransacaoId = _request.PassagemPendenteEstacionamento.RegistroTransacaoId;
            _passagemReprovada.TransacaoEstacionamentoRecusada.TipoTransacaoEstacionamentoId = (int)_request.PassagemPendenteEstacionamento.TipoTransacaoEstacionamento;
            _passagemReprovada.TransacaoEstacionamentoRecusada.MotivoAtrasoTransamissaoId = (int)_request.PassagemPendenteEstacionamento.MotivoAtrasoTransmissao;
            _passagemReprovada.TransacaoEstacionamentoRecusada.TempoPermamente = _request.PassagemPendenteEstacionamento.TempoPermanencia;
            _passagemReprovada.TransacaoEstacionamentoRecusada.MotivoRecusaId = (int)_request.Erro;
            _passagemReprovada.TransacaoEstacionamentoRecusada.MotivoDesconto = _request.PassagemPendenteEstacionamento.MotivoDesconto;
            _passagemReprovada.TransacaoEstacionamentoRecusada.DataHoraEntrada = _request.PassagemPendenteEstacionamento.DataHoraEntrada;
            _passagemReprovada.TransacaoEstacionamentoRecusada.DataHoraTransacao = _request.PassagemPendenteEstacionamento.DataPassagem;
            _passagemReprovada.TransacaoEstacionamentoRecusada.PracaId = _request.PassagemPendenteEstacionamento.Praca.Id.TryToIntNullable();
            _passagemReprovada.TransacaoEstacionamentoRecusada.PistaId = _request.PassagemPendenteEstacionamento.Pista.Id.TryToIntNullable();
            _passagemReprovada.TransacaoEstacionamentoRecusada.TagId = _request.PassagemPendenteEstacionamento.Tag.Id.TryToIntNullable();
            _passagemReprovada.TransacaoEstacionamentoRecusada.ConveniadoId = _request.PassagemPendenteEstacionamento.Conveniado.Id.TryToIntNullable();
            _passagemReprovada.TransacaoEstacionamentoRecusada.DataCadastro = DateTime.Now;
            _passagemReprovada.TransacaoEstacionamentoRecusada.ValorDesconto = _request.PassagemPendenteEstacionamento.ValorDesconto;
            _passagemReprovada.TransacaoEstacionamentoRecusada.ValorCobrado = _request.PassagemPendenteEstacionamento.Valor;
            _passagemReprovada.TransacaoEstacionamentoRecusada.SurrogateKey = _request.PassagemPendenteEstacionamento.RegistroTransacaoId;
            _passagemReprovada.TransacaoEstacionamentoRecusada.MotivoAtrasoTransamissaoId = (int) _request.PassagemPendenteEstacionamento.MotivoAtrasoTransmissao;

            _passagemReprovada.TransacaoEstacionamentoRecusada.Detalhes = new List<DetalheTransacaoEstacionamentoRecusada>();
            foreach (var d in _request.PassagemPendenteEstacionamento.Detalhes)
            {
                _passagemReprovada.TransacaoEstacionamentoRecusada.Detalhes.Add(new DetalheTransacaoEstacionamentoRecusada
                {
                    DataHoraPassagem = d.Data ?? DateTime.Now,
                    Pista = _request.PassagemPendenteEstacionamento.Praca.Id.TryToIntNullable(),
                    Praca = _request.PassagemPendenteEstacionamento.Praca.Id.TryToIntNullable(),
                    SurrogateKey = _request.PassagemPendenteEstacionamento.RegistroTransacaoId,
                });


            }

            _passagemReprovada.TransacaoEstacionamentoRecusada.NumeroRPS = 0;
            _passagemReprovada.TransacaoEstacionamentoRecusada.SerieRPS = null;
            _passagemReprovada.TransacaoEstacionamentoRecusada.DataReferencia = DateTime.Now;


            var response = new GerarPassagemReprovadaParkResponse
            {
                PassagemReprovadaEstacionamento = _passagemReprovada
            };
            return response;
        }
    }
}
