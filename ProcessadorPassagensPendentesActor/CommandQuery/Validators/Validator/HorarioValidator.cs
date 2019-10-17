using System;
using ConectCar.Framework.Domain.Model;
using ProcessadorPassagensActors.CommandQuery.Cache;
using ConectCar.Framework.Infrastructure.Log;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class HorarioValidator : Loggable
    {
        private readonly int _tempoAceitePassagem;

        public HorarioValidator()
        {           
            var configuracao = ConfiguracaoSistemaCacheRepository.Obter(ConfiguracaoSistemaModel.TempoAceitePassagem);

            if (configuracao != null)
                _tempoAceitePassagem = configuracao.Valor.TryToInt();
        }

        public bool Validate(DateTime dataHora, DateTime dataCriacao)
        {            
            return !(dataHora < DateTime.Now.AddDays(_tempoAceitePassagem * -1) || dataHora > dataCriacao);
        }

    }
}
