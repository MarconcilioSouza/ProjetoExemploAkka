
using System;
using ConectCar.Transacoes.Domain.Enum;

namespace ProcessadorPassagensActors.CommandQuery.Exceptions
{
    public class PassagemException : Exception
    {
        public MotivoNaoCompensado MotivoNaoCompensado { get; protected set; }
        public ResultadoPassagem ResultadoPassagem { get; protected set; }

        protected PassagemException(MotivoNaoCompensado motivoNaoCompensado, ResultadoPassagem resultadoPassagem = ResultadoPassagem.NaoCompensado)
        {
            MotivoNaoCompensado = motivoNaoCompensado;
            ResultadoPassagem = resultadoPassagem;
        }
        public PassagemException(MotivoNaoCompensado motivoNaoCompensado)
        {
            MotivoNaoCompensado = motivoNaoCompensado;
        }
    }
}
