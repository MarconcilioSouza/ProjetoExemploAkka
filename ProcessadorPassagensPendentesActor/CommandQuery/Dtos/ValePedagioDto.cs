using System.CodeDom;
using ConectCar.Transacoes.Domain.Model;
using System.Collections.Generic;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Dtos
{
    public abstract class ValePedagioDto
    {
        public List<DetalheViagem> ViagensParaRetorno { get; set; }
    }


    public class ValePedagioArtespDto : ValePedagioDto
    {
        public MotivoNaoCompensado MotivoNaoCompensado { get; set; }
        public int? ViagemNaoCompensadaId { get; set; }
    }

    public class ValePedagioEdiDto : ValePedagioDto
    {
        public ValePedagioEdiDto()
        {
            ViagensParaRetorno = new List<DetalheViagem>();
        }
    }
}
