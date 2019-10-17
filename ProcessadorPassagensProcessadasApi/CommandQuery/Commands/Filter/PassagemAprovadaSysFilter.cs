using ConectCar.Transacoes.Domain.Dto;
using System;
using System.Collections.Generic;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Filter
{
    public class PassagemAprovadaSysFilter
    {
        public Guid ExecucaoId { get; set; }
        public IEnumerable<PassagemDto> Passagens { get; set; }
        public IEnumerable<TransacaoPassagemDto> TransacoesPassagens { get; set; }
        public IEnumerable<ExtratoDto> Extratos { get; set; }
        public IEnumerable<EstornoPassagemDto> EstornosPassagem { get; set; }
        public IEnumerable<ExtratoDto> ExtratosEstornos { get; set; }
        public IEnumerable<VeiculoDto> Veiculos { get; set; }
        public IEnumerable<EventoDto> Eventos { get; set; }
        public IEnumerable<DetalheViagemDto> DetalhesViagem { get; set; }
        public IEnumerable<SolicitacaoImagemDto> SolicitacoesImagem { get; set; }
        public IEnumerable<AceiteManualReenvioPassagemDto> AceitesManuaisReenvioPassagem { get; set; }
        public IEnumerable<ConfiguracaoAdesaoDto> ConfiguracoesAdesao { get; set; }
        public IEnumerable<DivergenciaCategoriaConfirmadaDto> DivergenciasCategoriaConfirmada { get; set; }

    }
}
