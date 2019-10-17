using ProcessadorPassagensActors.CommandQuery.Queries;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class HorarioPassagemAutomaticaValidator 
    {
        private readonly ObterCountTransacaoExistenteNoPeriodoQuery _countTransacaoExistenteNoPeriodoQuery;        

        public HorarioPassagemAutomaticaValidator()
        {
            _countTransacaoExistenteNoPeriodoQuery = new ObterCountTransacaoExistenteNoPeriodoQuery();
        }


        public MotivoNaoCompensado Validate(PassagemPendenteArtesp passagemPendenteArtesp)
        {            
            var retorno = DataBaseConnection.HandleExecution(_countTransacaoExistenteNoPeriodoQuery.Execute,passagemPendenteArtesp);
            if (retorno)            
                return MotivoNaoCompensado.PassagemAutomaticaHorarioIncompativel;            

            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }
    }
}
