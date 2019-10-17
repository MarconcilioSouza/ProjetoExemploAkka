using ConectCar.Transacoes.Domain.Dto;
using System;
using System.Collections.Generic;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Filter
{
    public class PassagemProcessadaFilter
    {
        public List<PassagemProcessadaArtespDto> PassagensProcessadas { get; set; }
        public Guid ExecucaoId { get; set; }
    }
}
