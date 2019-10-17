using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.ActorsMessages.Artesp
{
    public abstract class PassagemArtespMessageBase
    {
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }

        public override string ToString()
        {
            return $"Passagem ID: {PassagemPendenteArtesp.MensagemItemId} Conveniado {PassagemPendenteArtesp.Conveniado.CodigoProtocoloArtesp}";
        }
    }
}