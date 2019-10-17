using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ProcessadorPassagensActors.Infrastructure;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class ValePedagioEdiValidator : Loggable, IValidator
    {
        #region [Properties]
        private const string TransacaoProvisoriaInvalida = "Detalhe de Viagem já possuí transação provisória.";
        private const string TransacaoInvalida = "Detalhe de Viagem já possuí transação.";
        private readonly PassagemPendenteEDI _passagemPendenteEdi;
        public ValePedagioEdiDto ValePedagioEdiDto;

        #endregion

        #region [Ctor]
        public ValePedagioEdiValidator(PassagemPendenteEDI passagemPendenteEdi)
        {
            _passagemPendenteEdi = passagemPendenteEdi;
            ValePedagioEdiDto = new ValePedagioEdiDto();
        }
        #endregion

        public void Validate()
        {

            var viagens = ObterViagemAgendada(_passagemPendenteEdi.StatusCobranca.Equals(StatusCobranca.Confirmacao));

            DetalheViagem detalheConsumido = null;
            if (viagens != null && viagens.Any())
            {
                var detalhesAgendamento = viagens;
                detalheConsumido = AvaliarSituacaoValePedagio(detalhesAgendamento);

                if (detalheConsumido != null)
                {
                    ValePedagioEdiDto.ViagensParaRetorno.Add(detalheConsumido);
                }
            }

            var ehTransacaoProvisoria = _passagemPendenteEdi.StatusCobranca.Equals(StatusCobranca.Provisoria);
            if (detalheConsumido != null)
            {
                if (ehTransacaoProvisoria)
                {
                    if (detalheConsumido.StatusDetalheViagem != StatusDetalheViagem.Criada)
                        throw new EdiDomainException(TransacaoProvisoriaInvalida, _passagemPendenteEdi);

                    detalheConsumido.StatusDetalheViagemId = (int)StatusDetalheViagem.AguardandoTransacaoDefinitiva;
                }
                else
                {
                    if (detalheConsumido.StatusDetalheViagem == StatusDetalheViagem.Utilizada)
                        throw new DomainException(TransacaoInvalida);

                    detalheConsumido.StatusDetalheViagemId = (int)StatusDetalheViagem.Utilizada;
                }

                if (_passagemPendenteEdi.StatusCobranca != StatusCobranca.Confirmacao)
                    CancelarDetalhesDuplicados(detalheConsumido);
            }
        }



        public void CancelarDetalhesDuplicados(DetalheViagem viagemAgendada)
        {
            var listarViagensASeremCanceladas = new ListarViagensASeremCanceladasQuery();
            var viagensACancelar = listarViagensASeremCanceladas.Execute(viagemAgendada);

            foreach (var viagem in viagensACancelar)
            {
                if (viagem.Id != viagemAgendada.Id)
                {
                    if (viagem.StatusDetalheViagemId == (int) StatusDetalheViagem.CanceladaDuplicidade)
                        return;

                    viagem.StatusDetalheViagemId = (int) StatusDetalheViagem.CanceladaDuplicidade;
                    viagem.DataCancelamento = DateTime.Now;

                    //save
                    ValePedagioEdiDto.ViagensParaRetorno.Add(viagem);
                }
            }
        }

        private List<DetalheViagem> ObterViagemAgendada(bool ehTransacaoConfirmacao)
        {
            IEnumerable<DetalheViagem> retorno;
            if (ehTransacaoConfirmacao)
            {
                var obterViagemAguardandoDefinicaoQuery = new ObterViagemAguardandoDefiniciaoPorPlacaPracaDataPassagemQuery();
                retorno = obterViagemAguardandoDefinicaoQuery.Execute(_passagemPendenteEdi);
                return retorno.ToList();
            }

            var obterViagemAgendadaQuery = new ObterViagemAgendadaPorPlacaPracaDataPassagemQuery();
            var filter = new ObterViagemAgendadaPorPlacaPracaDataPassagemFilter
            {
                Placa = _passagemPendenteEdi.Placa,
                PracaId = _passagemPendenteEdi.Praca.Id,
                DataPassagem = _passagemPendenteEdi.DataPassagem
            };

            retorno = obterViagemAgendadaQuery.Execute(filter);
            return retorno.ToList();
        }

        private DetalheViagem AvaliarSituacaoValePedagio(List<DetalheViagem> listaDetalheViagens)
        {
            Log.Info($"Validando o valor da passagem com o valor agendado para o detalheTrnId: {_passagemPendenteEdi.DetalheTrnId}");

            DetalheViagem detalheViagem = null;

            foreach (var viagem in listaDetalheViagens)
            {
                if (_passagemPendenteEdi.Valor != viagem.ValorPassagem) continue;
                detalheViagem = viagem;
                break;
            }

            if (detalheViagem == null)
            {
                if (!AvaliarSituacaoRecusa())
                {
                    var v = listaDetalheViagens.FirstOrDefault();
                    throw new EdiTransacaoParceiroException(CodigoRetornoTransacaoTRF.ValorNaoCorrespondenteCAT,
                        _passagemPendenteEdi, (v?.Id ?? 0).TryToInt());
                }

                detalheViagem = listaDetalheViagens.FirstOrDefault();
            }
            return detalheViagem;
        }

        private bool AvaliarSituacaoRecusa()
        {
            var obterQtdeRecusaDetalheTrn = new ObterCountDetalheTrfRecusadoPorDetalhesDaPassagemQuery();

            var quantidadeRecusasDetalheTrn = obterQtdeRecusaDetalheTrn.Execute(_passagemPendenteEdi);

            var obterParametrosValePedagioFinanceiroQuery = new ObterNumeroVezesRecusadoParamValePedagioFinanceiroQuery();
            var numeroVezesRecusado = obterParametrosValePedagioFinanceiroQuery.Execute();
            return quantidadeRecusasDetalheTrn >= numeroVezesRecusado;
        }


    }
}
