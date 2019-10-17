using System;

namespace ProcessadorPassagensActors.CommandQuery.Dtos
{
   public class PassagemAnteriorValidaDto : TagAdesaoDto
    {
        public int PassagemId { get; set; }
        public int? CategoriaCobradaId { get; set; }
        public int? CategoriaDetectadaId { get; set; }
        public int? CategoriaCadastradaId { get; set; }
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
        public int Reenvio { get; set; }
        public long CodigoPassagemConveniado { get; set; }
        public int CodigoPraca { get; set; }
        public int CodigoPista { get; set; }        
    }
}
