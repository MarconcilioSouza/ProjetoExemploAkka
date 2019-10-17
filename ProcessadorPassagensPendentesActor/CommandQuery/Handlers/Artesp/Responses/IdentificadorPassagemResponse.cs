namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses
{
    public class IdentificadorPassagemResponse
    {
        public long? PassagemId { get; set; }

        public bool ExistePassagem()
        {
            return PassagemId != null && PassagemId.Value > 0;
        }
    }
}
