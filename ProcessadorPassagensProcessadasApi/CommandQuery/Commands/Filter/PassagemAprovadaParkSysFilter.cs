using System;
using System.Collections.Generic;
using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Filter
{
    public class PassagemAprovadaParkSysFilter
    {
        public Guid ExecucaoId { get; set; }
        public IEnumerable<TransacaoEstacionamento> Transacoes { get; set; }
        public IEnumerable<DetalhePassagemEstacionamento> DetalhePassagens { get; set; }
        public IEnumerable<PistaInformacoesRps> PistaInformacoes { get; set; }
        public IEnumerable<ConveniadoInformacoesRps> ConveniadoInformacoes { get; set; }
    }
}
