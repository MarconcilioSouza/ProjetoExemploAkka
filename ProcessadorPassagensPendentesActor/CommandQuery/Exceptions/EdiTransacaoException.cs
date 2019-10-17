using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.CommandQuery.Exceptions
{
    public class EdiTransacaoException : EdiDomainException
    {
        public CodigoRetornoTransacaoTRF CodigoRetornoTransacaoTrf { get; }

        public PassagemPendenteEDI PassagemPendenteEdi { get; }


        public EdiTransacaoException(CodigoRetornoTransacaoTRF codigoTransacaoTrf, PassagemPendenteEDI passagemPendenteEdi)
        {
            CodigoRetornoTransacaoTrf = codigoTransacaoTrf;
            PassagemPendenteEdi = passagemPendenteEdi;
        }
    }
}
