using System.Linq;
using LeitorPassagensPendentesBatch.CommandQuery.Handlers;
using LeitorPassagensPendentesBatch.CommandQuery.Handlers.Request;
using LeitorPassagensPendentesBatch.Processadores.Interface;
using System.Threading.Tasks;

namespace LeitorPassagensPendentesBatch.Processadores
{
    public sealed class ProcessadorDeMensagensEdi : IProcessador
    {
        private readonly LeitorEdiHandler _leitorEdipHandler;

        public ProcessadorDeMensagensEdi()
        {
            _leitorEdipHandler = new LeitorEdiHandler();
        }

        public async Task Processar()
        {
            await Task.Run(() =>
            {
                var passagens = _leitorEdipHandler.Execute(new LerPassagemPendenteEdiRequest());
                
                _leitorEdipHandler.Execute(new EnviarPassagemEdiRequest { passagemPendenteMessageEdi = passagens });
            });
        }
    }
}
