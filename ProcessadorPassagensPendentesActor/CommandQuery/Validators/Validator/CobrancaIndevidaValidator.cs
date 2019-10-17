using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Queries;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class CobrancaIndevidaValidator 
    {
        private ObterTransacaoRecusadaPorPassagemIdQuery _transacaoRecusadaPorPassagemIdQuery;
        private ObterIdPassagemImediatamenteAnteriorQuery _idPassagemImediatamenteAnteriorQuery;

        public CobrancaIndevidaValidator()
        {
            _transacaoRecusadaPorPassagemIdQuery = new ObterTransacaoRecusadaPorPassagemIdQuery();
            _idPassagemImediatamenteAnteriorQuery = new ObterIdPassagemImediatamenteAnteriorQuery();            
        }

        public MotivoNaoCompensado Validate(PassagemPendenteArtesp passagemPendenteArtesp)
        {
            var motivoNaoCompensado = MotivoNaoCompensado.SemMotivoNaoCompensado;

            if (passagemPendenteArtesp.NumeroReenvio <= 0)
                return motivoNaoCompensado;

            
            var passagemOriginalId = DataBaseConnection.HandleExecution(_idPassagemImediatamenteAnteriorQuery.Execute,passagemPendenteArtesp);


            if (passagemOriginalId == null && passagemPendenteArtesp.MotivoSemValor == MotivoSemValor.CobrancaIndevida)                            
                return MotivoNaoCompensado.DadosInvalidos;
            

            if(passagemOriginalId != null && passagemOriginalId > 0)
            {                
                var transacaoRecusada = DataBaseConnection.HandleExecution(_transacaoRecusadaPorPassagemIdQuery.Execute,passagemOriginalId.Value);

                if (transacaoRecusada!=null  && passagemPendenteArtesp.MotivoSemValor == MotivoSemValor.CobrancaIndevida)                
                    return MotivoNaoCompensado.Isento;
                
            }

            return motivoNaoCompensado;
        }

    }
}
