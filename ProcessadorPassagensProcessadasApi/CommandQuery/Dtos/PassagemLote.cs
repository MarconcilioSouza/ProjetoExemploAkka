using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class PassagemLote
    {
        public int? Id { get; set; }
        public long? SurrogateKey { get; set; }
        public long? MensagemItemId { get; set; }
        public int? ConveniadoId { get; set; }
        public int? TagId { get; set; }
        public int? AdesaoId { get; set; }
        public decimal? Valor { get; set; }
        public DateTime? Data { get; set; }
        public string Placa { get; set; }
        public int? StatusPassagemId { get; set; }
        public int? MotivoSemValorId { get; set; }
        public int? StatusCobrancaId { get; set; }
        public int? CategoriaCobradaId { get; set; }
        public int? CategoriaTagId { get; set; }
        public int? CategoriaDetectadaId { get; set; }
        public long? CodigoPassagemConveniado { get; set; }
        public int? CodigoPraca { get; set; }
        public int? CodigoPista { get; set; }
        public bool? TagBloqueadaMomentoRecebimento { get; set; }
        public bool? PassagemRecusadaMensageria { get; set; }
        public int? Reenvio { get; set; }
        public bool? SomenteoInformacoesAlteradas { get; set; }

    }
}
