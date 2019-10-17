using System.Linq;
using ConectCar.Comercial.Cliente.Adesao.Backend.CommonQuery.Query;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterEcarregarCategoriaVeiculoPorCodigoEdiQuery
    {
        public void Execute(PassagemPendenteEDI passagemPendenteEDI,
          DbConnectionDataSource dbSysReadOnly)
        {
            var query = new ObterCategoriaVeiculoQuery(true, dbSysReadOnly);
            var categorias = query.Execute().ToList();

            if (passagemPendenteEDI.CategoriaTag.Codigo > 0)
            {
                passagemPendenteEDI.CategoriaTag.Id = categorias
                    .FirstOrDefault(c => c.Codigo == passagemPendenteEDI.CategoriaTag.Codigo)?.CategoriaVeiculoId;
            }

            if (passagemPendenteEDI.CategoriaCobrada.Codigo > 0)
            {
                passagemPendenteEDI.CategoriaCobrada.Id = categorias
                    .FirstOrDefault(c => c.Codigo == passagemPendenteEDI.CategoriaCobrada.Codigo)?.CategoriaVeiculoId;
            }

            if (passagemPendenteEDI. CategoriaDac.Codigo > 0)
            {
                passagemPendenteEDI.CategoriaDac.Id = categorias
                    .FirstOrDefault(c => c.Codigo == passagemPendenteEDI.CategoriaDac.Codigo)?.CategoriaVeiculoId;
            }            
        }
    }
}
