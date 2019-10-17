using ConectCar.Cadastros.Conveniados.Backend.CommonQuery.Query;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Framework.Infrastructure.Log;
using GeradorPassagensPendentesEDIBatch.CommandQuery.Handlers;
using GeradorPassagensPendentesEDIBatch.CommandQuery.Resources;
using GeradorPassagensPendentesEDIBatch.Management.Interfaces;
using System;
using System.Threading.Tasks;

namespace GeradorPassagensPendentesEDIBatch.Management
{
    public class GeradorPassagemPendenteEDI : Loggable, IGeradorPassagemPendenteEDI
    {
        #region [Properties]

        public DbConnectionDataSource DataSource { get; }
        public DbConnectionDataSource ReadOnlyDataSource { get; }

        #endregion [Properties]

        #region [Ctor]

        /// <summary>
        /// Inicializa o gerador de passagens pendentes.
        /// </summary>
        public GeradorPassagemPendenteEDI()
        {
            var dataProvider = new DbConnectionDataSourceProvider();

            ReadOnlyDataSource = dataProvider.GetDataSource(DbConnectionDataSourceType.ConectSysReadOnly);
            DataSource = dataProvider.GetDataSource(DbConnectionDataSourceType.ConectSys);
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
                Log.Error(string.Format(GeradorPassagemPendenteEDIResource.Error, e.Message), e);
            }
        }

        private async Task GerarPassagensPendentes()
        {
            try
            {
                Log.Info(GeradorPassagemPendenteEDIResource.InicioProcesso);
                var handler = new GeradorPassagemPendenteEdiHandler();
                await handler.GerarPassagensPendentesAsync();

                Log.Info(GeradorPassagemPendenteEDIResource.FinalProcesso);
            }
            catch (Exception e)
            {
                Log.Error(string.Format(GeradorPassagemPendenteEDIResource.Error, e.Message), e);
            }
        }
    }
}