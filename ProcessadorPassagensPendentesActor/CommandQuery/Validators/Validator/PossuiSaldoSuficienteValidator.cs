using ConectCar.Framework.Domain.Model;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensActors.CommandQuery.Cache;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Queries;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class PossuiSaldoSuficienteValidator
    {
        readonly PassagemPendenteEDI _passagemPendenteEdi;

        public PossuiSaldoSuficienteValidator(PassagemPendenteEDI passagemPendenteEdi)
        {
            _passagemPendenteEdi = passagemPendenteEdi;
        }

        public bool Validate()
        {
            if (_passagemPendenteEdi.Adesao.PlanoId == (int)PlanoDePagamento.PosPagoEmpresarial)
                return _passagemPendenteEdi.Adesao.Cliente.StatusId == (int)StatusCliente.Ativo;

            var obterSaldoAdesao = new ObterSaldoQuery();
            var saldo = obterSaldoAdesao.Execute(_passagemPendenteEdi.Adesao.SaldoId);

            var limiteCredito = ObterLimiteDeCredito();
            
            if (_passagemPendenteEdi.Adesao.Cliente.UltimaCobrancaPaga)
            {
                if (saldo + limiteCredito < _passagemPendenteEdi.Valor)
                    return false;
            }
            else
            {
                if (saldo < _passagemPendenteEdi.Valor)
                    return false;
            }

            return true;

        }

        private decimal ObterLimiteDeCredito()
        {
            var configuracaoSaldoMinimoNegativo = ConfiguracaoSistemaCacheRepository.Obter(ConfiguracaoSistemaModel.SaldoMinimoNegativo).Valor.TryToDecimal();

            switch (_passagemPendenteEdi.Adesao.Plano)
            {
                case PlanoDePagamento.PrePago:
                    return 0;

                case PlanoDePagamento.RecargaAutomatica:
                case PlanoDePagamento.PlanoMensalidade:
                case PlanoDePagamento.ValorVariavel:
                    return configuracaoSaldoMinimoNegativo * -1;

                case PlanoDePagamento.PrePagoEmpresarial:
                case PlanoDePagamento.PosPagoEmpresarial:
                    return ObterLimiteDeCreditoConfiguracaoPlanoCliente();

            }
            throw new EdiDomainException("Plano não configurado.", _passagemPendenteEdi);
        }

        private decimal ObterLimiteDeCreditoConfiguracaoPlanoCliente()
        {
            var obterLimiteDeCreditoConfiguracaoPlanoCliente = new ObterLimiteDeCreditoConfiguracaoPlanoClienteQuery();
            var limiteDeCredito = obterLimiteDeCreditoConfiguracaoPlanoCliente.Execute(_passagemPendenteEdi.Adesao.PlanoId);

            if (limiteDeCredito > 0)
                return limiteDeCredito;

            return 0;

        }

    }
}
