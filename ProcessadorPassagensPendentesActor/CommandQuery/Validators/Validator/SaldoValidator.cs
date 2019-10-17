using ConectCar.Framework.Domain.Model;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Cache;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Queries;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class SaldoValidator
    {
        private readonly TagPracaBloqueadoLadoMensageriaValidator _tagPracaBloqueadoLadoMensageriaValidator;
        private readonly ObterTransacaoPassagemIdAnteriorValidaQuery _transacaoPassagemIdAnteriorValidaQuery;
        private readonly ObterSaldoQuery _saldoQuery;

        public SaldoValidator()
        {
            _tagPracaBloqueadoLadoMensageriaValidator = new TagPracaBloqueadoLadoMensageriaValidator();
            _transacaoPassagemIdAnteriorValidaQuery = new ObterTransacaoPassagemIdAnteriorValidaQuery();
            _saldoQuery = new ObterSaldoQuery();
        }

        public MotivoNaoCompensado Validate(PassagemPendenteArtesp passagemPendenteArtesp)
        {
            var motivoNaoCompensado = MotivoNaoCompensado.SemMotivoNaoCompensado;

            if (passagemPendenteArtesp.Adesao.DataCancelamento != null)
                return motivoNaoCompensado;

            var passagemIsenta = PassagemIsenta(passagemPendenteArtesp);
            if (passagemIsenta)
                return motivoNaoCompensado;

            var possuiSaldoSuficiente = VerificarSaldo(passagemPendenteArtesp);

            if (!possuiSaldoSuficiente)
                return VerificarBloqueioPraca(passagemPendenteArtesp);

            return motivoNaoCompensado;


        }

        private MotivoNaoCompensado VerificarBloqueioPraca(PassagemPendenteArtesp passagemPendenteArtesp)
        {
            _tagPracaBloqueadoLadoMensageriaValidator.Init(passagemPendenteArtesp);


            var pracaBloqueada = _tagPracaBloqueadoLadoMensageriaValidator.ValidatePracaBloqueada(passagemPendenteArtesp.Praca.CodigoPraca ?? 0);
            if (pracaBloqueada)
                return MotivoNaoCompensado.PracaBloqueada;

            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        private bool VerificarSaldo(PassagemPendenteArtesp passagemPendenteArtesp)
        {
            if (passagemPendenteArtesp.NumeroReenvio > 0)
            {

                long? transacaoAnteriorId;
                transacaoAnteriorId = DataBaseConnection.HandleExecution(_transacaoPassagemIdAnteriorValidaQuery.Execute, passagemPendenteArtesp);

                if (transacaoAnteriorId > 0)
                    return true;

            }

            if (passagemPendenteArtesp.Adesao.Plano == PlanoDePagamento.PosPagoEmpresarial)
                return passagemPendenteArtesp.Adesao.Cliente.Status == StatusCliente.Ativo;

            var saldo = ObterSaldo(passagemPendenteArtesp.Adesao.SaldoId);

            if (passagemPendenteArtesp.Adesao.Cliente.UltimaCobrancaPaga)
            {
                var limiteCredito = ObterLimiteDeCredito(passagemPendenteArtesp.Adesao.Plano);
                if (saldo + limiteCredito < passagemPendenteArtesp.Valor)
                    return false;
            }
            else
            {
                if (saldo < passagemPendenteArtesp.Valor)
                    return false;
            }

            return true;
        }

        private bool PassagemIsenta(PassagemPendenteArtesp passagemPendenteArtesp)
        {
            return (passagemPendenteArtesp.MotivoSemValor == MotivoSemValor.GrupoIsento
                || passagemPendenteArtesp.MotivoSemValor == MotivoSemValor.IsentoConcessionaria);
        }

        private decimal ObterLimiteDeCredito(PlanoDePagamento planoDePagamento)
        {
            switch (planoDePagamento)
            {
                case PlanoDePagamento.PrePago:
                    return 0;

                case PlanoDePagamento.RecargaAutomatica:
                case PlanoDePagamento.PlanoMensalidade:
                case PlanoDePagamento.ValorVariavel:
                    return ObterSaldoMinimoNegativo() * -1;

                case PlanoDePagamento.PrePagoEmpresarial:
                case PlanoDePagamento.PosPagoEmpresarial:
                    return 0;

            }

            throw new DomainException("Plano não configurado.");
        }

        private decimal ObterSaldoMinimoNegativo()
        {
            var configuracao = ConfiguracaoSistemaCacheRepository.Obter(ConfiguracaoSistemaModel.SaldoMinimoNegativo);
            return configuracao.Valor.TryToDecimal();
        }

        private decimal ObterSaldo(int saldoId)
        {
            decimal saldo = 0;
            saldo = DataBaseConnection.HandleExecution(_saldoQuery.Execute, saldoId);
            return saldo;
        }

    }
}
