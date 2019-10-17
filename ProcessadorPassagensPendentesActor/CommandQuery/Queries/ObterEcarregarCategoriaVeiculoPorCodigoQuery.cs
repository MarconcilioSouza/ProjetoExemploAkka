using System.Linq;
using ConectCar.Comercial.Cliente.Adesao.Backend.CommonQuery.Query;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterEcarregarCategoriaVeiculoPorCodigoQuery
    {
        public void Execute(PassagemPendenteArtesp passagemPendenteArtesp,
            DbConnectionDataSource dbSysReadOnly)
        {
            var query = new ObterCategoriaVeiculoQuery(true, dbSysReadOnly);
            var categorias = query.Execute().ToList();

            if (passagemPendenteArtesp.CategoriaTag.Codigo > 0)
            {
                passagemPendenteArtesp.CategoriaTag.Id = categorias
                    .FirstOrDefault(c => c.Codigo == passagemPendenteArtesp.CategoriaTag.Codigo)?.CategoriaVeiculoId;
            }

            if (passagemPendenteArtesp.CategoriaCobrada.Codigo > 0)
            {
                passagemPendenteArtesp.CategoriaCobrada.Id = categorias
                    .FirstOrDefault(c => c.Codigo == passagemPendenteArtesp.CategoriaCobrada.Codigo)?.CategoriaVeiculoId;
            }

            if (passagemPendenteArtesp.CategoriaDetectada.Codigo > 0)
            {
                passagemPendenteArtesp.CategoriaDetectada.Id = categorias
                    .FirstOrDefault(c => c.Codigo == passagemPendenteArtesp.CategoriaDetectada.Codigo)?.CategoriaVeiculoId;
            }
        }
    }
}
