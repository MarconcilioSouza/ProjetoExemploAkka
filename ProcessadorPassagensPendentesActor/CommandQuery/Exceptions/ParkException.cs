using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Enums;

namespace ProcessadorPassagensActors.CommandQuery.Exceptions
{
    public class ParkException : Exception
    {
        public ParkException(PassagemPendenteEstacionamento passagemPendenteEstacionamento ,EstacionamentoErros erro)
            : base(erro.GetDescription())
        {
            PassagemPendenteEstacionamento = passagemPendenteEstacionamento;
            Erro = erro;
        }

        public PassagemPendenteEstacionamento PassagemPendenteEstacionamento { get; }
        public EstacionamentoErros Erro { get; set; }
    }
}
