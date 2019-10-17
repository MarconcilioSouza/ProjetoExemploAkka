using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests
{
    public class GeradorPassagemProcessadaMensageriaRequest
    {
        public MotivoNaoCompensado MotivoNaoCompensado { get; }
        public ResultadoPassagem ResultadoPassagem { get; set; }
        public long TransacaoIdOriginal { get; }
        public decimal Valor { get; }
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; }
        public DateTime DataPagamento { get; }
        public int PassagemId { get; set; }

        public GeradorPassagemProcessadaMensageriaRequest(
            ResultadoPassagem resultadoPassagem,
            PassagemPendenteArtesp passagemPendenteArtesp,
            long transacaoIdOriginal,
            decimal valor,
            DateTime dataPagamento,
            MotivoNaoCompensado motivoNaoCompensado,
            int passagemId)
        {
            ResultadoPassagem = resultadoPassagem;
            MotivoNaoCompensado = motivoNaoCompensado;
            TransacaoIdOriginal = transacaoIdOriginal;
            Valor = valor;
            PassagemPendenteArtesp = passagemPendenteArtesp;
            DataPagamento = dataPagamento;
            PassagemId = passagemId;
        }
    }
}
