using AutoMapper;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Dtos;

namespace ProcessadorPassagensActors.CommandQuery.Mappers
{
    public class PassagemPendenteParkProfile : Profile
    {
        public PassagemPendenteParkProfile()
        {
            #region PassagemPendentePark => PassagemAprovadaPark
            CreateMap<PassagemPendenteEstacionamento, PassagemAprovadaEstacionamento>();
            #endregion

            #region Conveniado => PassagemPendenteEstacionamento
            CreateMap<Conveniado, PassagemPendenteEstacionamento>()
                .ForMember(d => d.Conveniado, o => o.MapFrom(s => new Conveniado
                {
                    Id = s.Id,
                    CodigoProtocolo = s.CodigoProtocolo,
                    CodigoProtocoloArtesp = s.CodigoProtocoloArtesp,
                    NomeFantasia = s.NomeFantasia,
                    NomeFatura = s.NomeFatura,
                    RazaoSocial = s.RazaoSocial,
                    HabilitarValidacaoTarifa = s.HabilitarValidacaoTarifa,
                    UtilizaRps = s.UtilizaRps,
                    ListaDeParaCategoriaVeiculoId = s.ListaDeParaCategoriaVeiculoId.TryToInt(),
                    HabilitarConfirmacaoCategoria = s.HabilitarConfirmacaoCategoria,
                    AtivoProtocoloArtesp = s.AtivoProtocoloArtesp,
                    TempoDeCorrecaoDasTransacoesProvisorias = s.TempoDeCorrecaoDasTransacoesProvisorias,
                }));
            #endregion

            #region Praca => PassagemPendenteEstacionamento
            CreateMap<Praca, PassagemPendenteEstacionamento>()
            .ForMember(d => d.Praca, o => o.MapFrom(s => new Praca
            {
                Id = s.Id,
                CodigoPraca = s.CodigoPraca,
                IdentificacaoPraca = s.IdentificacaoPraca,
                TempoAtualizacaoPista = s.TempoAtualizacaoPista,
                TempoRetornoPraca = s.TempoRetornoPraca.TryToInt(),
                TempoAtualizacaoTransacao = s.TempoAtualizacaoTransacao,
            }));
            #endregion

            #region Pista => PassagemPendenteEstacionamento
            CreateMap<Pista, PassagemPendenteEstacionamento>()
              .ForMember(d => d.Pista, o => o.MapFrom(s => new Pista
              {
                  Id = s.Id,
                  CodigoPista = s.CodigoPista,
              }));
            #endregion


            #region PassagemPendenteEstacionamento => PassagemReprovadaEstacionamento
            CreateMap<PassagemPendenteEstacionamento, PassagemReprovadaEstacionamento>(); 
            #endregion

            #region TagAdesaoDto => PassagemPendenteEstacionamento
            CreateMap<TagAdesaoDto, PassagemPendenteEstacionamento>()
                    .ForMember(d => d.Adesao, o => o.MapFrom(s => new Adesao
                    {
                        Id = s.AdesaoId,
                        DataCancelamento = s.DataCancelamento,
                        PlanoId = s.PlanoId,
                        SaldoId = s.SaldoId,
                        Tag = new Tag
                        {
                            Id = s.TagId,
                            GrupoPadraoId = s.GrupoPadraoId,
                            OBUId = s.OBUId,
                            StatusTagId = s.StatusTagId,
                            EmissorId = s.EmissorId,
                            SolicitacaoImagem = new SolicitacaoImagem
                            {
                                Id = s.SolicitacaoImagem
                            }
                        },
                        ConfiguracaoAdesao = new ConfiguracaoAdesao
                        {
                            Id = s.ConfiguracaoAdesaoId,
                            Categoria = new CategoriaVeiculo { Id = s.CategoriaVeiculoId },
                            AdesaoProvisoria = s.AdesaoProvisoria
                        },
                        Cliente = new Cliente
                        {
                            Id = s.ClienteId,
                            PessoaFisica = s.PessoaFisica,
                            StatusId = s.StatusId,
                            UltimaCobrancaPaga = s.UltimaCobrancaPaga
                        },
                        Veiculo = new Veiculo
                        {
                            Id = s.VeiculoId,
                            Categoria = new CategoriaVeiculo { Id = s.CategoriaVeiculoId },
                            CategoriaConfirmada = s.CategoriaConfirmada,
                            ContagemConfirmacaoCategoria = s.ContagemConfirmacaoCategoria,
                            Placa = s.Placa,
                            DataConfirmacaoCategoria = s.DataConfirmacaoCategoria,
                            ContagemDivergenciaCategoriaConfirmada = s.ContagemDivergenciaCategoriaConfirmada
                        }
                    }))
                    .ForMember(d => d.Tag, o => o.MapFrom(s => new Tag
                    {
                        Id = s.TagId,
                        GrupoPadraoId = s.GrupoPadraoId,
                        OBUId = s.OBUId,
                        StatusTagId = s.StatusTagId,
                        SolicitacaoImagem = new SolicitacaoImagem
                        {
                            Id = s.SolicitacaoImagem
                        }
                    }));
            #endregion




        }
    }
}
