using System.Collections.Generic;
using ConectCar.Transacoes.Domain.Enum;

namespace ProcessadorPassagensActors.CommandQuery.Dtos
{
   public class TagMensageriaDto
    {
        public int Id { get; set; }

        public int? SituacaoId { get; set; }

        public List<PracasBloqueadas> PracasBloqueadases { get; set; }

        public SituacaoTag SituacaoTag => (SituacaoTag) (SituacaoId ?? 0);

    }


    public class PracasBloqueadas
    {
        public long CodigoPraca { get; set; }
        public int Id { get; set; }
    }
}
