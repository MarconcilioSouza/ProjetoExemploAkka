using System;
using AutoMapper;
using ConectCar.Cadastros.Domain.Dto;
using ConectCar.Transacoes.Domain.Dto;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Mappers
{
    public class PassagemPendenteArtespProfile : Profile
    {
        public PassagemPendenteArtespProfile()
        {
            #region PassagemPendenteArtespDto => PassagemPendenteArtesp
            CreateMap<PassagemPendenteArtespDto, PassagemPendenteArtesp>()
                    .ForMember(d => d.Pista, opt => opt.MapFrom(src => new Pista { CodigoPista = src.Pista }))
                    .ForMember(d => d.Praca, opt => opt.MapFrom(src => new Praca { CodigoPraca = src.Praca }))
                    .ForMember(d => d.Tag, opt => opt.MapFrom(src => new Tag { OBUId = src.TagId }))
                    .ForMember(d => d.CategoriaTag, opt => opt.MapFrom(src => new CategoriaVeiculo { Codigo = src.CategoriaCadastrada }))
                    .ForMember(d => d.CategoriaUtilizada, opt => opt.MapFrom(src => new CategoriaVeiculo { Codigo = src.CategoriaCadastrada }))
                    .ForMember(d => d.CategoriaCobrada, opt => opt.MapFrom(src => new CategoriaVeiculo { Codigo = src.CategoriaCobrada }))
                    .ForMember(d => d.CategoriaDetectada, opt => opt.MapFrom(src => new CategoriaVeiculo { Codigo = src.CategoriaDetectada }))
                    .ForMember(d => d.Mensagem, opt => opt.MapFrom(src => new MensagemPassagem
                    {
                        DataRecebimento = new DateTime(1970, 1, 1).AddSeconds(src.DataHoraRecebimento),
                        Id = src.MensagemId,
                        OsaId = src.OsaId,
                        Sequencial = src.Sequencial,
                        Serie = src.Serie,
                        TipoMensagem = (TipoMensagem)src.TipoMensagem
                    }))
                    .ForMember(d => d.Conveniado, opt => opt.MapFrom(src => new Conveniado { CodigoProtocoloArtesp = src.ConcessionariaId }))
                    .ForMember(d => d.DataCriacao, opt => opt.MapFrom(src => src.DataCriacao))
                    .ForMember(d => d.DataPassagem, opt => opt.MapFrom(src => new DateTime(1970, 1, 1,0,0,0,DateTimeKind.Utc).AddSeconds(src.DataHora)))
                    .ForMember(d => d.StatusPassagem, opt => opt.MapFrom(src => src.PassagemAutomatica ? (int)StatusPassagem.Automatica : (int)StatusPassagem.Manual))
                    .ForMember(d => d.Valor, opt => opt.MapFrom(src => src.Valor))
                      .ForMember(d => d.MensagemItemId, opt => opt.MapFrom(src => src.MensagemItemId))
                      .ForMember(d => d.MotivoSemValor, opt => opt.MapFrom(src => src.MotivoSemValor))
                      .ForMember(d => d.ConveniadoPassagemId, opt => opt.MapFrom(src => src.ConveniadoPassagemId))
                      .ForMember(d => d.NumeroReenvio, opt => opt.MapFrom(src => src.NumeroReenvio))
                      .ForMember(d => d.Placa, opt => opt.MapFrom(src => src.Placa))
                      .ForMember(d => d.MotivoManual, opt => opt.MapFrom(src => (MotivoManual)src.MotivoManual))
                      .ForMember(d => d.MotivoReenvio, opt => opt.MapFrom(src => (MotivoReenvio)src.MotivoReenvio));

            #endregion

            #region PassagemPendenteViewDto => PassagemPendenteArtesp
            CreateMap<PassagemAnteriorValidaDto, PassagemPendenteArtesp>()
                    .ForMember(d => d.DataPassagem, o => o.MapFrom(s => s.Data))
                    .ForMember(d => d.Adesao, o => o.MapFrom(s => new Adesao
                    {
                        Id = s.AdesaoId,
                        DataCancelamento = s.DataCancelamento,
                        PlanoId = s.PlanoId,
                        Tag = new Tag
                        {
                            Id = s.TagId,
                            GrupoPadraoId = s.GrupoPadraoId,
                            OBUId = s.OBUId,
                            StatusTagId = s.StatusTagId,
                            SolicitacaoImagem = new SolicitacaoImagem
                            {
                                Id = s.SolicitacaoImagem
                            }
                        },
                        ConfiguracaoAdesao = new ConfiguracaoAdesao
                        {
                            Id = s.ConfiguracaoAdesaoId,
                            AdesaoProvisoria = s.AdesaoProvisoria,
                            Categoria = new CategoriaVeiculo { Id = s.CategoriaVeiculoId }
                        },
                        Cliente = new Cliente
                        {
                            Id = s.ClienteId,
                            PessoaFisica = s.PessoaFisica
                        },
                        Veiculo = new Veiculo
                        {
                            Id = s.VeiculoId,
                            Categoria = new CategoriaVeiculo { Id = s.CategoriaVeiculoId, Codigo = s.CategoriaVeiculoId },
                            CategoriaConfirmada = s.CategoriaConfirmada,
                            ContagemConfirmacaoCategoria = s.ContagemConfirmacaoCategoria,
                            Placa = s.Placa,
                            DataConfirmacaoCategoria = s.DataConfirmacaoCategoria,
                            ContagemDivergenciaCategoriaConfirmada = s.ContagemDivergenciaCategoriaConfirmada
                        }
                    }))
                    .ForMember(d => d.CategoriaTag,
                        o => o.MapFrom(s => new CategoriaVeiculo { Id = s.CategoriaCobradaId }))
                    .ForMember(d => d.CategoriaCobrada,
                        o => o.MapFrom(s => new CategoriaVeiculo { Id = s.CategoriaCobradaId }))
                    .ForMember(d => d.CategoriaDetectada,
                        o => o.MapFrom(s => new CategoriaVeiculo { Id = s.CategoriaDetectadaId }));

            #endregion

            #region PistaPracaConveniadoDto => PassagemPendenteArtesp
            CreateMap<PistaPracaConveniadoDto, PassagemPendenteArtesp>()
                    .ForMember(d => d.Praca, o => o.MapFrom(s => new Praca
                    {
                        Id = s.PracaId,
                        CodigoPraca = s.CodigoPraca,
                        IdentificacaoPraca = s.IdentificacaoPraca,
                        TempoAtualizacaoPista = s.TempoAtualizacaoPista,
                        TempoRetornoPraca = s.TempoRetornoPraca.TryToInt()
                    }))
                    .ForMember(d => d.Conveniado, o => o.MapFrom(s => new Conveniado
                    {
                        Id = s.ConveniadoId,
                        CodigoProtocoloArtesp = s.CodigoProtocoloArtesp.TryToInt(),
                        HabilitarValidacaoTarifa = s.HabilitarValidacaoTarifa,
                        HabilitarConfirmacaoCategoria = s.HabilitarConfirmacaoCategoria,
                        NomeFantasia = s.NomeFantasia,
                        NomeFatura = s.NomeFatura,
                        RazaoSocial = s.RazaoSocial,
                        ListaDeParaCategoriaVeiculoId = s.ListaDeParaCategoriaVeiculoId.TryToInt(),
                        CodigoProtocolo = s.CodigoProtocolo
                    }))
                    .ForMember(d => d.Pista, o => o.MapFrom(s => new Pista
                    {
                        Id = s.PistaId,
                        CodigoPista = s.CodigoPista
                    }));

            #endregion

            #region TagAdesaoDto => PassagemPendenteArtesp
            CreateMap<TagAdesaoDto, PassagemPendenteArtesp>()
                    .ForMember(d => d.Placa, o => o.Ignore())
                    .ForMember(d => d.Adesao, o => o.MapFrom(s => new Adesao
                    {
                        Id = s.AdesaoId,
                        DataCancelamento = s.DataCancelamento,
                        PlanoId = s.PlanoId,
                        SaldoId = s.SaldoId,
                        StatusId = s.StatusId,
                        Tag = new Tag
                        {
                            Id = s.TagId,
                            GrupoPadraoId = s.GrupoPadraoId,
                            OBUId = s.OBUId,
                            StatusTagId = s.StatusTagId,
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
                            Categoria = new CategoriaVeiculo { Id = s.CategoriaVeiculoId, Codigo = s.CategoriaVeiculoId },
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
