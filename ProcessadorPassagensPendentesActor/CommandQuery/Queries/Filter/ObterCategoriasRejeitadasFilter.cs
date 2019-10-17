using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorPassagensActors.CommandQuery.Queries.Filter
{
    public class ObterCategoriasRejeitadasFilter
    {
        public readonly long ConveniadoId;
        public readonly long CategoriaId;
        public ObterCategoriasRejeitadasFilter(long conveniadoId, long categoriaId)
        {
            ConveniadoId = conveniadoId;
            CategoriaId = categoriaId;
        }
    }

}
