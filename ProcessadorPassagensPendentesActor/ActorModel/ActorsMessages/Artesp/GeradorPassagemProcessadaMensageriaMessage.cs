using System;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.ActorsMessages.Artesp
{
    public class GeradorPassagemProcessadaMensageriaMessage
    {
        public ResultadoPassagem ResultadoPassagem{ get; set; }
        public MotivoNaoCompensado MotivoNaoCompensado { get; }
        public long TransacaoIdOriginal { get; }
        public decimal ValorRepasse { get; }
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; }
        public DateTime DataPagamento { get; }
        public int PassagemId { get; set; }


        public GeradorPassagemProcessadaMensageriaMessage(
            ResultadoPassagem resultadoPassagem,
            PassagemPendenteArtesp passagemPendenteArtesp,
            long transacaoIdOriginal,
            decimal valorRepasse,
            DateTime dataPagamento,
            MotivoNaoCompensado motivoNaoCompensado,
            int passagemId):this(resultadoPassagem, passagemPendenteArtesp, motivoNaoCompensado)
        {            
            TransacaoIdOriginal = transacaoIdOriginal;
            ValorRepasse = valorRepasse;            
            DataPagamento = dataPagamento;
            PassagemId = passagemId;
        }

        public GeradorPassagemProcessadaMensageriaMessage(ResultadoPassagem resultadoPassagem
            , PassagemPendenteArtesp passagemPendenteArtesp
            , MotivoNaoCompensado motivoNaoCompensado)
        {
            ResultadoPassagem = resultadoPassagem;
            PassagemPendenteArtesp = passagemPendenteArtesp;
            MotivoNaoCompensado = motivoNaoCompensado;
        }
    }
}