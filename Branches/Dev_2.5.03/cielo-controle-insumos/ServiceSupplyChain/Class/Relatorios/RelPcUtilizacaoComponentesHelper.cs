using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.Class.Relatorios
{
    public class RelPcUtilizacaoComponentesHelper
    {
        public class TB_REL_PC_UTILIZACAOEntity
        {
            public long ID_REL_PC_UTILIZACAO { get; set; }
            public DateTime DT_MOVIMENTO { get; set; }
            public string ID_GRUPO { get; set; }
            public string DS_GRUPO { get; set; }
            public string ID_PRODUTO { get; set; }
            public string DS_PRODUTO { get; set; }
            public string CODIGO_SAP { get; set; }
            public int QT_PRODUTO { get; set; }
        }

        private static ConnectionHelper _connection;

        public RelPcUtilizacaoComponentesHelper(ConnectionHelper connection)
        {
            _connection = connection;
        }

        public void ReadData()
        {
            LogHelper.Log("Gerando dados relatório de saldo por etapa do processo");
            var dataBase = _connection.DataBase;
            LogHelper.Log(GetSqlFirebird());

            List<TB_REL_PC_UTILIZACAOEntity> dados = _connection.FirebirdContext.Database.SqlQuery<TB_REL_PC_UTILIZACAOEntity>(GetSqlFirebird()).ToList();
            LogHelper.Log($"{dados.Count()} registros encontrados");
            var cont = 0;
            var sqlInsert = new StringBuilder();
            sqlInsert.AppendLine("BEGIN TRANSACTION");
            sqlInsert.AppendLine($"DELETE FROM TB_REL_PC_UTILIZACAO");
            foreach (var item in dados)
            {
                cont++;
                if (cont % 1000 == 0)
                {
                    LogHelper.Log($"{cont} de {dados.Count()}");
                }
                LogHelper.Process();


                sqlInsert.AppendLine($@"INSERT INTO [dbo].[TB_REL_PC_UTILIZACAO]");
                sqlInsert.AppendLine($"           ([DT_MOVIMENTO]");
                sqlInsert.AppendLine($"           ,[ID_GRUPO]");
                sqlInsert.AppendLine($"           ,[DS_GRUPO]");
                sqlInsert.AppendLine($"           ,[ID_PRODUTO]");
                sqlInsert.AppendLine($"           ,[DS_PRODUTO]");
                sqlInsert.AppendLine($"           ,[CODIGO_SAP]");
                sqlInsert.AppendLine($"           ,[QT_PRODUTO])");
                sqlInsert.AppendLine($"     VALUES");
                sqlInsert.AppendLine($"           (CONVERT(DATE,'{item.DT_MOVIMENTO.ToString("dd/MM/yyyy")}',103)");
                sqlInsert.AppendLine($"           ,'{item.ID_GRUPO}'");
                sqlInsert.AppendLine($"           ,'{item.DS_GRUPO}'");
                sqlInsert.AppendLine($"           ,'{item.ID_PRODUTO}'");
                sqlInsert.AppendLine($"           ,'{item.DS_PRODUTO}'");
                sqlInsert.AppendLine($"           ,'{item.CODIGO_SAP}'");
                sqlInsert.AppendLine($"           ,{item.QT_PRODUTO})");


                sqlInsert.AppendLine($"");

            }
            sqlInsert.AppendLine("COMMIT");
            Console.WriteLine($"{DateTime.Now} - Gravando dados no banco de dados");
            _connection.SQLServerContext.Database.ExecuteSqlCommand($"{sqlInsert}");

            LogHelper.Log($"{DateTime.Now} - Relatório relatório de saldo por etapa do processo com sucesso");

        }

        private string GetSqlFirebird()
        {
            var sb = new StringBuilder();
            sb.AppendLine("select *");
            sb.AppendLine("  from (select r.dt_romaneio dt_movimento");
            sb.AppendLine("              ,recitem.id_item_yep id_produto");
            sb.AppendLine("              ,p.descricao ds_produto");
            sb.AppendLine("              ,p.codigo_sap");
            sb.AppendLine("              ,g.codigo id_grupo");
            sb.AppendLine("              ,g.descricao ds_grupo");
            sb.AppendLine("              ,sum(c.qt_itens) qt_produto");
            sb.AppendLine("          from tb_romaneio r");
            sb.AppendLine("        inner join tb_romaneio_item ri on ri.id_romaneio = r.id_romaneio");
            sb.AppendLine("        inner join tb_romaneio_reserva rr on rr.id_romaneio_item = ri.id_romaneio_item");
            sb.AppendLine("        inner join tb_caixa c on c.id_caixa = rr.id_caixa");
            sb.AppendLine("        inner join tb_prec_rec_item recitem on recitem.id_pre_rec_item = c.id_pre_rec_item");
            sb.AppendLine("        left join escadpro p on p.codigo = recitem.id_item_yep");
            sb.AppendLine("        left join esgrupro g on g.codigo = p.grupo");
            sb.AppendLine("        left join tb_config_gerais cfg on cfg.id_config = cfg.id_config");
            sb.AppendLine("         where r.id_status_romaneio = 11");
            sb.AppendLine("           and p.grupo in (cfg.id_grupo_rpa, cfg.id_grupo_npa)");
            sb.AppendLine("        group by r.dt_romaneio");
            sb.AppendLine("              ,recitem.id_item_yep");
            sb.AppendLine("              ,p.descricao");
            sb.AppendLine("              ,p.codigo_sap");
            sb.AppendLine("              ,g.codigo");
            sb.AppendLine("              ,g.descricao) a");
            sb.AppendLine("order by dt_movimento, id_grupo, id_produto");

            return sb.ToString();
        }
    }
}