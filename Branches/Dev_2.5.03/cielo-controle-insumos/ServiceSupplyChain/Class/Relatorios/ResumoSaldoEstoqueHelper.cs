using ServiceSupplyChain.DTO;
using ServiceSupplyChain.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.Class
{
    public class ResumoSaldoEstoqueHelper
    {
        private static ConnectionHelper _connection;

        public ResumoSaldoEstoqueHelper(ConnectionHelper connection)
        {
            _connection = connection;
        }

        private string GetSqlFirebird()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT S.id_deposito, s.id_local_estoque, S.ID_PRODUTO, SUM(S.qt_liberado) qt_produto");
            sb.AppendLine("  FROM TB_SALDO_ESTOQUE S");
            sb.AppendLine("WHERE S.ID_DEPOSITO IN (" + GetListCD() + ")");
            sb.AppendLine("GROUP BY S.id_deposito, s.id_local_estoque, S.ID_PRODUTO");
            return sb.ToString();
        }

        public string GetListCD()
        {
            var lista = _connection.SQLServerContext.TB_DEPOSITO_CD.Select(s => new { s.ID_DEPOSITO }).ToList();
            var retorno = "";
            foreach (var item in lista)
            {
                if (retorno != "")
                {
                    retorno = retorno + ", ";
                }
                retorno = retorno + item.ID_DEPOSITO.ToString();
            }

            return retorno;
        }

        public void ReadData()
        {
            LogHelper.Log("Gerando dados saldo de estoque");
            LogHelper.Log(GetSqlFirebird());

            var dataBase = _connection.DataBase.AddDays(1);
            for (var dia = 1; dia <= 1; dia++)
            {
                dataBase = dataBase.AddDays(-1);

                // para garantir que não está sendo duplicado, iremos excluir os registros do dia, se houver
                _connection.SQLServerContext.Database.ExecuteSqlCommand(string.Format("delete from tb_saldo_estoque where day(dt_saldo_estoque) = {0} and month(dt_saldo_estoque) = {1} and year(dt_saldo_estoque) = {2}", dataBase.Day, dataBase.Month, dataBase.Year));

                List<ResumoSaldoEstoqueDTO> itens = _connection.FirebirdContext.Database.SqlQuery<ResumoSaldoEstoqueDTO>(GetSqlFirebird()).ToList();
                var listaProd = _connection.SQLServerContext.TB_PRODUTO.Select(s => new { s.ID_PRODUTO, s.CD_PRODUTO }).ToList();
                StringBuilder insert = new StringBuilder();
                foreach (var item in itens)
                {
                    LogHelper.Process();

                    var prod = listaProd.Where(p => p.CD_PRODUTO == item.ID_PRODUTO).FirstOrDefault();

                    var str = string.Format("insert into tb_saldo_estoque (dt_saldo_estoque, id_deposito, id_local_estoque, id_produto, qt_produto) values (CONVERT(DATETIME,'{0}',111),{1},{2},'{3}',{4});",
                                                     dataBase.Year.ToString() + "-" + dataBase.Month.ToString() + "-" + dataBase.Day.ToString(),
                                                     item.ID_DEPOSITO, item.ID_LOCAL_ESTOQUE, prod.ID_PRODUTO, item.QT_PRODUTO);
                    insert.AppendLine(str);

                    //var itemRel = new TB_RESUMO_SALDO_ESTOQUE();
                    //itemRel.DT_RESUMO = dataBase;
                    //itemRel.ID_DEPOSITO = item.ID_DEPOSITO;
                    //itemRel.ID_PRODUTO = item.ID_PRODUTO;
                    //itemRel.QT_ESTOQUE = item.QT_PRODUTO;
                    //_connection.SQLServerContext.TB_RESUMO_SALDO_ESTOQUE.Add(itemRel);
                }

                //_connection.SQLServerContext.SaveChanges();
                _connection.SQLServerContext.Database.ExecuteSqlCommand(insert.ToString());
            }

            LogHelper.Log("Saldo de estoque gerado com sucesso");

        }
    }
}
