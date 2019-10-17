namespace ProcessadorPassagensActors.ActorsMessages.Artesp
{
    public class GeradorPassagemAprovadaComTransacaoExistenteArtespMessage
    {
        public long TransacaoId { get; set; }
        public long MensagemItemId { get; set; }

        public int CodigoProtocoloArtesp { get; set; }
    }
}