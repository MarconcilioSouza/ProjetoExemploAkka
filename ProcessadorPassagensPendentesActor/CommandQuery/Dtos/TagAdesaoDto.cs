using System;

namespace ProcessadorPassagensActors.CommandQuery.Dtos
{
    public class TagAdesaoDto
    {
        public int TagId { get; set; }
        public int GrupoPadraoId { get; set; }
        public long OBUId { get; set; }
        public int EmissorId { get; set; }
        public int StatusTagId { get; set; }
        public bool Deletado { get; set; }
        public int AdesaoId { get; set; }
        public int SaldoId { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public int PlanoId { get; set; }
        public DateTime DataAdesao { get; set; }
        public bool AdesaoProvisoria { get; set; }
        public int CategoriaId { get; set; }
        public int ConfiguracaoAdesaoId { get; set; }
        public int ClienteId { get; set; }
        public bool PessoaFisica { get; set; }
        public bool UltimaCobrancaPaga { get; set; }
        public int StatusId { get; set; }
        public bool CategoriaConfirmada { get; set; }
        public int VeiculoId { get; set; }
        public int CategoriaVeiculoId { get; set; }
        public int ContagemConfirmacaoCategoria { get; set; }
        public int? ContagemDivergenciaCategoriaConfirmada { get; set; }
        public DateTime? DataConfirmacaoCategoria { get; set; }
        public string Placa { get; set; }
        public int SolicitacaoImagem { get; set; }
        public int AdesaoOrigemId { get; set; }
    }
}
