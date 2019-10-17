using ConectCar.Transacoes.Domain.Enum;

namespace ProcessadorPassagensActors.CommandQuery.Dtos
{
    public class ListaDeParaCategoriaVeiculoDto
    {
        public int ListaDeParaCategoriaVeiculoId { get; set; }
        public string Descricao { get; set; }
        public int StatusId { get; set; }

        public bool ValidarLista()
        {
            return StatusId == (int) StatusListaDePara.Ativo;
        }
    }
}
