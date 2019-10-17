using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.CommandQuery.Validators.Validator;
using System;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp
{
    public class ValidadorPassagemArtespHandler : Loggable
    {
        #region [Properties]

        private readonly GenericValidator<PassagemPendenteArtesp> _validator;
        private TagPracaBloqueadoLadoMensageriaValidator _tagPracaBloqueadoLadoMensageriaValidator;
        private OSAValidator _oSAValidator;
        private HorarioValidator _horarioValidator;

        #endregion

        public ValidadorPassagemArtespHandler()
        {
            _validator = new GenericValidator<PassagemPendenteArtesp>();
            _tagPracaBloqueadoLadoMensageriaValidator = new TagPracaBloqueadoLadoMensageriaValidator();
            _oSAValidator = new OSAValidator();
            _horarioValidator = new HorarioValidator();
        }

        public ValidadorPassagemResponse Response { get; set; }

        /// <summary>
        /// Executa o processamento de Passagens
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ValidadorPassagemResponse Execute(ValidadorPassagemRequest request)
        {
            var passagemInvalida = false;
            var motivoNaoCompensado = MotivoNaoCompensado.SemMotivoNaoCompensado;

            Response = new ValidadorPassagemResponse
            {
                PassagemPendenteArtesp = request.PassagemPendenteArtesp,
            };

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarExistenciaTag(out passagemInvalida);

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarSePossuiAdesao();

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarConcessionariaConectSys(out passagemInvalida);

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarPraca();

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarPista();

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarCategoriaPassagem();

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarCategoriaUtilizada();

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarPlaca();

            // validações abaixo consomem cache ou base de dados
            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarOsa(request.PassagemPendenteArtesp.Mensagem.OsaId);

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarHora(request.PassagemPendenteArtesp.DataPassagem, request.PassagemPendenteArtesp.DataCriacao);

            Response.MotivoNaoCompensado = motivoNaoCompensado;
            Response.PassagemInvalida = passagemInvalida;

            return Response;
        }

        private MotivoNaoCompensado ValidarTagBloqueada(ValidadorPassagemRequest request)
        {
            _tagPracaBloqueadoLadoMensageriaValidator.Init(request.PassagemPendenteArtesp);

            if (_tagPracaBloqueadoLadoMensageriaValidator.ValidateSituacaoTag())
                return MotivoNaoCompensado.TagBloqueado;

            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        public MotivoNaoCompensado ValidarPlaca()
        {
            Log.Debug($"Passagem ID: {Response.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemHandler | ValidarPlaca");
            if (!_validator.Validate(Response.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarPlaca.ToString()))
                return MotivoNaoCompensado.PlacaInvalida;
            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        #region Chamada das validações
        public MotivoNaoCompensado ValidarExistenciaTag(out bool passagemInvalida)
        {
            Log.Debug($"Passagem ID: {Response.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemHandler | ValidarExistenciaTag");
            if (!_validator.Validate(Response.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarExistenciaTag.ToString()))
            {
                passagemInvalida = true;
                return MotivoNaoCompensado.TagInvalido;
            }

            passagemInvalida = false;
            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        public MotivoNaoCompensado ValidarSePossuiAdesao()
        {
            Log.Debug($"Passagem ID: {Response.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemHandler | ValidarSePossuiAdesao");
            if (!_validator.Validate(Response.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarSePossuiAdesao.ToString()))
                return MotivoNaoCompensado.TagBloqueado;
            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        public MotivoNaoCompensado ValidarPraca()
        {
            Log.Debug($"Passagem ID: {Response.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemHandler | ValidarPraca");
            if (!_validator.Validate(Response.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarPraca.ToString()))
                return MotivoNaoCompensado.PracaInvalida;
            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        public MotivoNaoCompensado ValidarPista()
        {
            Log.Debug($"Passagem ID: {Response.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemHandler | ValidarPista");
            if (!_validator.Validate(Response.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarPista.ToString()))
                return MotivoNaoCompensado.PistaInvalida;
            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        public MotivoNaoCompensado ValidarCategoriaPassagem()
        {
            Log.Debug($"Passagem ID: {Response.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemHandler | ValidarCategoriaPassagem");
            if (!_validator.Validate(Response.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarCategoriaPassagem.ToString()))
                return MotivoNaoCompensado.CategoriaNaoInformada;
            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        public MotivoNaoCompensado ValidarCategoriaUtilizada()
        {
            Log.Debug($"Passagem ID: {Response.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemHandler | ValidarCategoriaUtilizada");
            if (!_validator.Validate(Response.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarCategoriaUtilizada.ToString()))
                return MotivoNaoCompensado.DadosInvalidos;
            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        public MotivoNaoCompensado ValidarOsa(int osaId)
        {
            Log.Debug($"Passagem ID: {Response.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemHandler | validarOSA");
            if (!_oSAValidator.Validate(osaId))
                return MotivoNaoCompensado.OSAInvalida;
            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        public MotivoNaoCompensado ValidarConcessionariaConectSys(out bool passagemInvalida)
        {
            Log.Debug($"Passagem ID: {Response.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemHandler | ValidarConcessionariaConectSys");
            if (!_validator.Validate(Response.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarConcessionariaConectSys.ToString()))
            {
                passagemInvalida = true;
                return MotivoNaoCompensado.ConcessionariaInvalida;
            }

            passagemInvalida = false;
            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        public MotivoNaoCompensado ValidarHora(DateTime dataPassagem, DateTime dataCriacao)
        {
            Log.Debug($"Passagem ID: {Response.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemHandler | validarHora");
            var validarHora = new HorarioValidator();
            if (!validarHora.Validate(dataPassagem, dataCriacao))
                return MotivoNaoCompensado.HorarioInvalido;
            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        #endregion
    }
}
