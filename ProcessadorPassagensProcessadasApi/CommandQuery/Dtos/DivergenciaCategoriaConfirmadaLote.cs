using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class DivergenciaCategoriaConfirmadaLote
    {
        public int? Id { get; set; }
        public DateTime? Data { get; set; }
        public int? CategoriaVeiculoId { get; set; }
        public int? TransacaoPassagemId { get; set; }
        public long? Surrogatekey { get; set; }

    }
}
