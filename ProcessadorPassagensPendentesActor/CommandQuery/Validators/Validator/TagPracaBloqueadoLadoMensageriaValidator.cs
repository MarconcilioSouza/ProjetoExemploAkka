using System.Linq;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ConectCar.Framework.Infrastructure.Log;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class TagPracaBloqueadoLadoMensageriaValidator : Loggable
    {
        public bool EhReenvioCorrecao { get; set; }
        public TagMensageriaDto TagMensageriaDto { get; set; }

        private readonly ObterCountReenvioAnteriorMensageriaQuery _countReenvioAnteriorMensageriaQuery;
        private readonly ObterTagNoMomentoDaPassagemMensageriaQuery _tagNoMomentoDaPassagemMensageriaQuery;
        private readonly ObterCountPassagemProcessadaCompensadaQuery _countPassagemProcessadaCompensadaQuery;

        public TagPracaBloqueadoLadoMensageriaValidator()
        {
            _countReenvioAnteriorMensageriaQuery = new ObterCountReenvioAnteriorMensageriaQuery();
            _tagNoMomentoDaPassagemMensageriaQuery = new ObterTagNoMomentoDaPassagemMensageriaQuery();
            _countPassagemProcessadaCompensadaQuery = new ObterCountPassagemProcessadaCompensadaQuery();
        }


        public void Init(PassagemPendenteArtesp passagemPendenteArtesp)
        {
            var existeReenvioAnterior = DataBaseConnection.HandleExecution(_countReenvioAnteriorMensageriaQuery.Execute, passagemPendenteArtesp);

            var filter = new TagNoMomentoDaPassagemFilter
            {
                PassagemPendenteArtesp = passagemPendenteArtesp,
                ExistePassagemAnterior = existeReenvioAnterior,
                TempoAtualizacaoPista = passagemPendenteArtesp.Praca.TempoAtualizacaoPista
            };
            TagMensageriaDto = DataBaseConnection.HandleExecution(_tagNoMomentoDaPassagemMensageriaQuery.Execute, filter);

            if (existeReenvioAnterior && passagemPendenteArtesp.NumeroReenvio != 0)
            {
                if (DataBaseConnection.HandleExecution(_countPassagemProcessadaCompensadaQuery.Execute,passagemPendenteArtesp))
                    EhReenvioCorrecao = true;
            }
        }


        public MotivoNaoCompensado ValidateTagBloqueada(long codigoPraca)
        {
            if (ValidateSituacaoTag())
                return MotivoNaoCompensado.TagBloqueado;



            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        public bool ValidateSituacaoTag()
        {
            return (TagMensageriaDto?.SituacaoId != null && TagMensageriaDto.SituacaoTag == SituacaoTag.Bloqueado && !EhReenvioCorrecao);
        }

        public bool ValidatePracaBloqueada(long codigoPraca)
        {
            var pracaBloqueada = false;
            if (TagMensageriaDto != null)
                pracaBloqueada = TagMensageriaDto.PracasBloqueadases.Any(p => p.CodigoPraca == codigoPraca);
            return (pracaBloqueada && !EhReenvioCorrecao);
        }
    }
}
