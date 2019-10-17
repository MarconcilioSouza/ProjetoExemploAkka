namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests
{
    public class GeradorPassagemAprovadaComTransacaoExistenteRequest
    {
        public long TransacaoId { get; set; }
        public int CodigoProtocoloArtesp { get; set; }

        public GeradorPassagemAprovadaComTransacaoExistenteRequest(long transacaoId, int codigoProtocoloArtesp)
        {
            TransacaoId = transacaoId;
            CodigoProtocoloArtesp = codigoProtocoloArtesp;
        }
    }
}
