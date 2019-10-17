using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.CommandQuery.Validators.Validator;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp
{
    public class ValidadorPassagemAceiteManualReenvioArtespHandler : Loggable,
        ICommand<ValidadorPassagemAceiteManualReenvioRequest, ValidadorPassagemAceiteManualReenvioResponse>
    {
        #region [Properties]

        private readonly GenericValidator<PassagemPendenteArtesp> _validator;
        private OSAValidator _oSAValidator;

        
        #endregion        

        public ValidadorPassagemAceiteManualReenvioArtespHandler()
        {           
            _validator = new GenericValidator<PassagemPendenteArtesp>();
            _oSAValidator = new OSAValidator();
        }

        public ValidadorPassagemAceiteManualReenvioResponse Response { get; set; }

        /// <summary>
        /// Executa o processamento de Passagens
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ValidadorPassagemAceiteManualReenvioResponse Execute(ValidadorPassagemAceiteManualReenvioRequest request)
        {
            var motivoNaoCompensado = MotivoNaoCompensado.SemMotivoNaoCompensado;

            VerificarOsa(request.PassagemPendenteArtesp.Mensagem.OsaId);
            VerificarConcessionariaConectSys();

            if(motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarSePossuiAdesao();

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarPraca();

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarCategoriaUtilizada();

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarExistenciaTag();

            Response = new ValidadorPassagemAceiteManualReenvioResponse {
                PassagemPendenteArtesp = request.PassagemPendenteArtesp,
                MotivoNaoCompensado = motivoNaoCompensado
            };
            return Response;
        }


        #region Chamada das validações

        public MotivoNaoCompensado ValidarExistenciaTag()
        {
            Log.Debug($"Passagem ID: {Response.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemHandler | ValidarExistenciaTag");
            if (!_validator.Validate(Response.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarExistenciaTag.ToString()))
                return MotivoNaoCompensado.TagInvalido;
            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        public MotivoNaoCompensado ValidarSePossuiAdesao()
        {
            Log.Debug($"Passagem ID: {Response.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemHandler | ValidarSePossuiAdesao");
            if (!_validator.Validate(Response.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarSePossuiAdesao.ToString()))
                return MotivoNaoCompensado.AdesaoInvalida;
            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        public MotivoNaoCompensado ValidarPraca()
        {
            Log.Debug($"Passagem ID: {Response.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemHandler | ValidarPraca");
            if (!_validator.Validate(Response.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarPraca.ToString()))
                return MotivoNaoCompensado.PracaInvalida;
            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }
        
        public MotivoNaoCompensado ValidarCategoriaUtilizada()
        {
            Log.Debug($"Passagem ID: {Response.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemHandler | ValidarCategoriaUtilizada");
            if (!_validator.Validate(Response.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarCategoriaUtilizada.ToString()))
                return MotivoNaoCompensado.DadosInvalidos;
            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        public void VerificarOsa(int osaId)
        {
            if(Response.PassagemPendenteArtesp.PassagemRecusadaMensageria != true)
            {
                Log.Debug($"Passagem ID: {Response.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemHandler | validarOSA");                
                if (!_oSAValidator.Validate(osaId))                
                    Response.PassagemPendenteArtesp.PassagemRecusadaMensageria = true;                
            }                            
        }

        public void VerificarConcessionariaConectSys()
        {
            if (Response.PassagemPendenteArtesp.PassagemRecusadaMensageria != true)
            {
                Log.Debug($"Passagem ID: {Response.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemHandler | ValidarConcessionariaConectSys");
                if (!_validator.Validate(Response.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarConcessionariaConectSys.ToString()))
                    Response.PassagemPendenteArtesp.PassagemRecusadaMensageria = true;
            }
               
        }
        
       
        #endregion
    }
}
