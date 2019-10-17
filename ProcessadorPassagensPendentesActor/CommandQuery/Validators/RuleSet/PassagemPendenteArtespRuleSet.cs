using System;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;
using FluentValidation;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Resources;

namespace ProcessadorPassagensActors.CommandQuery.Validators.RuleSet
{
    public class PassagemPendenteArtespRuleSet : AbstractValidator<PassagemPendenteArtesp>
    {
        public PassagemPendenteArtespRuleSet()
        {
            //Validar Concessionária
            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarConcessionaria.ToString(), () =>
            {
                RuleFor(x => x.Conveniado).NotNull().WithMessage(x => string.Format(TransacaoAutorizacaoResource.ConcessionariaValidator, x.MensagemItemId));
                RuleFor(x => x.Conveniado.CodigoProtocoloArtesp).NotEmpty().WithMessage(x => string.Format(TransacaoAutorizacaoResource.ConcessionariaValidator, x.MensagemItemId));
            });

            //Validar Concessionária ConectSys
            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarConcessionariaConectSys.ToString(), () =>
            {
                RuleFor(x => x).Must(x => x.Conveniado != null && x.Conveniado.Id != null && x.Conveniado.Id > 0 && x.Conveniado.CodigoProtocoloArtesp > 0).WithMessage(x => string.Format(TransacaoAutorizacaoResource.ConcessionariaValidator, x.MensagemItemId));
            });

            //Verifica se passagem é de reenvio
            RuleSet(PassagemPendenteArtespValidatorEnum.EReenvio.ToString(), () =>
            {
                RuleFor(x => x.NumeroReenvio).GreaterThan(0);
            });

