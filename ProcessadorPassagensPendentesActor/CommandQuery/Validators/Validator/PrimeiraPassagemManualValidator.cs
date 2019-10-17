using System;
using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Transacoes.Domain.Enum;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Queries;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class PrimeiraPassagemManualValidator : Loggable
    {               
        private readonly ObterUltimaTransacaoPassagemArtespPorAdesaoIdQuery _ultimaTransacaoPassagemArtespPorAdesaoIdQuery;

        public PrimeiraPassagemManualValidator()
        {
            _ultimaTransacaoPassagemArtespPorAdesaoIdQuery = new ObterUltimaTransacaoPassagemArtespPorAdesaoIdQuery();             
        }




        public bool EhPrimeiraPassagemManual(long adesaoId, StatusPassagem statusPassagem)
        {
            try
            {
                if (statusPassagem == StatusPassagem.Manual)
                {                    
                    var ultimaTransacaoPassagem = DataBaseConnection.HandleExecution(_ultimaTransacaoPassagemArtespPorAdesaoIdQuery.Execute,adesaoId);

                    if (ultimaTransacaoPassagem != null && ultimaTransacaoPassagem.StatusPassagemId != (int)StatusPassagem.Manual)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex) // Seguimos o que está no legado, esse erro é tratado para não jogar a exceção para frente.
            {
                Log.Error("Ocorreu um erro no metodo VerificarPassagemManualParaNotificacao", ex);
                return false;
            }
        }
    }
}
