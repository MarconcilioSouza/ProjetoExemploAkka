using LeitorPassagensPendentesBatch.CommandQuery.Handlers;
using LeitorPassagensPendentesBatch.CommandQuery.Handlers.Request;
using LeitorPassagensPendentesBatch.Processadores.Interface;
using System.Threading.Tasks;

namespace LeitorPassagensPendentesBatch.Processadores
{
    class ProcessadorDeMensagensPark : IProcessador
    {
        private readonly LeitorParkHandler leitorParkHandler;

        public ProcessadorDeMensagensPark()
        {
            leitorParkHandler = new LeitorParkHandler();
        }

        public async Task Processar()
        {
            await Task.Run(() =>
            {
                var passagens = leitorParkHandler.Execute(new LerPassagemPendenteParkRequest());

                leitorParkHandler.Execute(new EnviarPassagemParkRequest { passagemPendenteMessagePark = passagens });
            });
        }
    }
}