            //Valida se o motivo é válido para 1º Envio
            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarReenvioSemNumeroDeReenvio.ToString(), () =>
            {
                RuleFor(x => x.MotivoReenvio).Must(x => x == MotivoReenvio.NaoSeAplica);

            });

            //Valida se não possui MotivoReenvio no Reenvio
            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarMotivoReenvioNaoInformado.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => x.MotivoReenvio != MotivoReenvio.NaoSeAplica).WithMessage(x => string.Format(TransacaoAutorizacaoResource.ReenvioPassagemValidatorSemMotivoReenvio, x.MensagemItemId));

            });

            //Valida o motivo manual
            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarMotivoManualNaoInformado.ToString(), () =>
            {
                RuleFor(x => x).Must(x => (x.MotivoManual != MotivoManual.NaoSeAplica)).WithMessage(x => string.Format(TransacaoAutorizacaoResource.PassagemManualMotivo, x.MensagemItemId));
            });

            //Valida Motivo Manual Bloqueado
            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarPassagemManualSemValorBloqueadoComMotivoDiferenteDeBloqueado.ToString(), () =>
            {
                RuleFor(x => x).Must(x => (!(
                x.MotivoSemValor == MotivoSemValor.Bloqueado
                && x.MotivoManual != MotivoManual.PassagemBloqueada)))
                    .WithMessage(x => string.Format(TransacaoAutorizacaoResource.PassagemManualMotivoBloqueado, x.MensagemItemId));
            });


            //ValidarFlagPassagemAutomatica
            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarFlagPassagemAutomatica.ToString(), () =>
            {
                RuleFor(x => x).Must(x => (x.MotivoManual == MotivoManual.NaoSeAplica)).WithMessage(x => string.Format(TransacaoAutorizacaoResource.FlagPassagemAutomaticaValidator, x.MensagemItemId));
            });

            //ValidarMotivoSemValor
            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarMotivoSemValor.ToString(), () =>
            {
                RuleFor(x => x).Must(x => !(x.MotivoSemValor == MotivoSemValor.NaoSeAplica && x.Valor == 0)).WithMessage(x => string.Format(TransacaoAutorizacaoResource.MotivoSemValorValidator, x.MensagemItemId));
            });

            //ValidarCodigoPraca
            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarCodigoPraca.ToString(), () =>
            {
                RuleFor(x => x).Must(x => (x.Praca.CodigoPraca ?? 0) != 0);
            });

            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarIdentificadorPassagem.ToString(), () =>
           {
               RuleFor(model => model)
                   .Must(passagempendente => (passagempendente.ConveniadoPassagemId != 0))
                   .WithMessage(MotivoNaoCompensado.IdentificadorPassagemInvalido.ToString());
           });

            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarGrupoIsento.ToString(), () =>
            {
                RuleFor(model => model)
                    .Must(ValidarGrupoIsento).WithMessage(MotivoNaoCompensado.DadosInvalidos.ToString());
            });


            // validações abaixo eram da Passagem

            RuleSet(PassagemPendenteArtespValidatorEnum.EManual.ToString(), () =>
            {
                RuleFor(model => model)
                    .Must(passagem => (passagem.StatusPassagem == StatusPassagem.Manual));
            });

            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarExistenciaTag.ToString(), () =>
            {
                RuleFor(model => model)
                    .Must(passagem => ((passagem.Tag.Id ?? 0) > 0) && passagem.Tag.OBUId != 0);
            });

            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarPassagemManualSemTag.ToString(), () =>
            {
                RuleFor(model => model)
                    .Must(passagem => (passagem.Tag != null && passagem.Tag.OBUId != 0));
            });

            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarSePossuiAdesao.ToString(), () =>
            {
                RuleFor(model => model)
                    .Must(passagem =>
                            passagem.Adesao?.Tag != null 
                            && (
                                (passagem.Adesao.Id ?? 0) > 0
                            ) 
                            && (
                                passagem.Adesao.Tag.StatusTag != StatusTag.Inativa 
                                || 
                                (
                                    passagem.Adesao.DataCancelamento == null
                                    ||
                                    passagem.Adesao.DataCancelamento.Value >= passagem.DataPassagem
                                )
                            )
                    );
            });

            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarPraca.ToString(), () =>
            {
                RuleFor(model => model)
                    .Must(passagem => passagem.Praca != null && passagem.Praca.Id > 0);
            });


            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarPista.ToString(), () =>
            {
                RuleFor(model => model)
                    .Must(passagem => passagem.Pista != null && passagem.Pista.Id > 0);
            });

            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarPlaca.ToString(), () =>
            {
                RuleFor(model => model)
                    .Must(passagem => passagem.Adesao != null && passagem.Adesao.Veiculo != null && (passagem.Adesao.Veiculo.Placa == passagem.Placa));
            });

            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarCategoriaPassagem.ToString(), () =>
            {
                RuleFor(model => model)
                    .Must(passagem => (passagem.CategoriaCobrada != null && passagem.CategoriaCobrada.Codigo > 0) || (passagem.CategoriaTag != null && passagem.CategoriaTag.Codigo > 0));
            });

            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarCategoriaUtilizada.ToString(), () =>
            {
                RuleFor(model => model)
                    .Must(passagem => (passagem.CategoriaUtilizada != null && passagem.CategoriaUtilizada.Codigo > 0));
            });

            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarVeiculoIsento.ToString(), () =>
            {
                RuleFor(model => model)
                    .Must(passagem => (passagem.Valor == 0 &&
                                       (passagem.Tag.Grupo == Grupo.Isento || passagem.Tag.Grupo == Grupo.IsentoPelaArtesp) &&
                                       passagem.MotivoSemValor != MotivoSemValor.CobrancaIndevida));
            });
            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarGrupoIsentoDadosInvalidos.ToString(), () =>
            {
                RuleFor(model => model)
                    .Must(passagem => ((passagem.MotivoSemValor == MotivoSemValor.GrupoIsento ||
                    passagem.MotivoSemValor == MotivoSemValor.IsentoConcessionaria) 
                    && passagem.Valor > 0));
            });
            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarGrupoIsentoMotivoSemValorNaoInformado.ToString(), () =>
            {
                RuleFor(model => model)
                    .Must(passagem => ((passagem.MotivoSemValor == MotivoSemValor.Bloqueado ||
                    passagem.MotivoSemValor == MotivoSemValor.TagMalInstalado ||
                    passagem.MotivoSemValor == MotivoSemValor.TagViolado) && passagem.Valor > 0));
            });
            RuleSet(PassagemPendenteArtespValidatorEnum.ValidarConcessionariaAposCarregarDados.ToString(), () =>
            {
                RuleFor(model => model)
                    .Must(passagem => (passagem.Conveniado.Id == null || passagem.Conveniado.Id == 0));
            });
        }


        public bool ValidarGrupoIsento(PassagemPendenteArtesp passagemPendente)
        {
            var motivoSemValor = passagemPendente.MotivoSemValor;

            if (motivoSemValor == MotivoSemValor.GrupoIsento || motivoSemValor == MotivoSemValor.IsentoConcessionaria)
            {
                if (passagemPendente.Valor > 0)
                    return true;

                return false;
            }

            if (motivoSemValor == MotivoSemValor.Bloqueado
                || motivoSemValor == MotivoSemValor.TagMalInstalado
                || motivoSemValor == MotivoSemValor.TagViolado)
            {
                if (passagemPendente.Valor > 0)
                    return true;
            }

            return false;
        }
    }
}
