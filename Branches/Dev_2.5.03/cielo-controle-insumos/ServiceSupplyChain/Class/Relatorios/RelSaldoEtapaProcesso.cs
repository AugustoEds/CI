using ServiceSupplyChain.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.Class.Relatorios
{
    public class RelSaldoEtapaProcesso
    {
        private static ConnectionHelper _connection;

        public RelSaldoEtapaProcesso(ConnectionHelper connection)
        {
            _connection = connection;
        }

        public void ReadData()
        {
            LogHelper.Log("Gerando dados relatório de saldo por etapa do processo");
            var dataBase = _connection.DataBase;
            LogHelper.Log(GetSqlFirebird());

            List<TB_SALDO_ETAPA_PROCESSO> dados = _connection.FirebirdContext.Database.SqlQuery<TB_SALDO_ETAPA_PROCESSO>(GetSqlFirebird()).ToList();
            LogHelper.Log($"{dados.Count()} registros encontrados");
            var cont = 0;
            var sqlInsert = new StringBuilder();
            sqlInsert.AppendLine("BEGIN TRANSACTION");
            sqlInsert.AppendLine($"delete from TB_SALDO_ETAPA_PROCESSO where dt_relatorio = convert(date,'{dataBase.Day}/{dataBase.Month}/{dataBase.Year}',103)");
            foreach (var item in dados)
            {
                cont++;
                if (cont % 1000 == 0)
                {
                    LogHelper.Log($"{cont} de {dados.Count()}");
                }
                LogHelper.Process();


                sqlInsert.AppendLine($"INSERT INTO [dbo].[TB_SALDO_ETAPA_PROCESSO]");
                sqlInsert.AppendLine($"           ([DT_RELATORIO]");
                sqlInsert.AppendLine($"           ,[ID_DEPOSITO]");
                sqlInsert.AppendLine($"           ,[DS_DEPOSITO]");
                sqlInsert.AppendLine($"           ,[ID_LOCAL_ESTOQUE]");
                sqlInsert.AppendLine($"           ,[DS_LOCAL_ESTOQUE]");
                sqlInsert.AppendLine($"           ,[ID_PRODUTO]");
                sqlInsert.AppendLine($"           ,[DS_PRODUTO]");
                sqlInsert.AppendLine($"           ,[CODIGO_SAP]");
                sqlInsert.AppendLine($"           ,[ID_GRUPO]");
                sqlInsert.AppendLine($"           ,[DS_GRUPO]");
                sqlInsert.AppendLine($"           ,[ID_PROCESSO]");
                sqlInsert.AppendLine($"           ,[DS_PROCESSO]");
                sqlInsert.AppendLine($"           ,[ID_STATUS_CAIXA]");
                sqlInsert.AppendLine($"           ,[DS_STATUS_CAIXA]");
                sqlInsert.AppendLine($"           ,[QT_ITENS])");
                sqlInsert.AppendLine($"     VALUES");
                sqlInsert.AppendLine($"           (convert(date,'{item.DT_RELATORIO.Day}/{item.DT_RELATORIO.Month}/{item.DT_RELATORIO.Year}',103)");
                sqlInsert.AppendLine($"           ,'{item.ID_DEPOSITO}'");
                sqlInsert.AppendLine($"           ,'{item.DS_DEPOSITO}'");
                sqlInsert.AppendLine($"           ,'{item.ID_LOCAL_ESTOQUE}'");
                sqlInsert.AppendLine($"           ,'{item.DS_LOCAL_ESTOQUE}'");
                sqlInsert.AppendLine($"           ,'{item.ID_PRODUTO}'");
                sqlInsert.AppendLine($"           ,'{item.DS_PRODUTO}'");
                sqlInsert.AppendLine($"           ,'{item.CODIGO_SAP}'");
                sqlInsert.AppendLine($"           ,'{item.ID_GRUPO}'");
                sqlInsert.AppendLine($"           ,'{item.DS_GRUPO}'");
                sqlInsert.AppendLine($"           ,'{item.ID_PROCESSO}'");
                sqlInsert.AppendLine($"           ,'{item.DS_PROCESSO}'");
                sqlInsert.AppendLine($"           ,'{item.ID_STATUS_CAIXA}'");
                sqlInsert.AppendLine($"           ,'{item.DS_STATUS_CAIXA}'");
                sqlInsert.AppendLine($"           ,'{item.QT_ITENS}');");
                sqlInsert.AppendLine($"");

            }
            sqlInsert.AppendLine("COMMIT");
            Console.WriteLine($"{DateTime.Now} - Gravando dados no banco de dados");
            _connection.SQLServerContext.Database.ExecuteSqlCommand(sqlInsert.ToString());

            LogHelper.Log($"{DateTime.Now} - Relatório relatório de saldo por etapa do processo com sucesso");

        }

        private string GetSqlFirebird()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"select cast('{_connection.DataBase.Month}-{_connection.DataBase.Day}-{_connection.DataBase.Year}' as date) dt_relatorio");
            sb.AppendLine("      ,escaddep.codigo id_deposito");
            sb.AppendLine("      ,escaddep.descricao ds_deposito");
            sb.AppendLine("      ,tb_local_estoque.id_local_estoque");
            sb.AppendLine("      ,tb_local_estoque.ds_local_estoque");
            sb.AppendLine("      ,escadpro.codigo id_produto");
            sb.AppendLine("      ,escadpro.descricao ds_produto");
            sb.AppendLine("      ,escadpro.codigo_sap");
            sb.AppendLine("      ,escadpro.grupo id_grupo");
            sb.AppendLine("      ,esgrupro.descricao ds_grupo");
            //sb.AppendLine("      ,tb_processo.id_processo");
            //sb.AppendLine("      ,tb_processo.ds_processo");
            sb.AppendLine("      ,tb_status_caixa.id_status_caixa");
            sb.AppendLine("      ,tb_status_caixa.ds_status_caixa");
            sb.AppendLine("      ,sum(tb_caixa.qt_itens) qt_itens");
            sb.AppendLine("  from tb_caixa");
            sb.AppendLine("left join tb_status_caixa on tb_status_caixa.id_status_caixa = tb_caixa.id_status_caixa");
            //sb.AppendLine("left join tb_processo on tb_processo.id_processo = tb_status_caixa.id_processo");
            sb.AppendLine("left join tb_local_estoque on tb_local_estoque.id_local_estoque = tb_status_caixa.id_local_estoque");
            sb.AppendLine("left join tb_prec_rec_item on tb_prec_rec_item.id_pre_rec_item = tb_caixa.id_pre_rec_item");
            sb.AppendLine("left join tb_pre_rec on tb_pre_rec.id_pre_rec = tb_prec_rec_item.id_pre_rec");
            sb.AppendLine("left join escaddep on escaddep.codigo = tb_pre_rec.id_deposito_destino");
            sb.AppendLine("left join escadpro on escadpro.codigo = tb_prec_rec_item.id_item_yep");
            sb.AppendLine("left join esgrupro on esgrupro.codigo = escadpro.grupo");
            sb.AppendLine(" where tb_caixa.id_caixa > 0");
            sb.AppendLine("   and tb_status_caixa.id_processo is not null");
            sb.AppendLine("group by escaddep.codigo");
            sb.AppendLine("          ,escaddep.descricao");
            sb.AppendLine("          ,tb_local_estoque.id_local_estoque");
            sb.AppendLine("          ,tb_local_estoque.ds_local_estoque");
            sb.AppendLine("          ,escadpro.codigo");
            sb.AppendLine("          ,escadpro.descricao");
            sb.AppendLine("          ,escadpro.codigo_sap");
            sb.AppendLine("          ,escadpro.grupo");
            sb.AppendLine("          ,esgrupro.descricao");
            //sb.AppendLine("          ,tb_processo.id_processo");
            //sb.AppendLine("          ,tb_processo.ds_processo");
            sb.AppendLine("          ,tb_status_caixa.id_status_caixa");
            sb.AppendLine("          ,tb_status_caixa.ds_status_caixa");

            return sb.ToString();
        }
    }
}
