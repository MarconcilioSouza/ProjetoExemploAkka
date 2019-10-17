using ConectCar.Transacoes.Domain.Dto;
using ConectCar.Transacoes.Domain.Model;
using System.Collections.Generic;
using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Filter
{
    public class PassagemReprovadaSysParkFilter
    {
        public Guid ExecucaoId { get; set; }
        public IEnumerable<TransacaoEstacionamentoRecusada> TransacoesRecusadas { get; set; }
        public IEnumerable<DetalheTransacaoEstacionamentoRecusada> DetalheTransacoes { get; set; }
    }
}
