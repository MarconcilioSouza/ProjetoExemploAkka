using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;
using FluentValidation;
using ProcessadorPassagensActors.CommandQuery.Enums;

namespace ProcessadorPassagensActors.CommandQuery.Validators.RuleSet
{
    public class PassagemPendenteEdiRuleset : AbstractValidator<PassagemPendenteEDI>
    {
        public PassagemPendenteEdiRuleset()
        {
            RuleSet(PassagemPendenteEdiValidatorEnum.ValidarArquivoNulo.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => x != null);
            });

            RuleSet(PassagemPendenteEdiValidatorEnum.PossuiTransacaoAprovadaManualmente.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => (x.DetalheTrfAprovadoManualmenteId ?? 0) > 0);
            });

            RuleSet(PassagemPendenteEdiValidatorEnum.ValidarPossuiArquivoTrn.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => x.ArquivoTrnId != 0);
            });

            RuleSet(PassagemPendenteEdiValidatorEnum.ValidarPossuiArquivoTrf.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => x.ArquivoTrfId != 0);
            });

            RuleSet(PassagemPendenteEdiValidatorEnum.ValidarPossuiNumeroTag.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => x.Tag.OBUId != 0);
            });

            RuleSet(PassagemPendenteEdiValidatorEnum.ValidarPassagemManualComNumeroTagInvalida.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => !(x.Tag.OBUId == 0 && x.StatusPassagem == StatusPassagem.Manual));
            });

            RuleSet(PassagemPendenteEdiValidatorEnum.ValidarPassagemListaNela.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => x.StatusPassagem != StatusPassagem.ListaNela);
            });

            RuleSet(PassagemPendenteEdiValidatorEnum.PassagemIsenta.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => x.StatusPassagem != StatusPassagem.Isento);
            });

            RuleSet(PassagemPendenteEdiValidatorEnum.PassagemValorZerado.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => x.Valor > 0);
            });

            RuleSet(PassagemPendenteEdiValidatorEnum.ValidarPassagemIsentaComValor.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => x.StatusPassagem != StatusPassagem.Isento && x.Valor == 0);
            });

            RuleSet(PassagemPendenteEdiValidatorEnum.ValidarCategoria.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => (x.CategoriaUtilizada != null && x.CategoriaUtilizada.Id > 0));
            });

            RuleSet(PassagemPendenteEdiValidatorEnum.ValidarTag.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => x.Tag != null && x.Tag.Id > 0);
            });

            RuleSet(PassagemPendenteEdiValidatorEnum.ValidarAdesao.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => x.Adesao != null && x.Adesao.Id > 0);
            });

            RuleSet(PassagemPendenteEdiValidatorEnum.ValidarPistaPraca.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => (x.Pista != null && x.Pista.Id > 0 && x.Praca != null && x.Praca.Id > 0));
            });

            RuleSet(PassagemPendenteEdiValidatorEnum.PossuiAdesaoAtiva.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => x.Adesao.DataCancelamento == null || x.Adesao.DataCancelamento == DateTime.MinValue);
            });

            RuleSet(PassagemPendenteEdiValidatorEnum.ValidarEmissorTagId.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => x.IdEmissorTag == x.Adesao.Tag.EmissorId);
            });


            RuleSet(PassagemPendenteEdiValidatorEnum.ValidarTempoSlaEnvioPassagem.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(ValidarTempoSlaEnvioPassagem);
            });

        }


        private bool ValidarTempoSlaEnvioPassagem(PassagemPendenteEDI passagemPendenteEdi)
        {
            const int prazoMaximoEnvioTransacao = 60;
            var intervaloPassagem = (int)DateTime.Now.Subtract(passagemPendenteEdi.DataPassagem).TotalDays;

            return (passagemPendenteEdi.StatusCobranca != StatusCobranca.Normal || intervaloPassagem <= prazoMaximoEnvioTransacao);
        }
    }
}
