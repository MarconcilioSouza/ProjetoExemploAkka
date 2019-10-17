using ConectCar.Transacoes.Domain.Dto;
using System.Collections.Generic;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Filter
{
    public class PassagemReprovadaEdiSysArgs
    {
        public IEnumerable<DetalheTRFRecusadoDto> DetalheTRFRecusado { get; set; }
        public IEnumerable<VeiculoDto> Veiculo { get; set; }
        public IEnumerable<TransacaoRecusadaParceiroEdiDto> TransacaoRecusadaParceiro { get; set; }
    }
}
