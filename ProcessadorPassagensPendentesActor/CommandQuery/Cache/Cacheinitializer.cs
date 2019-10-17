using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using ConectCar.Framework.Infrastructure.Log;

namespace ProcessadorPassagensActors.CommandQuery.Cache
{
    public static class Cacheinitializer
    {
        private static ILog _log;

        public static void Iniciar()
        {
            _log = LogManager.GetLogger(typeof(Cacheinitializer));

            // Inicia o Cache de Categorias de Veiculos
            _log.Debug($"Actor System - Início - Carregar Categorias Veículos.");
            var qtdCategorias = CategoriaVeiculoCacheRepository.Listar().Count;
            _log.Debug($"Actor System - Fim - Carregar Categorias Veículos. Total: {qtdCategorias}.");

            _log.Debug($"Actor System - Início - Carregar Pistas, Praças e Conveniados.");
            var qtdPistas = PistaPracaConveniadoArtespCacheRepository.Listar().Count;
            _log.Debug($"Actor System - Fim - Carregar Pistas, Praças e Conveniados. Total: {qtdPistas}.");

            _log.Debug($"Actor System - Início - Carregar configurações de sistema.");
            var qtdConfiguracoes = ConfiguracaoSistemaCacheRepository.Listar().Count;
            _log.Debug($"Actor System - Fim - Carregar configurações de sistema. Total: {qtdConfiguracoes}.");

            _log.Debug($"Actor System - Início - Carregar configurações de feriado.");
            var qtdFeriados = FeriadoCacheRepository.Listar().Count;
            _log.Debug($"Actor System - Fim - Carregar configurações de feriado. Total: {qtdFeriados}.");
        }


    }
}
