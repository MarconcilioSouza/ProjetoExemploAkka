using System.Collections.Generic;
using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.ActorsMessages.Edi
{
    public class GeradorPassagemAprovadaEdiMessage
    {
        public PassagemPendenteEDI PassagemPendenteEdi { get; set; }
        public List<DetalheViagem> DetalheViagens { get; set; }
    }
}