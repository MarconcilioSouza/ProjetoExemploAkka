using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class DivergenciaCategoriaConfirmadaLoteStaging
    {
        public Guid ExecucaoId { get; set; }
        public int? Id { get; set; }
        public DateTime? Data { get; set; }
        public int? CategoriaVeiculoId { get; set; }
        public int? TransacaoPassagemId { get; set; }
        public long? Surrogatekey { get; set; }
        public int? StagingId { get; set; }
    }
}