using ConectCar.Framework.Domain.Model;
using ProcessadorPassagensActors.CommandQuery.Cache;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class OSAValidator
    {
        private readonly int _idOsaConfiguracao;       

        public OSAValidator()
        {
            var configuracaoSistema = ConfiguracaoSistemaCacheRepository.Obter(ConfiguracaoSistemaModel.OsaId);
            _idOsaConfiguracao = configuracaoSistema.Valor.TryToInt();            
        }

        public bool Validate(int idOsaPassagem)
        {            
            return (_idOsaConfiguracao == idOsaPassagem);
        }



    }
}
