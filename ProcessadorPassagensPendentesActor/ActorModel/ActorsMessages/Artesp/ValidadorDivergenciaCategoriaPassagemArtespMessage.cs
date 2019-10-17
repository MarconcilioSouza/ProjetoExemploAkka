using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;
using System.Collections.Generic;

namespace ProcessadorPassagensActors.ActorsMessages.Artesp
{
    public class ValidadorDivergenciaCategoriaPassagemArtespMessage
    {

        /// <summary>
        /// Passagem 
        /// </summary>
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }
        public List<DetalheViagem> ViagensAgendadas { get; set; }

    }
}