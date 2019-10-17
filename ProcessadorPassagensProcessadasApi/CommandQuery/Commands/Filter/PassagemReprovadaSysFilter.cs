using ConectCar.Transacoes.Domain.Dto;
using System;
using System.Collections.Generic;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Filter
{
    public class PassagemReprovadaSysFilter
    {
        public Guid ExecucaoId { get; set; }
        public IEnumerable<PassagemDto> Passagens { get; set; }
        public IEnumerable<TransacaoRecusadaDto> TransacoesRecusada { get; set; }
        public IEnumerable<TransacaoRecusadaParceiroDto> TransacoesRecusadaParceiro { get; set; }
        public IEnumerable<VeiculoDto> Veiculos { get; set; }

    }
}
