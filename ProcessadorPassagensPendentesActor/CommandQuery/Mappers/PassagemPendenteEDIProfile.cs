using System;
using AutoMapper;
using ConectCar.Cadastros.Domain.Dto;
using ConectCar.Transacoes.Domain.Dto;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Dtos;

namespace ProcessadorPassagensActors.CommandQuery.Mappers
{
    public class PassagemPendenteEdiProfile : Profile
    {
        public PassagemPendenteEdiProfile()
        {
            #region PassagemPendenteEDI => PassagemAprovadaEDI
            CreateMap<PassagemPendenteEDI, PassagemAprovadaEDI>();
            #endregion

            #region PassagemPendenteEDI => PassagemReprovadaEDI
            CreateMap<PassagemPendenteEDI, PassagemReprovadaEDI>();
            #endregion

            #region PassagemPendenteEdiDto => PassagemPendenteEDI

            CreateMap<PassagemPendenteEdiDto, PassagemPendenteEDI>()
                .ForMember(d => d.Pista, opt => opt.MapFrom(src => new Pista { CodigoPista = src.NumeroPista ?? 0 }))
                .ForMember(d => d.Praca, opt => opt.MapFrom(src => new Praca { CodigoPraca = src.NumeroPraca }))
                .ForMember(d => d.Tag, opt => opt.MapFrom(src => new Tag { OBUId = src.NumeroTag }))
                .ForMember(d => d.CategoriaTag,
                    opt => opt.MapFrom(src => new CategoriaVeiculo { Codigo = src.CategoriaTag ?? 0 }))
                .ForMember(d => d.CategoriaCobrada,
                    opt => opt.MapFrom(src => new CategoriaVeiculo { Codigo = src.CategoriaCobrada ?? 0 }))
                .ForMember(d => d.CategoriaDac,
                    opt => opt.MapFrom(src => new CategoriaVeiculo { Codigo = src.CategoriaDac ?? 0 }))
                .ForMember(d => d.Conveniado,
                    opt => opt.MapFrom(src => new Conveniado { CodigoProtocolo = src.CodigoProtocolo }))
                .ForMember(d => d.DataCriacao, opt => opt.MapFrom(src => src.DataGeracao))
                .ForMember(d => d.DataPassagem, opt => opt.MapFrom(src => src.Data))
                .ForMember(d => d.DataAnterior, opt => opt.MapFrom(src => src.DataAnterior))
                .ForMember(d => d.StatusPassagem, opt => opt.MapFrom(src => (StatusPassagem)src.StatusPassagem))
                .ForMember(d => d.Valor, opt => opt.MapFrom(src => src.ValorPassagem))
                .ForMember(d => d.DetalheRepetido, opt => opt.MapFrom(src => src.DetalheRepetido))
                .ForMember(d => d.ArquivoTrfId, opt => opt.MapFrom(src => src.ArquivoTrfId))
                .ForMember(d => d.ArquivoTrnId, opt => opt.MapFrom(src => src.ArquivoTrnId))
                .ForMember(d => d.DetalheTrnId, opt => opt.MapFrom(src => src.DetalheTrnId))
                .ForMember(d => d.CodigoConveniadoAnterior, opt => opt.MapFrom(src => src.CodigoConveniadoAnterior))
                .ForMember(d => d.CodigoPaisAnterior, opt => opt.MapFrom(src => src.CodigoPaisAnterior))
                .ForMember(d => d.Placa, opt => opt.MapFrom(src => src.PlacaTag))
                .ForMember(d => d.ImagemFrontal, opt => opt.MapFrom(src => src.ImagemFrontal))
                .ForMember(d => d.ImagemLateral1, opt => opt.MapFrom(src => src.ImagemLateral1))
                .ForMember(d => d.ImagemLateral2, opt => opt.MapFrom(src => src.ImagemLateral2))
                .ForMember(d => d.DetalheTrfAprovadoManualmenteId, opt => opt.MapFrom(src => src.DetalheTrfAprovadoManualmenteId))
                .ForMember(d => d.MotivoImagem, opt => opt.MapFrom(src => src.MotivoImagem))
                .ForMember(d => d.NumeroPistaAnterior, opt => opt.MapFrom(src => src.NumeroPistaAnterior))
                .ForMember(d => d.NumeroPracaAnterior, opt => opt.MapFrom(src => src.NumeroPracaAnterior))
                .ForMember(d => d.SequenciaRegistro, opt => opt.MapFrom(src => src.SequenciaRegistro))
                .ForMember(d => d.SequenciaTransacao, opt => opt.MapFrom(src => src.SequenciaTransacao))
                .ForMember(d => d.Tipo, opt => opt.MapFrom(src => src.Tipo))
                .ForMember(d => d.NivelBateriaNormal, opt => opt.MapFrom(src => src.FlagBateria))
                .ForMember(d => d.StatusCobranca, opt => opt.MapFrom(src => src.StatusCobranca))
                .ForMember(d => d.TagViolado, opt => opt.MapFrom(src => src.FlagViolacao));
            #endregion

            #region PistaPracaConveniadoDto => PassagemPendenteEDI
            CreateMap<PistaPracaConveniadoDto, PassagemPendenteEDI>()
                    .ForMember(d => d.Praca, o => o.MapFrom(s => new Praca
                    {
                        Id = s.PracaId,
                        CodigoPraca = s.CodigoPraca,
                        IdentificacaoPraca = s.IdentificacaoPraca,
                        TempoAtualizacaoPista = s.TempoAtualizacaoPista,
                        TempoRetornoPraca = s.TempoRetornoPraca.TryToInt(),
                        TempoAtualizacaoTransacao = s.TempoAtualizacaoTransacao
                    }))
                    .ForMember(d => d.Conveniado, o => o.MapFrom(s => new Conveniado
                    {
                        Id = s.ConveniadoId,
                        HabilitarValidacaoTarifa = s.HabilitarValidacaoTarifa,
                        HabilitarConfirmacaoCategoria = s.HabilitarConfirmacaoCategoria,
                        NomeFantasia = s.NomeFantasia,
                        NomeFatura = s.NomeFatura,
                        ListaDeParaCategoriaVeiculoId = s.ListaDeParaCategoriaVeiculoId.TryToInt(),
                        CodigoProtocolo = s.CodigoProtocolo,
                        TempoDeCorrecaoDasTransacoesProvisorias = s.TempoDeCorrecaoDasTransacoesProvisorias,
                    }))
                    .ForMember(d => d.Pista, o => o.MapFrom(s => new Pista
                    {
                        Id = s.PistaId,
                        CodigoPista = s.CodigoPista
                    }));

            #endregion

            #region TagAdesaoDto => PassagemPendenteEDI
            CreateMap<TagAdesaoDto, PassagemPendenteEDI>()
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
