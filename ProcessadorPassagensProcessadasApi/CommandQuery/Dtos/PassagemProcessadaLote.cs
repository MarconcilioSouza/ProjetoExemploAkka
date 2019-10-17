namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class PassagemProcessadaLote
    {
        public long? MensagemId { get; set; }
        public long? DataPagamento { get; set; }
        public bool? ValePedagio { get; set; }
        public long? MotivoNaoCompensado { get; set; }
        public int MotivoOutroValor { get; set; }
        public int MotivoProvisionado { get; set; }
        public long? Resultado { get; set; }
        public long? PassagemId { get; set; }
        public long? TransacaoId { get; set; }
        public int Valor { get; set; }
        public int Reenvio { get; set; }
        public long? MensagemItemId { get; set; }

    }
}
