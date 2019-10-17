using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Exceptions
{
    public class PassagemInvalidaNoSysException : PassagemException
    {        
        public long TransacaoIdOriginal { get; }
        public decimal ValorRepasse { get; }
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; }
        public DateTime DataPagamento { get; }

        public int PassagemId { get; set; }


        public PassagemInvalidaNoSysException(ResultadoPassagem resultadoPassagem,PassagemPendenteArtesp passagemPendenteArtesp, MotivoNaoCompensado motivoNaoCompensado) : base(motivoNaoCompensado)
        {
            ResultadoPassagem = resultadoPassagem;
            PassagemPendenteArtesp = passagemPendenteArtesp;
            MotivoNaoCompensado = motivoNaoCompensado;
        }

        public PassagemInvalidaNoSysException(ResultadoPassagem resultadoPassagem,PassagemPendenteArtesp passagemPendenteArtesp,
            MotivoNaoCompensado motivoNaoCompensado,
            long transacaoIdOriginal,
            decimal valorRepasse,
            DateTime dataPagamento,
            int passagemId) : base(motivoNaoCompensado)
        {
            ResultadoPassagem = resultadoPassagem;
            this.TransacaoIdOriginal = transacaoIdOriginal;
            this.ValorRepasse = valorRepasse;
            PassagemPendenteArtesp = passagemPendenteArtesp;
            DataPagamento = dataPagamento;
            MotivoNaoCompensado = motivoNaoCompensado;
            PassagemId = passagemId;
        }
    }
}
