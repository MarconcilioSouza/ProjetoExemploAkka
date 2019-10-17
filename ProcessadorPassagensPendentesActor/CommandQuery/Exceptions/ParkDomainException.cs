using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Exceptions
{
    public class ParkDomainException : Exception
    {
        public PassagemPendenteEstacionamento PassagemPendenteEstacionamento { get; }
        public override string Message { get; }

        public ParkDomainException(PassagemPendenteEstacionamento passagemPendenteEstacionamento, string message)
        {
            PassagemPendenteEstacionamento = passagemPendenteEstacionamento;
            Message = $"Erro na requisição - {message}";
        }
    }
}
