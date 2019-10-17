namespace ProcessadorPassagensProcessadasApi.CommandQuery.Response
{
    public class PassagensAprovadasArtespResponse: PassagensArtespResponse
    {
        public int QtdAceiteManualReenvioStaging { get; set; }
        public int QtdConfiguracaoAdesaoStaging { get; set; }
        public int QtdDivergenciaCategoriaConfirmadaStaging { get; set; }
        public int QtdEstornoStaging { get; set; }
        public int QtdExtratoEstornoStaging { get; set; }
        public int QtdEventoStaging { get; set; }
        public int QtdExtratoStaging { get; set; }
        public int QtdPassagensStaging { get; set; }
        public int QtdPassagensProcessadasStaging { get; set; }
        public int QtdSolicitacaoImagemStaging { get; set; }
        public int QtdTransacaoPassagemStaging { get; set; }
        public int QtdVeiculosStaging { get; set; }
        public int QtdViagemValePedagioStaging { get; set; }        
    }
}
