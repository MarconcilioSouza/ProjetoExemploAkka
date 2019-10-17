namespace ProcessadorPassagensProcessadasApi.CommandQuery.Response
{
    public class PassagensAprovadasParkResponse : PassagensParkResponse
    {
        public int QtdConveniadoInformacoesRPSStaging { get; set; }
        public int QtdDetalhePassagemEstacionamentoStaging { get; set; }
        public int QtdPistaInformacoesRPSStaging { get; set; }
        public int QtdTransacaoPassagemEstacionamentoStaging { get; set; }
    }
}
