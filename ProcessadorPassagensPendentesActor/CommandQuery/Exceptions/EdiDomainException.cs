using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.CommandQuery.Exceptions
{
    public class EdiDomainException : Exception
    {
        public PassagemPendenteEDI PassagemPendenteEdi { get; set; }
        public EdiDomainException(string mensagem, PassagemPendenteEDI passagemPendenteEdi) : this(new Exception(mensagem))
        {
            PassagemPendenteEdi = passagemPendenteEdi;
        }

        public EdiDomainException(Exception exception) : this(exception.Message, exception)
        {
        }

        public EdiDomainException(string mensagem, Exception exception) : base(mensagem, exception)
        {
            Errors = new List<Exception> { exception };
        }

        public EdiDomainException()
        {
        }

        public List<Exception> Errors { get; set; }
    }
}
