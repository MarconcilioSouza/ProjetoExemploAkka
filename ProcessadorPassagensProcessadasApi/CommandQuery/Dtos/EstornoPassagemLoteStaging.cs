using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class EstornoPassagemLoteStaging: EstornoPassagemLote
    {
        public Guid ExecucaoId { get; set; }       
    }
}
