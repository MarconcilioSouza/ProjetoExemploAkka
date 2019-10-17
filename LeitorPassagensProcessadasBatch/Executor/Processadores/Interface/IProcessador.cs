using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using LeitorPassagensProcessadasBatch.CommandQuery.Handlers;
using System.Threading.Tasks;

namespace LeitorPassagensProcessadasBatch.Processadores.Interface
{
    public interface IProcessador
    {
        Task Processar();
    }
}
