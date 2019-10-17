using System;

namespace ProcessadorPassagensActors.CommandQuery.Dtos
{
    public class PeriodoFaturaDto
    {
        public DateTime DataReferenciaInicial { get; set; }
        public DateTime DataReferenciaFinal { get; set; }
        public DateTime DataVencimento { get; set; }
    }
}
