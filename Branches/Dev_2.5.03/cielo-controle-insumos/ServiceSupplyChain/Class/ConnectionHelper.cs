using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceSupplyChain.Firebird;
using ServiceSupplyChain.SQLServer;
using System.Configuration;

namespace ServiceSupplyChain.Class
{
    public class ConnectionHelper
    {
        private DateTime _dataBase;
        private FirebirdEntities _firebirdContext;
        private SQLServerEntities _sqlServerContext;
        private int _countDays;

        public ConnectionHelper()
        {
            LogHelper.Log("Criando conexões com os bancos de dados");

            LogHelper.Log("Firebird");
            _firebirdContext = new Firebird.FirebirdEntities();
            LogHelper.Log(_firebirdContext.Database.Connection.ConnectionString);

            LogHelper.Log("SQL Server");
            _sqlServerContext = new SQLServer.SQLServerEntities();
            LogHelper.Log(_sqlServerContext.Database.Connection.ConnectionString);

            _dataBase = DateTime.Now.AddDays(-1).Date;

            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            _countDays = int.Parse(config.AppSettings.Settings["DaysToLoad"].Value);
        }

        public FirebirdEntities FirebirdContext { get { return _firebirdContext; } }
        public SQLServerEntities SQLServerContext { get { return _sqlServerContext; } }
        public DateTime DataBase { get { return _dataBase; } }
        public int CountDays { get { return _countDays; } }
    }
}
