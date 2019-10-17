using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeitorPassagensPendentesBatch.Processadores.Interface
{
    public interface IProcessador
    {
        Task Processar();
    }
}
