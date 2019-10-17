using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.CommandQuery.Exceptions
{
    public class EdiTransacaoParceiroException : EdiDomainException
    {
        public int? DetalheViagemId { get; set; }
        public CodigoRetornoTransacaoTRF CodigoRetornoTransacaoTrf { get; }
        public PassagemPendenteEDI PassagemPendenteEdi { get; set; }

        public EdiTransacaoParceiroException(CodigoRetornoTransacaoTRF codigoTransacaoTrf, PassagemPendenteEDI passagemPendenteEdi, int detalheViagemId)
        {
            DetalheViagemId = detalheViagemId;
            PassagemPendenteEdi = passagemPendenteEdi;
            CodigoRetornoTransacaoTrf = codigoTransacaoTrf;
        }
    }
}
