using ConectCar.Transacoes.Domain.Model;
using System;
using System.Collections.Generic;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Args
{
    public class PassagemReprovadaSysParkFilter
    {
        public Guid ExecucaoId { get; set; }
        public IEnumerable<TransacaoEstacionamentoRecusada> TransacoesRecusadas { get; set; }
        public IEnumerable<DetalheTransacaoEstacionamentoRecusada> DetalheTransacoes { get; set; }
    }
}
