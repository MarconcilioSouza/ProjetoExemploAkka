using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class ConfiguracaoAdesaoLoteStaging
    {
        public Guid ExecucaoId { get; set; }
        public int? Id { get; set; }
        public int? CategoriaId { get; set; }
        public bool? Falha { get; set; }
        public int? StagingId { get; set; }
    }
}
