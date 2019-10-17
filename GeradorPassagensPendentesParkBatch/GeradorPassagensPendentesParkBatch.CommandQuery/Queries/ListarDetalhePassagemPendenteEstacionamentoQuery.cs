using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using Dapper;
using GeradorPassagensPendentesParkBatch.CommandQuery.Messages;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ConectCar.Transacoes.Domain.Dto;

namespace GeradorPassagensPendentesParkBatch.CommandQuery.Queries
{
    public class ListarDetalhePassagemPendenteEstacionamentoQuery : DbConnectionQueryBase<ListarDetalhePassagemPendenteEstacionamentoFilter, IEnumerable<PassagemPendenteParkMessage>>
    {
        public ListarDetalhePassagemPendenteEstacionamentoQuery(DbConnectionDataSource dataSource) : base(dataSource)
        {
        }

        public override IEnumerable<PassagemPendenteParkMessage> Execute(ListarDetalhePassagemPendenteEstacionamentoFilter filter)
        {
            var resultado = DataSource.Connection.Query<DetalhePassagemPendenteEstacionamentoMessage>(
                "spObterPassagemPendenteEstacionamento",
                new
                {
                    filter.QuantidadeMaximaPassagens
                },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );

            var result = resultado
                .GroupBy(x => new
                {
                    x.RegistroTransacaoId,
                    x.CodigoConveniado,
                    x.DataHoraEntrada,
                    x.DataHoraTransacao,
                    x.Mensalista,
                    x.MotivoAtrasoTransmissao,
                    x.MotivoDesconto,
                    x.Pista,
                    x.Praca,
                    x.Tag,
                    x.TempoPermanencia,
                    x.TipoTransacao,
                    x.ValorCobrado,
                    x.ValorDesconto,
                    x.Ticket,
                })
                .Select(x => new PassagemPendenteParkMessage
                {
                    RegistroTransacaoId = x.Key.RegistroTransacaoId,
                    CodigoConveniado = x.Key.CodigoConveniado,
                    DataHoraEntrada = x.Key.DataHoraEntrada,
                    DataHoraTransacao = x.Key.DataHoraTransacao,
                    Mensalista = x.Key.Mensalista,
                    MotivoAtrasoTransmissao = x.Key.MotivoAtrasoTransmissao,
                    MotivoDesconto = x.Key.MotivoDesconto,
                    Pista = x.Key.Pista,
                    Praca = x.Key.Praca,
                    Tag = x.Key.Tag,
                    TempoPermanencia = x.Key.TempoPermanencia,
                    TipoTransacao = x.Key.TipoTransacao,
                    ValorCobrado = x.Key.ValorCobrado,
                    ValorDesconto = x.Key.ValorDesconto,
                    Ticket = x.Key.Ticket,
                    Detalhes = x.Select(d => new DetalhePassagemPendenteEstacionamentoDto
                    {
                        CodigoPista = d.PistaDetalhe,
                        CodigoPraca = d.PracaDetalhe,
                        Data = d.DataHoraPassagem,
                    }).ToList(),
                });

            return result;
        }
    }
}