using System.Threading.Tasks;

namespace LeitorPassagensPendentesBatch.Management.Interface
{
    public interface IMonitorPassagemQueue
    {
        /// <summary>
        /// Método responsável por verificar se existe pendencias nas filas das concessionarias e enviar para o akka
        /// </summary>
        Task Executar();

    }
}
