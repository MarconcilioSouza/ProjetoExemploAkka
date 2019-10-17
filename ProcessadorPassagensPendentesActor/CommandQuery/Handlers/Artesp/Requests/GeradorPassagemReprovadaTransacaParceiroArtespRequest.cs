using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests
{
    public class GeradorPassagemReprovadaTransacaParceiroArtespRequest
    {
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }
        public MotivoNaoCompensado MotivoNaoCompensado { get; set; }
        public int DetalheViagemId { get; set; }
    }
}
