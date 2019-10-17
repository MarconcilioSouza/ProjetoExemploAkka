namespace ProcessadorPassagensProcessadasApi.CommandQuery.Response
{
    public class PassagensReprovadasEdiResponse : PassagensEdiResponse
    {
        public int QtdDetalheTRFRecusadoStaging { get; set; }
        public int QtdVeiculoStaging { get; set; }
        public int QtdTransacaoRecusadaParceiroStaging { get; set; }
    }
}
