using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class VeiculoLoteStaging
    {
        public Guid ExecucaoId { get; set; }
        public int? Id { get; set; }
        public int? StagingId { get; set; }
        public string Placa { get; set; }
        public bool? CategoriaConfirmada { get; set; }
        public int? CategoriaVeiculoId { get; set; }
        public int? ContagemConfirmadaCategoria { get; set; }
        public int? ContagemDivergenciaConfirmada { get; set; }
        public DateTime? DataConfirmacaoCategoria { get; set; }
        public int? CategoriaId { get; set; }
        public bool? Falha { get; set; }
        public string Motivo { get; set; }
    }
}
