using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorPassagensActors.CommandQuery.Dtos
{
    public class IncidenteDto
    {
        public bool Ativo { get; set; }
        public DateTime? VigenciaFim { get; set; }

        public DateTime? VigenciaInicio { get; set; }

        public virtual bool EstaAtivo()
        {
            return Ativo && VigenciaFim.HasValue && DateTime.Now.Subtract(VigenciaFim.Value).Days <= 60;
        }
    }
}
