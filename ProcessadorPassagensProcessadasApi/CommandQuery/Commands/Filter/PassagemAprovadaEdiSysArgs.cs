using ConectCar.Transacoes.Domain.Dto;
using System.Collections.Generic;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Filter
{
    public class PassagemAprovadaEdiSysArgs
    {
        public IEnumerable<TransacaoPassagemEDIDto> TransacoesPassagens { get; set; }
        public IEnumerable<DetalheTRFRecusadoDto> DetalheTRFRecusado { get; set; }
        public IEnumerable<TransacaoProvisoriaEDIDto> TransacaoProvisoria { get; set; }
        public IEnumerable<DetalheTRFAprovadoManualmenteDto> DetalheTRFAprovadoManualmente { get; set; }
        public IEnumerable<ExtratoDto> Extrato { get; set; }
        public IEnumerable<EventoDto> Evento { get; set; }
        public IEnumerable<ConfiguracaoAdesaoDto> ConfiguracaoAdesao { get; set; }
        public IEnumerable<DivergenciaCategoriaConfirmadaDto> DivergenciaCategoriaConfirmada { get; set; }
        public IEnumerable<VeiculoDto> Veiculo { get; set; }
        public IEnumerable<DetalheViagemDto> DetalheViagem { get; set; }

    }
}
