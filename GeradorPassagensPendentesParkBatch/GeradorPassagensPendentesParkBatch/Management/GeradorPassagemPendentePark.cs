using ConectCar.Cadastros.Conveniados.Backend.CommonQuery.Query;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Framework.Infrastructure.Log;
using GeradorPassagensPendentesParkBatch.CommandQuery.Handlers;
using GeradorPassagensPendentesParkBatch.CommandQuery.Resources;
using GeradorPassagensPendentesParkBatch.Management.Interfaces;
using System;
using System.Threading.Tasks;

namespace GeradorPassagensPendentesParkBatch.Management
{
    public class GeradorPassagemPendentePark : Loggable, IGeradorPassagemPendentePark
    {
        #region [Properties]
        private GeradorPassagemPendenteParkHandler _geradorPassagemPendenteParkHandler { get; }

        #endregion [Properties]

        #region [Ctor]

        /// <summary>
        /// Inicializa o gerador de passagens pendentes.
        /// </summary>
        public GeradorPassagemPendentePark()
        {
            _geradorPassagemPendenteParkHandler = new GeradorPassagemPendenteParkHandler();
        }

        #endregion [Ctor]

        public async Task ExecuteAsync()
        {
            try
            {
                await GerarPassagensPendentes();
            }
            catch (Exception e)
            {
                Log.Error(string.Format(GeradorPassagemPendenteParkResource.Error, e.Message), e);
            }
        }

        private async Task GerarPassagensPendentes()
        {
            try
            {
                Log.Info(GeradorPassagemPendenteParkResource.InicioProcesso);
                await _geradorPassagemPendenteParkHandler.GerarPassagensPendentesAsync();
                Log.Info(GeradorPassagemPendenteParkResource.FinalProcesso);
            }
            catch (Exception e)
            {
                Log.Error(string.Format(GeradorPassagemPendenteParkResource.Error, e.Message), e);
            }
        }
    }
}