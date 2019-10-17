using System;
using ProcessadorPassagensActors.CommandQuery.Enums;

namespace ProcessadorPassagensActors.CommandQuery.Dtos
{
    public class RepasseDto
    {
        public int RepasseId { get; set; }
        public decimal? TarifaDeInterconexao { get; set; }
        public int Dias { get; set; }
        public DateTime VigenciaInicio { get; set; }
        public int ConveniadoId { get; set; }
        public int TipoDeRepasseId { get; set; }
        public DateTime DataDeCadastro { get; set; }
        public int PlanoId { get; set; }
        public decimal TarifaDeInterconexaoParceiro { get; set; }
        public decimal TarifaDeInterconexaoValePedagio { get; set; }
        public int PistaId { get; set; }

        public TipoRepasse TipoRepasse => (TipoRepasse) TipoDeRepasseId;


    }
}
