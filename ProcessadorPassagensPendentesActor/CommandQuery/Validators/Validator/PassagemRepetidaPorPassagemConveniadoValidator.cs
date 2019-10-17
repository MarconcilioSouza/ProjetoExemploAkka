using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Queries;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class PassagemRepetidaPorPassagemConveniadoValidator
    {        
        private readonly ObterPassagemPorCodigoPassagemConveniadoQuery _passagemPorCodigoPassagemConveniadoQuery;

        public PassagemRepetidaPorPassagemConveniadoValidator()
        {
            _passagemPorCodigoPassagemConveniadoQuery = new ObterPassagemPorCodigoPassagemConveniadoQuery();            
        }


        public bool Validate(PassagemPendenteArtesp passagemPendenteArtesp)
        {            
            var passagemRepetida =
                DataBaseConnection.HandleExecution(_passagemPorCodigoPassagemConveniadoQuery.Execute,passagemPendenteArtesp);

            if (passagemRepetida?.NumeroReenvio >= passagemPendenteArtesp.NumeroReenvio)
            {
                return true;
            }

            return false;
        }
    }
}
