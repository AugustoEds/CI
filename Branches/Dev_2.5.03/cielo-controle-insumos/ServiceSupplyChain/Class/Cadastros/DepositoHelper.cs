using ServiceSupplyChain.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.Class
{
    public class DepositoHelper
    {
        private static ConnectionHelper _connection;

        public DepositoHelper(ConnectionHelper connection)
        {
            _connection = connection;
        }

        public void Sync()
        {
            LogHelper.Log("Sincronizando cadastro de Depósitos");
            var depsFirebird = _connection.FirebirdContext.ESCADDEP;
            LogHelper.Log(String.Format("{0} registros a serem atualizados", depsFirebird.Count()));
            var depsSQLServer = _connection.SQLServerContext.TB_DEPOSITO;

            foreach (var depF in depsFirebird)
            {
                LogHelper.Process();
                var depS = depsSQLServer.Find(depF.CODIGO);
                if (depS == null)
                {
                    depS = new TB_DEPOSITO();
                    depS.ID_DEPOSITO = depF.CODIGO;
                    depS.DS_DEPOSITO = depF.DESCRICAO;
                    _connection.SQLServerContext.TB_DEPOSITO.Add(depS);
                }
                
            }

            _connection.SQLServerContext.SaveChanges();
            LogHelper.Log("Atualização dos depósitos concluído");

        }
    }
}
