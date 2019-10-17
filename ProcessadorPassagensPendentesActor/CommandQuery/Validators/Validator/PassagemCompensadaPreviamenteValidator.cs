using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class PassagemCompensadaPreviamenteValidator: Loggable
    {                 
        private readonly ObterTransacaoPassagemIdAnteriorValidaQuery _transacaoPassagemIdAnteriorValidaQuery;
        private readonly ObterTransacaoPassagemPorTransacaoIdQuery _transacaoPassagemPorTransacaoIdQuery;
        private readonly ObterPassagemAnteriorValidaQuery _passagemAnteriorValidaQuery;

        public PassagemCompensadaPreviamenteValidator()
        {
            
            
            _transacaoPassagemIdAnteriorValidaQuery = new ObterTransacaoPassagemIdAnteriorValidaQuery();
            _transacaoPassagemPorTransacaoIdQuery = new ObterTransacaoPassagemPorTransacaoIdQuery();
            _passagemAnteriorValidaQuery = new ObterPassagemAnteriorValidaQuery();
        }

        public void Validate(PassagemPendenteArtesp passagemPendenteArtesp)
        {           
            if (passagemPendenteArtesp.NumeroReenvio > 0)
            {
                passagemPendenteArtesp.TransacaoPassagemIdAnterior = DataBaseConnection.HandleExecution(_transacaoPassagemIdAnteriorValidaQuery.Execute,passagemPendenteArtesp);

                if (passagemPendenteArtesp.TransacaoPassagemIdAnterior > 0)
                {                    
                    var transacaoOriginal = DataBaseConnection.HandleExecution(_transacaoPassagemPorTransacaoIdQuery.Execute,passagemPendenteArtesp.TransacaoPassagemIdAnterior);

                    var passagemAnteriorFilter = new ObterPassagemAnteriorValidaCompletaFilter(passagemPendenteArtesp.Conveniado.Id ?? 0,
                            passagemPendenteArtesp.NumeroReenvio,
                            passagemPendenteArtesp.ConveniadoPassagemId,
                            passagemPendenteArtesp.Conveniado.CodigoProtocoloArtesp);

                    var passagemOriginal = DataBaseConnection.HandleExecution(_passagemAnteriorValidaQuery.Execute,passagemAnteriorFilter);

                    if (passagemOriginal != null && passagemPendenteArtesp.NumeroReenvio > passagemOriginal.Reenvio &&
                        PassagemJaProcessadaAnteriormente(passagemOriginal, passagemPendenteArtesp) && transacaoOriginal != null)
                    {
                        throw new PassagemInvalidaNoSysException(
                            ResultadoPassagem.CompensadoPreviamente,
                            passagemPendenteArtesp,
                            MotivoNaoCompensado.SemMotivoNaoCompensado,
                            transacaoOriginal.Id ?? 0,
                            passagemOriginal.Valor,
                            transacaoOriginal.DataRepasse,
                            passagemOriginal.PassagemId);
                    }
                }
            }
        }


        public bool PassagemJaProcessadaAnteriormente(PassagemAnteriorValidaDto passagemAnterior, PassagemPendenteArtesp passagemPendenteArtesp)
        {
            return passagemAnterior.CodigoPassagemConveniado == passagemPendenteArtesp.ConveniadoPassagemId
                && passagemAnterior.Valor == passagemPendenteArtesp.Valor &&
                   passagemAnterior.OBUId == passagemPendenteArtesp.Tag.OBUId &&
                   passagemAnterior.Data == passagemPendenteArtesp.DataPassagem &&
                   passagemAnterior.CodigoPraca == passagemPendenteArtesp.Praca.CodigoPraca &&
                   passagemAnterior.CodigoPista == passagemPendenteArtesp.Pista.CodigoPista;
        }
    }
}
