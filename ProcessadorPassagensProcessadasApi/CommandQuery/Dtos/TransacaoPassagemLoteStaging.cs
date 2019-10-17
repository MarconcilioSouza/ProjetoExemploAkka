using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class TransacaoPassagemLoteStaging
    {
        public Guid ExecucaoId { get; set; }
        public DateTime Data { get; set; }
        public int StatusId { get; set; }
        public int Id { get; set; }
        public int AdesaoId { get; set; }
        public int TipoOperacaoId { get; set; }
        public bool Credito { get; set; }
        public decimal Valor { get; set; }
        public long SurrogateKey { get; set; }
        public string ChaveCriptografiaBanco { get; set; }
        public int SaldoId { get; set; }
        public int OrigemTransacaoId { get; set; }
        public DateTime DataDePassagem { get; set; }
        public decimal ValorRepasse { get; set; }
        public decimal TarifaDeInterconexao { get; set; }
        public int PistaId { get; set; }
        public int CategoriaUtilizadaId { get; set; }
        public int RepasseId { get; set; }
        public DateTime DataRepasse { get; set; }
        public int ItemListaDeParaUtilizado { get; set; }
        public long? DetalheTRNId { get; set; }
        public bool EvasaoAceita { get; set; }
        public int? TransacaoDeCorrecaoId { get; set; }
        public bool TransacaoProvisoria { get; set; }
        public int EstornoId { get; set; }
        public long PassagemId { get; set; }
        public bool Falha { get; set; }
    }
}