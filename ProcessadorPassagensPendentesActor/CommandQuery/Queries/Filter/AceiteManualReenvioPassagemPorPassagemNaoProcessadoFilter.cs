namespace ProcessadorPassagensActors.CommandQuery.Queries.Filter
{
    public class AceiteManualReenvioPassagemPorPassagemNaoProcessadoFilter
    {
        public long CodigoPassagemConveniado { get; }
        public long CodigoProtocoloArtesp { get; }

        public AceiteManualReenvioPassagemPorPassagemNaoProcessadoFilter(long codigoPassagemConveniado, long codigoProtocoloArtesp)
        {
            CodigoPassagemConveniado = codigoPassagemConveniado;
            CodigoProtocoloArtesp = codigoProtocoloArtesp;
        }
    }
}
