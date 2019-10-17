using System;
using System.Collections.Generic;
using System.Linq;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.CommandQuery.Validators.Validator;
using ConectCar.Transacoes.Domain.Enum;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Bo
{
    public class EstornoBo : Loggable
    {
        
        private TransacaoPassagemArtesp TransacaoAnterior { get; set; }        
        private PassagemValePedagioValidator _passagemValePedagioValidator;
        private ObterStatusAdesaoIdPlacaDocumentoPortransacaoIdOriginalQuery _statusAdesaoIdPlacaDocumentoPortransacaoIdOriginalQuery;
        private ObterAdesaoPorPlacaDocumentoQuery _adesaoPorPlacaDocumentoQuery;
        private ObterAdesaoPorDocumentoQuery _adesaoPorDocumentoQuery;

        public EstornoBo()
        {            
            _passagemValePedagioValidator = new PassagemValePedagioValidator();
            _statusAdesaoIdPlacaDocumentoPortransacaoIdOriginalQuery = new ObterStatusAdesaoIdPlacaDocumentoPortransacaoIdOriginalQuery();
            _adesaoPorPlacaDocumentoQuery = new ObterAdesaoPorPlacaDocumentoQuery();
            _adesaoPorDocumentoQuery = new ObterAdesaoPorDocumentoQuery();
        }


        public TransacaoPassagemArtesp ValidarEstornoSeNecessario(PassagemPendenteArtesp passagemPendenteArtesp,
            List<DetalheViagem> viagens)
        {
            if (passagemPendenteArtesp.TransacaoPassagemIdAnterior > 0)
            {
                var obterPassagemAnteriorValida =
                new ObterPassagemAnteriorValidaQuery();

                var passagemOriginal = DataBaseConnection.HandleExecution(obterPassagemAnteriorValida.Execute,new ObterPassagemAnteriorValidaCompletaFilter(passagemPendenteArtesp.Conveniado.Id ?? 0,
                        passagemPendenteArtesp.NumeroReenvio,
                        passagemPendenteArtesp.ConveniadoPassagemId,
                        passagemPendenteArtesp.Conveniado.CodigoProtocoloArtesp));

                if (passagemOriginal != null &&
                    passagemPendenteArtesp.NumeroReenvio > passagemOriginal.Reenvio &&
                    !PossuiViagemAgendadaAnterior(passagemOriginal, passagemPendenteArtesp, viagens))
                {

                    var obterTransacaoPassagem = new ObterTransacaoPassagemPorTransacaoIdQuery();

                    TransacaoAnterior = DataBaseConnection.HandleExecution(obterTransacaoPassagem.Execute,passagemPendenteArtesp.TransacaoPassagemIdAnterior);
                    
                    Estornar(SomenteInformacoesAlteradas(passagemPendenteArtesp, passagemOriginal), passagemPendenteArtesp.TransacaoPassagemIdAnterior);
                }
            }
            return TransacaoAnterior;
        }


        private bool PossuiViagemAgendadaAnterior(PassagemAnteriorValidaDto passagemOriginal
            , PassagemPendenteArtesp passagemPendenteArtesp
            ,List<DetalheViagem> detalhesViagem)
        {
            if (passagemOriginal == null)
                return false;

            long? viagemId = null;

            if (detalhesViagem != null && detalhesViagem.Any())
                viagemId = detalhesViagem.FirstOrDefault().Viagem.Id;

            
            var viagens = _passagemValePedagioValidator.ObterViagensAgendadas(passagemPendenteArtesp, viagemId);

            if (viagens != null && viagens.Any())
                return true;

            //if (DetalhesViagens != null && DetalhesViagens.Any(c => c.StatusDetalheViagem == StatusDetalheViagem.Criada))
            //    return true;

            return false;
        }

        private bool SomenteInformacoesAlteradas(PassagemPendenteArtesp passagemNova, PassagemAnteriorValidaDto passagemAntiga)
        {
            return passagemAntiga.Data == passagemNova.DataPassagem
                && passagemAntiga.Valor == passagemNova.Valor &&
                   passagemAntiga.CodigoPraca == passagemNova.Praca.CodigoPraca &&
                   passagemAntiga.CodigoPista == passagemNova.Pista.CodigoPista;
        }

        private void Estornar(bool somenteInformacoesAlteradas,  long transacaoPassagemIdAnterior)
        {            
            var anteriorFilter = DataBaseConnection.HandleExecution(_statusAdesaoIdPlacaDocumentoPortransacaoIdOriginalQuery.Execute,transacaoPassagemIdAnterior);
            TransacaoAnterior.Estorno = new EstornoPassagem
            {
                SomenteInformacoesAlteradas = somenteInformacoesAlteradas
            };

            if (TransacaoAnterior.StatusId != (int)StatusAutorizacao.Ativa)
                throw new Exception("Erro ao estornar transação passagem. Transação já estava cancelada.");

            if (anteriorFilter.StatusAdesao.Equals(StatusAutorizacao.Cancelada))
            {                
                var adesaoCorrenteId = DataBaseConnection.HandleExecution(_adesaoPorPlacaDocumentoQuery.Execute,anteriorFilter);

                if (adesaoCorrenteId <= 0)
                {                    
                    var listAdesaoCorrente = DataBaseConnection.HandleExecution(_adesaoPorDocumentoQuery.Execute,anteriorFilter.Documento);

                    if (listAdesaoCorrente > 0)
                        TransacaoAnterior.AdesaoId = listAdesaoCorrente;
                }
                else
                    TransacaoAnterior.AdesaoId = adesaoCorrenteId;

            }
            else
                TransacaoAnterior.AdesaoId = anteriorFilter.AdesaoId;

        }

    }
}
