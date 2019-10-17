using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Bo;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ProcessadorPassagensActors.CommandQuery.Validators.Validator;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp
{
    public class GeradorPassagemArtespHandler : Loggable,
           ICommand<GeradorPassagemRequest, GeradorPassagemResponse>
    {

        #region [Properties]

        private ObterPassagemIdPorMensagemItemIdQuery _passagemIdPorMensagemItemIdQuery;
        private ObterPassagemCompletaPorPassagemIdQuery _passagemCompletaPorPassagemIdQuery;
        private CriarPassagemCompletaQuery _criarPassagemCompletaQuery;
        private DefinirCategoriaUtilizadaArtesp _definirCategoriaUtilizadaArtesp;
        readonly GenericValidator<PassagemPendenteArtesp> _validator;

        #endregion

        #region [Ctor]

        public GeradorPassagemArtespHandler()
        {
            _validator = new GenericValidator<PassagemPendenteArtesp>();
            _passagemIdPorMensagemItemIdQuery = new ObterPassagemIdPorMensagemItemIdQuery();
            _passagemCompletaPorPassagemIdQuery = new ObterPassagemCompletaPorPassagemIdQuery();
            _criarPassagemCompletaQuery = new CriarPassagemCompletaQuery();
            _definirCategoriaUtilizadaArtesp = new DefinirCategoriaUtilizadaArtesp();
        }


        #endregion

        /// <summary>
        /// Executa o processamento de Passagens
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GeradorPassagemResponse Execute(GeradorPassagemRequest request)
        {
            //TODO: criar mecanismo de cache Lvl1 - memoria do servidor
            // CategoriaVeiculo
            // Conveniado, Praca, Pista
            // Feriado
            var response = new GeradorPassagemResponse
            {
                PassagemPendenteArtesp = CarregarPassagemPendenteArtesp(request.PassagemPendenteArtesp)
            };

            return response;
        }


        public PassagemPendenteArtesp CarregarPassagemPendenteArtesp(PassagemPendenteArtesp passagemPendenteArtesp)
        {
            Log.Debug($"Passagem ID: {passagemPendenteArtesp.MensagemItemId} - Fluxo: GeradorPassagemHandler | Verificar Passagem existente.");

            var passagemId = DataBaseConnection.HandleExecution(_passagemIdPorMensagemItemIdQuery.Execute, passagemPendenteArtesp.MensagemItemId);
            if (passagemId > 0)
            {
                // carregar passagem da base de dados
                Log.Debug($"Passagem ID: {passagemPendenteArtesp.MensagemItemId} - Fluxo: GeradorPassagemHandler | Carregando Passagem existente.");

                var passagemCompletaFilter = new ObterPassagemCompletaFilter(passagemPendenteArtesp, passagemId);
                DataBaseConnection.HandleExecution(_passagemCompletaPorPassagemIdQuery.Execute, passagemCompletaFilter);
            }
            else
            {
                // criar passagem baseado no passagem pendente
                Log.Debug($"Passagem ID: {passagemPendenteArtesp.MensagemItemId} - Fluxo: GeradorPassagemHandler | Criando Passagem Completa.");
                DataBaseConnection.HandleExecution(_criarPassagemCompletaQuery.Execute,passagemPendenteArtesp);
            }

            passagemPendenteArtesp.PossuiSolicitacaoImagem = (passagemPendenteArtesp.Adesao.Tag.SolicitacaoImagem.Id ?? 0) > 0;

            Log.Debug($"Passagem ID: {passagemPendenteArtesp.MensagemItemId} - Fluxo: GeradorPassagemHandler | DefinirCategoriaUtilizadaArtesp");
            _definirCategoriaUtilizadaArtesp.Definir(passagemPendenteArtesp);

            return passagemPendenteArtesp;
        }
    }
}
