using ConectCar.Cadastros.Conveniados.Backend.CommonQuery.Query;
using ConectCar.Cadastros.Domain.Model;
using ConectCar.Framework.Backend.CommonQuery.Query;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Framework.Infrastructure.Transactions;
using GeradorPassagensPendentesBatch.CommandQuery.Commands;
using GeradorPassagensPendentesBatch.CommandQuery.Commands.CommandsArgs;
using GeradorPassagensPendentesBatch.CommandQuery.Messages;
using GeradorPassagensPendentesBatch.CommandQuery.Queries;
using GeradorPassagensPendentesBatch.CommandQuery.Queries.Filters;
using GeradorPassagensPendentesBatch.CommandQuery.Resources;
using GeradorPassagensPendentesBatch.CommandQuery.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Async;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GeradorPassagensPendentesBatch.CommandQuery.Handlers
{
    /// <summary>
    /// Handler gerador de passagens pendentes de processamento.
    /// </summary>
    public class GeradorPassagensPendentesHandler : Loggable
    {
        #region [Properties]

        private DbConnectionDataSource _readOnlyDataSource;
        private DbConnectionDataSource _dataSource;
        private DbConnectionDataSource _dataSourceMensageria;
        private ServiceBusDataSourceBase _serviceBusDataSource;
        private PassagemPendenteTopicCommand _passagemPendenteTopicCommand;

        #endregion [Properties]

        #region [Ctor]

        /// <summary>
        /// Cria um handler gerador de passagens pendentes de processamento.
        /// </summary>
        /// <param name="readOnlyDataSource">Fonte de dados readonly.</param>
        /// <param name="dataSource">Fonte de dados ConectSys.</param>
        /// <param name="dataSourceMensageria">Fonte de dados mensageria.</param>
        public GeradorPassagensPendentesHandler(DbConnectionDataSource readOnlyDataSource
            , DbConnectionDataSource dataSource
            , DbConnectionDataSource dataSourceMensageria
            , ServiceBusDataSourceBase serviceBusDataSource
            , ObterConcessionariasQuery obterConcessionariasQuery
            , PassagemPendenteTopicCommand passagemPendenteTopicCommand)
        {
            try
            {
                _readOnlyDataSource = readOnlyDataSource;
                _dataSource = dataSource;
                _dataSourceMensageria = dataSourceMensageria;
                _serviceBusDataSource = serviceBusDataSource;
                _passagemPendenteTopicCommand = passagemPendenteTopicCommand;

            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message, ex);
            }
        }

        #endregion [Ctor]

        #region [Methods]

        /// <summary>
        /// Gera as passagens pendentes de processamento para uma determinada concessionária.
        /// </summary>        
        /// <param name="tempoMaximoTtlEmMinutos">Tempo máximo do TTL.</param>
        /// <param name="qtdMaximaPassagensParaProcessar">Qtd máxima de passagens para processar.</param>
        /// <param name="concessionarias">Concessionárias.</param>
        /// <returns>Task</returns>
        public async Task GerarPassagensPendentesAsync(int tempoMaximoTtlEmMinutos
            , int qtdMaximaPassagensParaProcessar
            , IEnumerable<ConcessionariaModel> concessionarias)
        {
            await Task.Run(() =>
            {
                try
                {
                    var passagensDisponiveisProcessamento = ObterPassagensDisponiveisParaProcessamento(tempoMaximoTtlEmMinutos, qtdMaximaPassagensParaProcessar, concessionarias);
                    var passagensPendentesProcessamento = FiltrarPassagensDisponiveisParaProcessamento(passagensDisponiveisProcessamento, concessionarias);

                    var sucessoEnvio = EnviarPassagensPendentes(passagensPendentesProcessamento);

                    if (sucessoEnvio)
                    {
                        AtualizarTtlPassagensPendentesProcessamento(tempoMaximoTtlEmMinutos, passagensPendentesProcessamento);
                    }
                    else
                    {
                        Log.Info(GeradorPassagemPendenteResource.PassagensPendentesBarramentoNaoEnviadas);
                    }
                }

                catch (ArgumentNullException ex)
                {
                    Log.Debug(ex.Message);
                }
                catch (ArgumentException ex)
                {
                    Log.Debug(ex.Message);
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format(GeradorPassagemPendenteResource.Error, ex.Message), ex);
                }
            });
        }

        /// <summary>
        /// Atualiza o Ttl das passagens pendentes de processamento.
        /// </summary>
        /// <param name="tempoMaximoTtlEmMinutos">Tempo máximo do TTL.</param>
        /// <param name="passagensPendentesProcessamento">PassagemPendenteMessage[]</param>
        private void AtualizarTtlPassagensPendentesProcessamento(int tempoMaximoTtlEmMinutos, List<PassagemPendenteMessage> passagensPendentesProcessamento)
        {
            Log.Debug(GeradorPassagemPendenteResource.InicioAtualizacaoTTLPassagensPendentes);
            var sucessoAtualizacaoTtl = AtualizarTtlPassagensPendentes(passagensPendentesProcessamento, tempoMaximoTtlEmMinutos);
            if (sucessoAtualizacaoTtl)
            {
                Log.Debug(GeradorPassagemPendenteResource.SucessoAtualizacaoTTLPassagensPendentes);
            }
            else
            {
                Log.Debug(GeradorPassagemPendenteResource.FalhaAtualizacaoTTLPassagensPendentes);
            }
        }

        /// <summary>
        /// Filtra as passagens pendentes de processamento.
        /// </summary>
        /// <param name="passagensDisponiveisProcessamento">PassagemPendenteMessage[]</param>
        /// <param name="concessionarias">Concessionárias.</param>
        /// <returns>PassagemPendenteMessage[]</returns>
        private List<PassagemPendenteMessage> FiltrarPassagensDisponiveisParaProcessamento(
            List<PassagemPendenteMessage> passagensDisponiveisProcessamento
            , IEnumerable<ConcessionariaModel> concessionarias)
        {
            Log.Debug(String.Format("Foram encontradas {0} passagens disponíveis para processamento.", passagensDisponiveisProcessamento.Count()));

            Log.Debug(GeradorPassagemPendenteResource.PreencherSLAPassagensPendentes);
            var passagensDisponiveisProcessamentoComSla = PreencherSlaDaPassagem(passagensDisponiveisProcessamento, concessionarias);

            Log.Debug("Filtrando passagens disponíveis.");
            var passagensPendentesProcessamento = FiltrarPassagensDisponiveis(passagensDisponiveisProcessamentoComSla);
            if (passagensPendentesProcessamento == null || !passagensPendentesProcessamento.Any())
            {
                throw new ArgumentNullException("passagensPendentesProcessamento", GeradorPassagemPendenteResource.NaoExistePassagensPendentes);
            }

            Log.Info(String.Format("Foram encontradas {0} passagens pendentes para envio ao barramento.", passagensPendentesProcessamento.Count()));
            return passagensPendentesProcessamento;
        }

        /// <summary>
        /// Obtem as passagens pendentes disponiveis para processamento.
        /// </summary>
        /// <param name="tempoMaximoTtlEmMinutos">Tempo do TTL em minutos.</param>
        /// <param name="qtdMaximaPassagensParaProcessar">Quantidade máxima de passagens a processar.</param>
        /// <returns>PassagemPendenteMessage[]</returns>
        private List<PassagemPendenteMessage> ObterPassagensDisponiveisParaProcessamento(int tempoMaximoTtlEmMinutos
            , int qtdMaximaPassagensParaProcessar
            , IEnumerable<ConcessionariaModel> concessionarias)
        {
            Log.Debug(GeradorPassagemPendenteResource.ObterPassagensPendentes);

            if (concessionarias == null || !concessionarias.Any())
            {
                throw new ArgumentNullException("concessionarias", GeradorPassagemPendenteResource.NaoExisteConcessionarias);
            }

            var concessionariasArtesp = concessionarias.Where(x => x.AtivoProtocoloArtesp);
            if (concessionariasArtesp == null || !concessionariasArtesp.Any())
            {
                throw new ArgumentNullException("concessionariasArtesp", GeradorPassagemPendenteResource.NaoExisteConcessionariasArtesp);
            }


            var request = new ObterPassagensPendentesFilter
            {
                QuantidadeMinutosTtl = tempoMaximoTtlEmMinutos,
                QtdMaximaPassagens = qtdMaximaPassagensParaProcessar
            };
            var obterPassagensPendentesQuery = new ObterPassagensPendentesQuery(_dataSourceMensageria);
            var passagensDisponiveisProcessamento = obterPassagensPendentesQuery.Execute(request);
            if (passagensDisponiveisProcessamento == null || !passagensDisponiveisProcessamento.Any())
            {
                throw new ArgumentNullException("passagensDisponiveisProcessamento", GeradorPassagemPendenteResource.NaoExistePassagensPendentes);
            }


            if (passagensDisponiveisProcessamento == null || !passagensDisponiveisProcessamento.Any())
            {
                throw new ArgumentNullException("passagensDisponiveisProcessamento", GeradorPassagemPendenteResource.NaoExistePassagensPendentes);
            }


            return passagensDisponiveisProcessamento.ToList();
        }



        /// <summary>
        /// Preenche o sla da passagem.
        /// </summary>
        /// <param name="passagensDisponiveisProcessamento">PassagemPendenteMessage[]</param>
        /// <param name="concessionarias">Concessionárias.</param>
        /// <returns>PassagemPendenteMessage[]</returns>
        private List<PassagemPendenteMessage> PreencherSlaDaPassagem(
            List<PassagemPendenteMessage> passagensDisponiveisProcessamento
            , IEnumerable<ConcessionariaModel> concessionarias)
        {
            Log.Debug("Associando o SLA de envio de passagem a cada passagem.");

            passagensDisponiveisProcessamento.ForEach(x =>
            {

                x.TempoSLAEnvioPassagem = concessionarias.Where(y => y.CodigoProtocoloArtesp == x.ConcessionariaId)
                                                         .Select(y => y.TempoSLAEnvioPassagem)
                                                         .FirstOrDefault();
            });

            return passagensDisponiveisProcessamento;
        }

        /// <summary>
        /// Filtra as passagens disponíveis para processamento caso haja mais de 1 passagem em uma mesma TAG.
        /// </summary>
        /// <param name="passagensDisponiveis">Passagens disponíveis para processamento.</param>
        /// <param name="concessionaria">Concessionária.</param>
        /// <returns>Passagens pendentes de processamento</returns>
        private List<PassagemPendenteMessage> FiltrarPassagensDisponiveis(IEnumerable<PassagemPendenteMessage> passagensDisponiveis)
        {
            var ret = new List<PassagemPendenteMessage>();

            passagensDisponiveis
                .GroupBy(x => new { x.TagId, x.ConcessionariaId })
                .Select(x => x.FirstOrDefault().TagId)
                .ToList()
                .ForEach(tagId =>
                {
                    var passagensDisponiveisTag = passagensDisponiveis.Where(y => y.TagId == tagId).ToList();

                    if (ExistePassagensPendentesDentroEForaDoSla(passagensDisponiveisTag))
                    {
                        var passagensDentroDoSla = passagensDisponiveisTag.Where(x => PassagemDentroDoSlaConcessionaria(x));
                        var passagemMaisAntiga = ObterPassagemPendenteMaisAntiga(passagensDentroDoSla);
                        ret.Add(passagemMaisAntiga);
                    }
                    else
                    {
                        //Verifica se existem passagens com reenvio...
                        if (ExistePassagemPendenteComReenvio(passagensDisponiveisTag))
                        {
                            var menorReenvio = passagensDisponiveisTag.Min(x => x.NumeroReenvio);
                            var passagensComMenorReenvio = passagensDisponiveisTag.Where(x => x.NumeroReenvio == menorReenvio);
                            var passagemPendenteComReenvioMaisAntiga = ObterPassagemPendenteMaisAntiga(passagensComMenorReenvio);
                            ret.Add(passagemPendenteComReenvioMaisAntiga);
                        }
                        else
                        {
                            var passagemMaisAntiga = ObterPassagemPendenteMaisAntiga(passagensDisponiveisTag);
                            ret.Add(passagemMaisAntiga);
                        }
                    }
                });

            return ret;
        }

        /// <summary>
        /// Verifica se há mais de uma passagem vinculada a mesma TAG que esteja dentro e fora do SLA.
        /// </summary>
        /// <param name="passagensDisponiveisTag">Passagens pedentes de uma TAG.</param>
        /// <param name="concessionaria">Concessionária.</param>
        /// <returns>True caso a TAG possua passagens dentro e fora do SLA.</returns>
        private bool ExistePassagensPendentesDentroEForaDoSla(
            IEnumerable<PassagemPendenteMessage> passagensDisponiveisTag)
        {
            return passagensDisponiveisTag.Any(x => PassagemDentroDoSlaConcessionaria(x))
                && passagensDisponiveisTag.Any(x => !PassagemDentroDoSlaConcessionaria(x));
        }

        /// <summary>
        /// Verifica se a passagem está dentro do SLA da concessionária.
        /// </summary>
        /// <param name="passagemPendente">Passagem pendente de processamento.</param>
        /// <param name="concessionaria">Concessionária</param>
        /// <returns>True caso a passagem esteja dentro do SLA.</returns>
        private bool PassagemDentroDoSlaConcessionaria(PassagemPendenteMessage passagemPendente)
        {
            var slaPassagemPendente = ObterSla(passagemPendente);
            return passagemPendente.TempoSLAEnvioPassagem > slaPassagemPendente;
        }

        /// <summary>
        /// Obtem o valor do SLA (diferença entre a data de recebimento da passagem e a data da passagem) da passagem.
        /// </summary>
        /// <remarks>
        /// SLA consiste na diferença entre a data de recebimento da passagem e a data da passagem.
        /// </remarks>
        /// <param name="passagemPendente">Passagem pendente de processamento.</param>
        /// <returns>Valor do SLA da passagem.</returns>
        private static int ObterSla(PassagemPendenteMessage passagemPendente)
        {
            return (int)(passagemPendente.DataHoraRecebimento - passagemPendente.DataHora);
        }

        /// <summary>
        /// Obtem a passagem pendente mais antiga.
        /// </summary>
        /// <param name="passagensPendentes">Passagens pendentes de processamento.</param>
        /// <returns>Passagem pendente mais antiga</returns>
        private PassagemPendenteMessage ObterPassagemPendenteMaisAntiga(IEnumerable<PassagemPendenteMessage> passagensPendentes)
        {
            return passagensPendentes.OrderBy(x => x.DataHora)
                            .ThenBy(x => x.DataHoraRecebimento)
                            .First();
        }

        /// <summary>
        /// Verifica se existe alguma passagem pendente com reenvio.
        /// </summary>
        /// <param name="passagens">Passagens pendentes de processamento.</param>
        /// <returns>True caso exista alguma passagem pendente com reenvio.</returns>
        private bool ExistePassagemPendenteComReenvio(IEnumerable<PassagemPendenteMessage> passagensPendentes)
        {
            return passagensPendentes.Any(x => x.NumeroReenvio > 0);
        }

        /// <summary>
        /// Atualiza o TTL das passagens pendentes.
        /// </summary>
        /// <param name="passagensPendentes">Passagens pendentes de processamento.</param>
        /// <param name="codigoProtocoloArtesp">Código da concessionária.</param>
        /// <param name="tempoMaximoTtlEmMinutos">Tempo máximo em TTL para próxima verificação.</param>
        /// <returns>True caso atualizações de TTL tenham sido realizadas.</returns>
        private bool AtualizarTtlPassagensPendentes(IEnumerable<PassagemPendenteMessage> passagensPendentes,
            int tempoMaximoTtlEmMinutos)
        {
            var mensagemItemIdMin = passagensPendentes.Min(x => x.MensagemItemId);
            var mensagemItemIdMax = passagensPendentes.Max(x => x.MensagemItemId);
            var args = new AlterarPassagemPendenteCommandArg
            {
                DataTtl = DateTime.Now.AddMinutes(tempoMaximoTtlEmMinutos),
                MensagemItemIdMax = mensagemItemIdMax,
                MensagemItemIdMin = mensagemItemIdMin
            };

            var alterarPassagemPendenteCommand = new AlterarPassagemPendenteCommand(_dataSourceMensageria);
            return TransactionContextHelper.ExecuteTransaction(alterarPassagemPendenteCommand.Execute, args);
        }

        /// <summary>
        /// Envia passagens pendentes de processamento para o Barramento.
        /// </summary>
        /// <param name="passagensPendentes">Passagens pendentes de processamento.</param>
        /// <returns>True caso tenha sido enviado com sucesso.</returns>
        private bool EnviarPassagensPendentes(List<PassagemPendenteMessage> passagensPendentes)
        {
            try
            {
                var topicName = ServiceBusUtil.ObterNomeQueuePassagem();
                Log.Info(string.Format(GeradorPassagemPendenteResource.EnviandoPassagem, passagensPendentes.Count, topicName));

                _passagemPendenteTopicCommand.Execute(passagensPendentes, topicName, $"sb_{topicName}", (p) =>
                {
                    IDictionary<string, object> properties = new Dictionary<string, object>();

                    properties.Add("MensagemItemId", p.MensagemItemId);

                    return properties;
                });                

                //var xJson = JsonConvert.SerializeObject(passagensPendentes);
                Log.Info(string.Format(GeradorPassagemPendenteResource.SucessoEnvio, topicName, passagensPendentes.Count));
                return true;
            }
            catch (Exception e)
            {
                Log.Error(string.Format(GeradorPassagemPendenteResource.Error, e.Message), e);
                return false;
            }
        }

        #endregion [Methods]
    }
}