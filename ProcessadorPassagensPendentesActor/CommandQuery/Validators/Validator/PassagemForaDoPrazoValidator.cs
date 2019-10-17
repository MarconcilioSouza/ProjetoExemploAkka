using System;
using ConectCar.Framework.Domain.Model;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.CommandQuery.Cache;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class PassagemForaDoPrazoValidator
    {
        private int DiasLimiteEstornoPassagem { get; }
        private PassagemAnteriorValidaDto _passagemAnteriorValidaDto;        
        private readonly ObterPassagemAnteriorValidaQuery _passagemAnteriorValidaQuery;

        public PassagemForaDoPrazoValidator()
        {
            _passagemAnteriorValidaQuery = new ObterPassagemAnteriorValidaQuery();
            var configuracao = ConfiguracaoSistemaCacheRepository.Obter(ConfiguracaoSistemaModel.QuantidadeDiasLimiteEstornoPassagem);

            if (configuracao != null)
                DiasLimiteEstornoPassagem = configuracao.Valor.TryToInt();
        }


        public void Init(PassagemPendenteArtesp passagem)
        {            
            var filter = new ObterPassagemAnteriorValidaCompletaFilter(passagem);
            _passagemAnteriorValidaDto = DataBaseConnection.HandleExecution(_passagemAnteriorValidaQuery.Execute,filter); 

        }

        public bool AchouPassagemAnterior => _passagemAnteriorValidaDto != null;

        public bool ValidatePrazo()
        {
            if (_passagemAnteriorValidaDto == null)
                return false;
            return DateTime.Now.Subtract(_passagemAnteriorValidaDto.Data).TotalDays > DiasLimiteEstornoPassagem;

        }

        public bool ValidateValor()
        {
            return _passagemAnteriorValidaDto.Valor != 0;
        }
    }
}

