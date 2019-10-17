using System.Data;
using System.Linq;
using AutoMapper;
using ConectCar.Transacoes.Domain.ValueObject;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ConectCar.Cadastros.Domain.Dto;
using System.Collections.Generic;
using ProcessadorPassagensActors.CommandQuery.Cache;
using ProcessadorPassagensActors.Infrastructure;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class CriarPassagemCompletaQuery : IQuery<PassagemPendenteArtesp, PassagemPendenteArtesp>
    {
        private PassagemPendenteArtesp _passagemPendenteArtesp;


        public CriarPassagemCompletaQuery()
        {
        }

        public PassagemPendenteArtesp Execute(PassagemPendenteArtesp passagemPendenteArtesp)
        {
            _passagemPendenteArtesp = passagemPendenteArtesp;

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var tagAdesaoDto = conn.Query<TagAdesaoDto>(
                    "[dbo].[spObterTagAdesao]",
                    new
                    {
                        passagemPendenteArtesp.Tag.OBUId,
                        DataReferenciaCancelamento = passagemPendenteArtesp.DataPassagem
                    },
                    commandTimeout: TimeHelper.CommandTimeOut,
                    commandType: CommandType.StoredProcedure).FirstOrDefault();                

                if (tagAdesaoDto != null)
                    Mapper.Map(tagAdesaoDto, passagemPendenteArtesp);
                
                PreencherPistaPraca();

                PreencherCategoriaVeiculo();

                return passagemPendenteArtesp;
            }
                
        }
        
        private void PreencherPistaPraca()
        {
            var pistaPracas = PistaPracaConveniadoArtespCacheRepository.Listar();

            if (pistaPracas != null && pistaPracas.Any())
            {
                var pistaPraca = pistaPracas.FirstOrDefault(x => 
                x.CodigoProtocoloArtesp == _passagemPendenteArtesp.Conveniado.CodigoProtocoloArtesp &&
                x.CodigoPraca == _passagemPendenteArtesp.Praca.CodigoPraca &&
                x.CodigoPista == _passagemPendenteArtesp.Pista.CodigoPista);
                if (pistaPraca != null)
                {
                    Mapper.Map(pistaPraca, _passagemPendenteArtesp);
                }
                else
                {
                    // verificar se existe praça
                    var codigoPista = _passagemPendenteArtesp.Pista.CodigoPista;
                    var pracaPistaPraca = pistaPracas.FirstOrDefault(x =>
                    x.CodigoProtocoloArtesp == _passagemPendenteArtesp.Conveniado.CodigoProtocoloArtesp &&
                    x.CodigoPraca == _passagemPendenteArtesp.Praca.CodigoPraca);
                    if (pracaPistaPraca != null)
                    {
                        Mapper.Map(pracaPistaPraca, _passagemPendenteArtesp);
                        _passagemPendenteArtesp.Pista.Id = null;
                        _passagemPendenteArtesp.Pista.CodigoPista = codigoPista;
                    }
                    else
                    {
                        var conveniadoPistaPraca = pistaPracas.FirstOrDefault(x=> x.CodigoProtocoloArtesp == _passagemPendenteArtesp.Conveniado.CodigoProtocoloArtesp);
                        codigoPista = _passagemPendenteArtesp.Pista.CodigoPista;
                        var codigoPraca = _passagemPendenteArtesp.Praca.CodigoPraca;

                        Mapper.Map(conveniadoPistaPraca, _passagemPendenteArtesp);

                        _passagemPendenteArtesp.Pista.Id = null;
                        _passagemPendenteArtesp.Pista.CodigoPista = codigoPista;
                        _passagemPendenteArtesp.Praca.Id = null;
                        _passagemPendenteArtesp.Praca.CodigoPraca = codigoPraca;
                    }
                    // Mapper praca e conveniado
                }
            }
        }

        private void PreencherCategoriaVeiculo()
        {
            var categorias = CategoriaVeiculoCacheRepository.Listar();

            if (_passagemPendenteArtesp.CategoriaTag.Codigo > 0)
            {
                _passagemPendenteArtesp.CategoriaTag.Id = categorias
                    .FirstOrDefault(c => c.Codigo == _passagemPendenteArtesp.CategoriaTag.Codigo)?.CategoriaVeiculoId;
            }

            if (_passagemPendenteArtesp.CategoriaCobrada.Codigo > 0)
            {
                _passagemPendenteArtesp.CategoriaCobrada.Id = categorias
                    .FirstOrDefault(c => c.Codigo == _passagemPendenteArtesp.CategoriaCobrada.Codigo)?.CategoriaVeiculoId;
            }

            if (_passagemPendenteArtesp.CategoriaDetectada.Codigo > 0)
            {
                _passagemPendenteArtesp.CategoriaDetectada.Id = categorias
                    .FirstOrDefault(c => c.Codigo == _passagemPendenteArtesp.CategoriaDetectada.Codigo)?.CategoriaVeiculoId;
            }
        }


    }
}
