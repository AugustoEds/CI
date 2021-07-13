using ServiceSupplyChain.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.Class
{
    public class ConfigGeraisHelper
    {
        private static ConnectionHelper _connection;

        public ConfigGeraisHelper(ConnectionHelper connection)
        {
            _connection = connection;
        }

        private int GetIdGrupo(string CdGrupo)
        {
            int retorno = 0;

            var grupo = _connection.SQLServerContext.TB_GRUPO_PRODUTO.Where(p => p.CD_GRUPO_PRODUTO == CdGrupo).FirstOrDefault();
            if (grupo != null)
            {
                retorno = grupo.ID_GRUPO_PRODUTO;
            }

            return retorno;
        }

        public void Sync()
        {
            LogHelper.Log("Sincronizando cadastro de configurações gerais");
            LogHelper.Log(_connection.FirebirdContext.Database.Connection.ConnectionString);
            var configsFirebird = _connection.FirebirdContext.TB_CONFIG_GERAIS;
            LogHelper.Log(String.Format("{0} registros a serem atualizados", configsFirebird.Count()));
            var configsSQLServer = _connection.SQLServerContext.TB_CONFIG_GERAIS;

            foreach (var configF in configsFirebird)
            {
                var configS = configsSQLServer.Find(configF.ID_CONFIG);
                if (configS == null)
                {
                    configS = new TB_CONFIG_GERAIS();
                    configS.ID_CONFIG = configF.ID_CONFIG;
                    _connection.SQLServerContext.TB_CONFIG_GERAIS.Add(configS);
                }
                configS.ID_GRUPO_NPA = GetIdGrupo(configF.ID_GRUPO_NPA);
                configS.ID_GRUPO_RPA = GetIdGrupo(configF.ID_GRUPO_RPA);
                configS.ID_GRUPO_RPT_GARANTIA = GetIdGrupo(configF.ID_GRUPO_RPT_GARANTIA);
                configS.ID_GRUPO_RPT_REPARO = GetIdGrupo(configF.ID_GRUPO_RPT_REPARO);
                configS.ID_GRUPO_RPT_TRIAGEM = GetIdGrupo(configF.ID_GRUPO_TRIAGEM);
                configS.ID_GRUPO_SUCATA = GetIdGrupo(configF.ID_GRUPO_SUCATA);
                configS.ID_GRUPO_TEC_DESCONTINUADA = GetIdGrupo(configF.ID_GRUPO_TEC_DESCONTINUADA);
                configS.ID_DEPOSITO_SB = 3113;
            }

            _connection.SQLServerContext.SaveChanges();
            LogHelper.Log("Atualização dos configurações gerais concluído");

        }
    }
}
