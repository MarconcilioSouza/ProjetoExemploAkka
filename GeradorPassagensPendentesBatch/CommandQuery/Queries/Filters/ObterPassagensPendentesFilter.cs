namespace GeradorPassagensPendentesBatch.CommandQuery.Queries.Filters
{
   public class ObterPassagensPendentesFilter
    {
        public int QtdMaximaPassagens { get; set; }
        public int QuantidadeMinutosTtl { get; set; }
        public string ItemGrupoProcessamentoId { get; set; }        
    }
}
