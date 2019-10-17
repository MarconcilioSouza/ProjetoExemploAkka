namespace ProcessadorPassagensProcessadasApi.CommandQuery.Response
{
    public class PassagensAprovadasEdiResponse : PassagensEdiResponse
    {
        public int QtdTransacaoPassagemEDIStaging { get; set; }
        public int QtdDetalheTRFRecusaEvasaoStaging { get; set; }
        public int QtdTransacaoProvisoriaEDIStaging { get; set; }
        public int QtdDetalheTRFAprovadoManualmenteStaging { get; set; }
        public int QtdExtratoStaging { get; set; }
        public int QtdEventoStaging  { get; set; }
        public int QtdConfiguracaoAdesaoStaging { get; set; }
        public int QtdDivergenciaCategoriaConfirmadaStaging { get; set; }
        public int QtdVeiculoStaging { get; set; }
        public int QtdDetalheViagemStaging { get; set; }
    }
}
