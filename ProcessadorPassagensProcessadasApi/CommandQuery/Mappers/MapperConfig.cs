using AutoMapper;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public static class MapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AceiteManualReenvioPassagemProfile>();
                cfg.AddProfile<ConfiguracaoAdesaoProfile>();
                cfg.AddProfile<DetalheViagemProfile>();
                cfg.AddProfile<DetalheTRFAprovadaManualmenteProfile>();
                cfg.AddProfile<DetalheTRFRecusadoProfile>();
                cfg.AddProfile<DivergenciaCategoriaConfirmadaProfile>();
                cfg.AddProfile<EstornoPassagemProfile>();
                cfg.AddProfile<EventoProfile>();
                cfg.AddProfile<ExtratoProfile>();
                cfg.AddProfile<PassagemProfile>();
                cfg.AddProfile<PassagemProcessadaProfile>();
                cfg.AddProfile<SolicitacaoImagemProfile>();
                cfg.AddProfile<TransacaoPassagemProfile>();
                cfg.AddProfile<TransacaoProvisoriaEdiProfile>();
                cfg.AddProfile<TransacaoRecusadaParceiroProfile>();
                cfg.AddProfile<TransacaoRecusadaProfile>();
                cfg.AddProfile<TransacaoEstacionamentoRecusadaProfile>();
                cfg.AddProfile<VeiculoProfile>();
                cfg.AddProfile<TransacaoPassagemEdiProfile>();
                cfg.AddProfile<TransacaoPassagemEstacionamentoProfile>();
                cfg.AddProfile<PistaInformacoesRpsProfile>();
                cfg.AddProfile<ConveniadoInformacoesRpsProfile>();
                cfg.AddProfile<DetalhePassagemEstacionamentoProfile>();
                cfg.AddProfile<DetalheTransacaoEstacionamentoRecusadaProfile>();
            });
        }
    }
}
