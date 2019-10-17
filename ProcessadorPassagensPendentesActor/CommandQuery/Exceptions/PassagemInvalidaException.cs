using System;

namespace ProcessadorPassagensActors.CommandQuery.Exceptions
{
    public class PassagemInvalidaException : Exception
    {
        public long ReferenceKey { get; set; }

        public PassagemInvalidaException(long referenceKey, string message, Exception innerException): base(message, innerException)
        {
            ReferenceKey = referenceKey;
        }
    }

}
