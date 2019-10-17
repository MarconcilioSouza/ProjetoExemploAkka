using AutoMapper;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Commands;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Filter;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using ProcessadorPassagensProcessadasApi.CommandQuery.Extension;
using System.Collections.Generic;
using System.Linq;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands
{
    public class SalvarPassagensReprovadasSysEdiCommand : DbConnectionCommandBase<PassagemReprovadaEdiSysArgs>
    {
        public SalvarPassagensReprovadasSysEdiCommand(DbConnectionDataSource dataSource) : base(dataSource)
        {
        }

        public override void Execute(PassagemReprovadaEdiSysArgs args)
        {
            var detalheTrfRecusado = args.DetalheTRFRecusado?.Where(x => x != null);
            var veiculo = args.Veiculo?.Where(x => x != null);
            var transacaoRecusadaParceiro = args.TransacaoRecusadaParceiro?.Where(x => x != null);

            var detalheTrfRecusadoLoteStaging = Mapper.Map<IEnumerable<DetalheTRFRecusadoDto>, IEnumerable<DetalheTRFRecusadoLoteStaging>>(detalheTrfRecusado);
            var veiculoLoteStaging = Mapper.Map<IEnumerable<VeiculoDto>, IEnumerable<VeiculoLoteStaging>>(veiculo);
            var transacaoRecusadaParceiroLoteStaging = Mapper.Map<IEnumerable<TransacaoRecusadaParceiroEdiDto>, IEnumerable<TransacaoRecusadaParceiroLoteStaging>>(transacaoRecusadaParceiro);

            if (detalheTrfRecusadoLoteStaging != null)
            { 
                DataSource.Connection.BulkInsertTransacoes(detalheTrfRecusadoLoteStaging.ToList(), "DetalheTRFRecusadoLoteStaging");
            }
            
            if (veiculoLoteStaging != null)
            { 
                DataSource.Connection.BulkInsertTransacoes(veiculoLoteStaging.ToList(), "VeiculoLoteStaging");
            }

            if (transacaoRecusadaParceiroLoteStaging != null)
            { 
                DataSource.Connection.BulkInsertTransacoes(transacaoRecusadaParceiroLoteStaging.ToList(), "TransacaoRecusadaParceiroLoteStaging");
            }
        }
    }
}
