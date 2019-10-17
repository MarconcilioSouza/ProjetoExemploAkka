using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Queries;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class ReenvioInvalidoValidator
    {
        private readonly ObterCountPassagemForaSequencia _countPassagemForaSequencia;

        public ReenvioInvalidoValidator()
        {
            _countPassagemForaSequencia = new ObterCountPassagemForaSequencia();
        }
        public bool ValidateForaSequencia(PassagemPendenteArtesp passagem)
        {
            var passagemForaDeSequencia = DataBaseConnection.HandleExecution(_countPassagemForaSequencia.Execute, passagem);
            return passagemForaDeSequencia;
        }
    }
}
