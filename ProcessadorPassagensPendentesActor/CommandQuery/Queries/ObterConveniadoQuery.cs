using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Cadastros.Conveniados.Backend.CommonQuery.Query;
using ConectCar.Cadastros.Domain.Model;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Framework.Infrastructure.Data.DataProviders;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterConveniadoQuery
    {
        private DbConnectionDataSource _dataSourceConectSysReadOnly;
        readonly DbConnectionDataSource _dataSourceFallBack;

        public ObterConveniadoQuery(DbConnectionDataSource dbSysReadOnly, DbConnectionDataSource dbSysFallBack)
        {
            _dataSourceConectSysReadOnly = dbSysReadOnly;
            _dataSourceFallBack = dbSysFallBack;
        }

        public ConcessionariaModel Execute(int codigoProtocolo)
        {
            var obterConveniado = new ObterConcessionariasEDIQuery(true, _dataSourceConectSysReadOnly,
                _dataSourceFallBack);

            var conveniados = obterConveniado.Execute();
            return conveniados.FirstOrDefault(c =>c.CodigoProtocolo == codigoProtocolo);
        }
    }
}
