using System.Threading.Tasks;

namespace LeitorPassagensProcessadasBatch.Management.Interface
{
    public interface IMonitorTransacaoQueue
    {
        Task Executar();
    }
}
