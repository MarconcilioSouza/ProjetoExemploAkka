using System;

namespace ProcessadorPassagensActors.CommandQuery.Exceptions
{
   public class TransacaoPassagemExistenteException : Exception
    {
        public long TransacaoId { get; }
        public long MensagemItemId { get; set; }
        public int CodigoProtocoloArtesp { get; set; }
    

    public TransacaoPassagemExistenteException(long transacaoId, long mensagemItemId, int codigoProtocoloArtesp) 
        {
            TransacaoId = transacaoId;
            MensagemItemId = mensagemItemId;
            CodigoProtocoloArtesp = codigoProtocoloArtesp;
        }

    }
}
