using ConectCar.Transacoes.Domain.Enum;

namespace ProcessadorPassagensActors.CommandQuery.Dtos
{
   public class ItemListaDeParaDto
    {
        public int ItemListaDeParaId { get; set; }
        public int CategoriaVeiculoId { get; set; }
        public int StatusId { get; set; }

        public bool ValidarLista()
        {
            return StatusId == (int) StatusListaDePara.Ativo;
        }
    }
}
