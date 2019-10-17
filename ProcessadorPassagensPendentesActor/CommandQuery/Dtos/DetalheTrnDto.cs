using System;

namespace ProcessadorPassagensActors.CommandQuery.Dtos
{
    public class DetalheTrnDto
    {
        public DateTime Data { get; set; }
        public long OBUId { get; set; }
        public int? CodigoPraca { get; set; }
        public int DetalheTrnId { get; set; }
        public int StatusPassagemId { get; set; }
        public int StatusCobrancaId { get; set; }
    }
}
