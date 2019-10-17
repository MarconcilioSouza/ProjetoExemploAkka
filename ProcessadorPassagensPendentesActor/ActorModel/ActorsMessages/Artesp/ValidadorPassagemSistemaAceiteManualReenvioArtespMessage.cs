using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.ActorsMessages.Artesp
{
    public class ValidadorPassagemSistemaAceiteManualReenvioArtespMessage
    {
        
        /// <summary>
        /// Passagem 
        /// </summary>
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }

    }
}