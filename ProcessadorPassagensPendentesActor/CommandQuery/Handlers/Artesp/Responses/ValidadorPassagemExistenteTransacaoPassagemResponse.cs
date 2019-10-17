namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses
{
    public class ValidadorPassagemExistenteTransacaoPassagemResponse : ValidadorPassagemExistenteResponse
    {
        public long TransacaoId { get; set; }
        public long MensagemItemId { get; set; }
        public int CodigoProtocoloArtesp { get; set; }
    }
}
