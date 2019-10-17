using LeitorPassagensPendentesBatch.CommandQuery.Handlers;
using LeitorPassagensPendentesBatch.CommandQuery.Handlers.Request;
using LeitorPassagensPendentesBatch.Processadores.Interface;
using System.Threading.Tasks;

namespace LeitorPassagensPendentesBatch.Processadores
{
    public sealed class ProcessadorDeMensagensArtesp : IProcessador
    {
        private readonly LeitorArtespHandler leitorArtespHandler;

        public ProcessadorDeMensagensArtesp()
        {
            leitorArtespHandler = new LeitorArtespHandler();
        }
        
        public async Task Processar()
        {
            await Task.Run(() =>
            {
                var passagens = leitorArtespHandler.Execute(new LerPassagemPendenteArtespRequest());

                leitorArtespHandler.Execute(new EnviarPassagemArtespRequest { passagemPendenteMessageArtesp = passagens });
            });
        }
    }

}
