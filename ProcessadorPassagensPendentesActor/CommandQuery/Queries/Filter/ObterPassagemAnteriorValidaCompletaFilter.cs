using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Queries.Filter
{
    public class ObterPassagemAnteriorValidaCompletaFilter
    {
        public long ConveniadoId { get; set; }
        public int Reenvio { get; set; }
        public long CodigoPassagemConveniado { get; set; }
        public int CodigoProtocoloArtesp { get; set; }

        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }


        public ObterPassagemAnteriorValidaCompletaFilter(long conveniadoId, int reenvio, long codigoPassagemConveniado, int codigoProtocoloArtesp)
        {
            ConveniadoId = conveniadoId;
            Reenvio = reenvio;
            CodigoPassagemConveniado = codigoPassagemConveniado;
            CodigoProtocoloArtesp = codigoProtocoloArtesp;

        }
        public ObterPassagemAnteriorValidaCompletaFilter(PassagemPendenteArtesp passagemPendenteArtesp)
        {
            PassagemPendenteArtesp = passagemPendenteArtesp;
            ConveniadoId = passagemPendenteArtesp?.Conveniado?.Id ?? 0;
            Reenvio = passagemPendenteArtesp?.NumeroReenvio ?? 0;
            CodigoPassagemConveniado = passagemPendenteArtesp?.ConveniadoPassagemId ?? 0;
            CodigoProtocoloArtesp = passagemPendenteArtesp?.Conveniado?.CodigoProtocoloArtesp ?? 0;

        }
    }
}
