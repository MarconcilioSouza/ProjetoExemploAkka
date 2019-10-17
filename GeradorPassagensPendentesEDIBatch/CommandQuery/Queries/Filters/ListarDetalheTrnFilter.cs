namespace GeradorPassagensPendentesEDIBatch.CommandQuery.Queries
{
    public class ListarDetalheTrnFilter
    {
        public int QuantidadeMaximaPassagens { get; set; }
        public int QuantidadeMinutosTtl { get; set; }
        public string ItemGrupoProcessamentoId { get; set; }
    }
}