using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Framework.Backend.CommonQuery.Query;
using ConectCar.Framework.Domain.Model;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Queries;

namespace ProcessadorPassagensActors.CommandQuery.Validators
{
    public class PossuiSaldoSuficienteValidator
    {

        readonly DbConnectionDataSource _dataSourceConectSysReadOnly;
        readonly DbConnectionDataSource _dataSourceFallBack;
        readonly PassagemPendenteEDI _passagemPendenteEdi;

        public PossuiSaldoSuficienteValidator(DbConnectionDataSource dbSysReadOnly, DbConnectionDataSource dbSysFallBack, PassagemPendenteEDI passagemPendenteEdi)
        {
            _dataSourceConectSysReadOnly = dbSysReadOnly;
            _dataSourceFallBack = dbSysFallBack;
            _passagemPendenteEdi = passagemPendenteEdi;
        }

        public bool Validate()
        {
            if (_passagemPendenteEdi.Adesao.PlanoId == (int)PlanoDePagamento.PosPagoEmpresarial)
                return _passagemPendenteEdi.Adesao.Cliente.StatusId == (int)StatusCliente.Ativo;

            var obterSaldoAdesao = new ObterSaldoQuery(_dataSourceConectSysReadOnly);
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
            var queryConfiguracaoSistema = new ObterConfiguracaoSistemaQuery(true, _dataSourceConectSysReadOnly, _dataSourceFallBack);
            var configuracaoSaldoMinimoNegativo = queryConfiguracaoSistema.Execute(ConfiguracaoSistemaModel.SaldoMinimoNegativo).Valor.TryToDecimal();

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
            var obterLimiteDeCreditoConfiguracaoPlanoCliente = new ObterLimiteDeCreditoConfiguracaoPlanoClienteQuery(_dataSourceConectSysReadOnly);
            var limiteDeCredito = obterLimiteDeCreditoConfiguracaoPlanoCliente.Execute(_passagemPendenteEdi.Adesao.PlanoId);

            if (limiteDeCredito > 0)
                return limiteDeCredito;

            return 0;

        }

    }
}
