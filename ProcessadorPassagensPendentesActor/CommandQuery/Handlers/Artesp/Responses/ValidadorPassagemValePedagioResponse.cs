using System.Collections.Generic;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;
using ConectCar.Transacoes.Domain.Enum;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses
{
    public class ValidadorPassagemValePedagioResponse
    {
        /// <summary>
        /// PassagemPendenteArtesp
        /// </summary>
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }

        public List<DetalheViagem> ViagensAgendadas { get; set; }

        public MotivoNaoCompensado MotivoNaoCompensado { get; set; }

        public int? ViagemNaoCompensadaId { get; set; }
    }
}
