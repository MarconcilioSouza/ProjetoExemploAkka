namespace ProcessadorPassagensProcessadasApi.CommandQuery.Response
{
    public class PassagensReprovadasParkResponse : PassagensParkResponse
    {
        public int QtdTransacaoEstacionamentoRecusadaStaging { get; set; }
        public int QtdDetalhePassagemEstacionamentoRecusadaStaging { get; set; }
    }
}
