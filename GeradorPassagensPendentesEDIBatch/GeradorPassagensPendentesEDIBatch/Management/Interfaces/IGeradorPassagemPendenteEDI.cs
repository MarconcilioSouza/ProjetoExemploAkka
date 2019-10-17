using System.Threading.Tasks;

namespace GeradorPassagensPendentesEDIBatch.Management.Interfaces
{
    public interface IGeradorPassagemPendenteEDI
    {
        Task ExecuteAsync();
    }
}