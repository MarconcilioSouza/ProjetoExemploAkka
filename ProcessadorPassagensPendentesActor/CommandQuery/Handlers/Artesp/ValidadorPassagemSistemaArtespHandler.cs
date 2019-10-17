using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.CommandQuery.Validators.Validator;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp
{
    public class ValidadorPassagemSistemaArtespHandler : Loggable
    {
        
        readonly GenericValidator<PassagemPendenteArtesp> _validator;
        private TagPracaBloqueadoLadoMensageriaValidator _tagPracaBloqueadoLadoMensageriaValidator;
        private PassagemForaDoPrazoValidator _passagemForaDoPrazoValidator;
        private ReenvioInvalidoValidator _reenvioInvalidoValidator;
        private PassagemRepetidaPorPassagemConveniadoValidator _passagemRepetidaPorPassagemConveniadoValidator;
        private CobrancaIndevidaValidator _cobrancaIndevidaValidator;
        private TagGrupoPracaValidator _tagGrupoPracaValidator;
        private HorarioPassagemManualValidator _horarioPassagemManualValidator;
        private HorarioPassagemAutomaticaValidator _horarioPassagemAutomaticaValidator;
        private GrupoIsentoTagValidator _grupoIsentoTagValidator;
        private ValorTarifaValidator _valorTarifaValidator;
        private SaldoValidator _saldoValidator;
        private PrimeiraPassagemManualValidator _primeiraPassagemManualValidator;
        private TransacaoRepetidaValidator _transacaoRepetidaValidator;

        public ValidadorPassagemSistemaArtespHandler()
        {            
            _validator = new GenericValidator<PassagemPendenteArtesp>();
            _tagPracaBloqueadoLadoMensageriaValidator = new TagPracaBloqueadoLadoMensageriaValidator();
            _passagemForaDoPrazoValidator = new PassagemForaDoPrazoValidator();
            _reenvioInvalidoValidator = new ReenvioInvalidoValidator();
            _passagemRepetidaPorPassagemConveniadoValidator = new PassagemRepetidaPorPassagemConveniadoValidator();
            _cobrancaIndevidaValidator = new CobrancaIndevidaValidator();
            _tagGrupoPracaValidator = new TagGrupoPracaValidator();
            _horarioPassagemManualValidator = new HorarioPassagemManualValidator();
            _horarioPassagemAutomaticaValidator = new HorarioPassagemAutomaticaValidator();
            _grupoIsentoTagValidator = new GrupoIsentoTagValidator();
            _valorTarifaValidator = new ValorTarifaValidator();
            _saldoValidator = new SaldoValidator();
            _primeiraPassagemManualValidator = new PrimeiraPassagemManualValidator();
            _transacaoRepetidaValidator = new TransacaoRepetidaValidator();
        }

        /// <summary>
        /// Executa o processamento de Passagens
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ValidadorPassagemSistemaResponse Execute(ValidadorPassagemSistemaRequest request)
        {
            var motivoNaoCompensado = MotivoNaoCompensado.SemMotivoNaoCompensado;

            if(motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = VerificarTagPracaBloqueada(request.PassagemPendenteArtesp.MensagemItemId, request.PassagemPendenteArtesp.Praca.CodigoPraca ?? 0);

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarReenvio(request);

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarPassagemJaExistentePorCodigoPassagem(request);

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarCobrancaIndevida(request);

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarTagGrupoPraca(request);

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarTransacaoRepetida(request);

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarHorarioPassagem(request);

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarGrupoIsentoTag(request);

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarVeiculoIsento(request);

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarValorTarifa(request);

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarSaldo(request);

            PreencherPrimeiraPassagemManual(request);

            //Retorna o objeto pelo response...
            var response = new ValidadorPassagemSistemaResponse();
            response.PassagemPendenteArtesp = request.PassagemPendenteArtesp;
            response.MotivoNaoCompensado = motivoNaoCompensado;

            return response;
        }

        public MotivoNaoCompensado VerificarTagPracaBloqueada(long mensagemItemId, int codigoPraca)
        {
            Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: ValidadorPassagemSistemaHandler | validarTagPracaBloqueada");
            var validarTagPracaBloqueada = new TagPracaBloqueadoLadoMensageriaValidator();
            return _tagPracaBloqueadoLadoMensageriaValidator.ValidateTagBloqueada(codigoPraca);
        }


        public MotivoNaoCompensado ValidarPassagemJaExistentePorCodigoPassagem(ValidadorPassagemSistemaRequest request)
        {
            Log.Debug($"Passagem ID: {request.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemSistemaHandler |  Validar Passagem Já Existente Por Código Passagem");            
            if (_passagemRepetidaPorPassagemConveniadoValidator.Validate(request.PassagemPendenteArtesp))
            {
                return MotivoNaoCompensado.ReenvioInvalido;
            }
            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        private MotivoNaoCompensado ValidarReenvio(ValidadorPassagemSistemaRequest request)
        {
            var motivoNaoCompensado = MotivoNaoCompensado.SemMotivoNaoCompensado;

            if (_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.EReenvio.ToString()))
            {
                if(motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                    motivoNaoCompensado = ValidarPassagemEnviadaForaDoPrazo(request);

                if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                    motivoNaoCompensado = ValidarReenvioInvalido(request);
            }

            return motivoNaoCompensado;
        }

        private void PreencherPrimeiraPassagemManual(ValidadorPassagemSistemaRequest request)
        {
            Log.Debug($"Passagem ID: {request.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemSistemaHandler |  Verificar Existência Primeira Passagem Manual");            
            request.PassagemPendenteArtesp.PrimeiraPassagemManual = _primeiraPassagemManualValidator.EhPrimeiraPassagemManual(request.PassagemPendenteArtesp.Adesao.Id.TryToInt(), request.PassagemPendenteArtesp.StatusPassagem);
        }

        private MotivoNaoCompensado ValidarReenvioInvalido(ValidadorPassagemSistemaRequest request)
        {
            Log.Debug($"Passagem ID: {request.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemSistemaHandler |  Validar Reenvio Inválido");            
            if (_reenvioInvalidoValidator.ValidateForaSequencia(request.PassagemPendenteArtesp))
                return MotivoNaoCompensado.ReenvioInvalido;
            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        private MotivoNaoCompensado ValidarPassagemEnviadaForaDoPrazo(ValidadorPassagemSistemaRequest request)
        {
            Log.Debug($"Passagem ID: {request.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemSistemaHandler | Validar Passagem Fora Do Prazo");            
            _passagemForaDoPrazoValidator.Init(request.PassagemPendenteArtesp);
            if (_passagemForaDoPrazoValidator.ValidatePrazo())
            {
                request.PassagemPendenteArtesp.PassagemRecusadaMensageria = true;

                return MotivoNaoCompensado.PassagemEnviadaForaDoPrazo;
            }

            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        private MotivoNaoCompensado ValidarSaldo(ValidadorPassagemSistemaRequest request)
        {
            Log.Debug($"Passagem ID: {request.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemSistemaHandler | Validar Saldo");            
            return _saldoValidator.Validate(request.PassagemPendenteArtesp);
        }

        private MotivoNaoCompensado ValidarValorTarifa(ValidadorPassagemSistemaRequest request)
        {
            Log.Debug($"Passagem ID: {request.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemSistemaHandler | Validar Valor Tarifa");            
            return _valorTarifaValidator.Validate(request.PassagemPendenteArtesp);
        }

        private MotivoNaoCompensado ValidarVeiculoIsento(ValidadorPassagemSistemaRequest request)
        {
            Log.Debug($"Passagem ID: {request.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemSistemaHandler | Validar Veiculo Isento");
            if (_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarVeiculoIsento.ToString()))
                return MotivoNaoCompensado.Isento;
            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        private MotivoNaoCompensado ValidarGrupoIsentoTag(ValidadorPassagemSistemaRequest request)
        {
            Log.Debug($"Passagem ID: {request.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemSistemaHandler | Validar Grupo Isento Tag");            
            return _grupoIsentoTagValidator.Validate(request.PassagemPendenteArtesp);
        }

        private MotivoNaoCompensado ValidarHorarioPassagem(ValidadorPassagemSistemaRequest request)
        {
            if (_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.EManual.ToString()))            
                return ValidarHorarioPassagemManual(request);            
            else            
                return ValidarHorarioPassagemAutomatica(request);            
        }

        private MotivoNaoCompensado ValidarHorarioPassagemAutomatica(ValidadorPassagemSistemaRequest request)
        {
            Log.Debug($"Passagem ID: {request.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemSistemaHandler | Validar Horário Passagem Automática");            
            return _horarioPassagemAutomaticaValidator.Validate(request.PassagemPendenteArtesp);
        }

        private MotivoNaoCompensado ValidarHorarioPassagemManual(ValidadorPassagemSistemaRequest request)
        {
            Log.Debug($"Passagem ID: {request.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemSistemaHandler | Validar Horário Passagem Manual");            
            return _horarioPassagemManualValidator.Validate(request.PassagemPendenteArtesp);
        }

        private MotivoNaoCompensado ValidarTransacaoRepetida(ValidadorPassagemSistemaRequest request)
        {
            Log.Debug($"Passagem ID: {request.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemSistemaHandler | Validar Transacao Repetida");            
            return _transacaoRepetidaValidator.Validate(request.PassagemPendenteArtesp);
        }

        private MotivoNaoCompensado ValidarCobrancaIndevida(ValidadorPassagemSistemaRequest request)
        {
            Log.Debug($"Passagem ID: {request.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemSistemaHandler | Validar Cobrança Indevida");            
            return _cobrancaIndevidaValidator.Validate(request.PassagemPendenteArtesp);
        }

        private MotivoNaoCompensado ValidarTagGrupoPraca(ValidadorPassagemSistemaRequest request)
        {
            Log.Debug($"Passagem ID: {request.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemSistemaHandler | validarTagBloqueada");            
            if (_tagGrupoPracaValidator.Validate(request.PassagemPendenteArtesp))
                return MotivoNaoCompensado.TagNaoPertenceGrupoIsento;

            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }
    }
}
