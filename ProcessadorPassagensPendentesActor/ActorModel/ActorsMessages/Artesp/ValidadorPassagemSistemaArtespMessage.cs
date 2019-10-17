using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.ActorsMessages.Artesp
{
    public class ValidadorPassagemSistemaArtespMessage
    {
        
        /// <summary>
        /// Passagem 
        /// </summary>
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }

    }
}