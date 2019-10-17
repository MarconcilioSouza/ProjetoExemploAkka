using ConectCar.Transacoes.Domain.Enum;

namespace ProcessadorPassagensActors.CommandQuery.Dtos
{
    public class ObterStatusAdesaoIdPlacaDocumentoPortransacaoIdOriginalDto
    {
        public StatusAutorizacao StatusAdesao => (StatusAutorizacao) (StatusId);
        public string Placa { get; set; }
        public long Documento { get; set; }
        public int AdesaoId { get; set; }

        public decimal Valor { get; set; }

        public int StatusId { get; set; }
    }
}
