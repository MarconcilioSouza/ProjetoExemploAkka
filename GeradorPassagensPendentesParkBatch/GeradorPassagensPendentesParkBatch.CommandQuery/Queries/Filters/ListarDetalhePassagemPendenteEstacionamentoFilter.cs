namespace GeradorPassagensPendentesParkBatch.CommandQuery.Queries
{
    public class ListarDetalhePassagemPendenteEstacionamentoFilter
    {
        public int QuantidadeMaximaPassagens { get; set; }
        public int QuantidadeMinutosTtl { get; set; }
        public string ItemGrupoProcessamentoId { get; set; }
    }
}