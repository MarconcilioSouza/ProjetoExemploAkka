using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class DetalheTRFAprovadaManualmenteLoteStaging
    {
        public Guid ExecucaoId { get; set; }    
        public int Id { get; set; }
        public int? StagingId { get; set; }
        public int TransacaoPassagemId { get; set; }
        public long SurrogateKey { get; set; }
    }
}
