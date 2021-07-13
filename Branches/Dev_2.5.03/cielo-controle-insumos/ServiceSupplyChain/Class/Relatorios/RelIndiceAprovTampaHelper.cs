using ServiceSupplyChain.DTO;
using ServiceSupplyChain.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.Class
{
    public class RelIndiceAprovTampaHelper
    {
        private static ConnectionHelper _connection;

        public RelIndiceAprovTampaHelper(ConnectionHelper connection)
        {
            _connection = connection;
        }

        private string GetSqlFirebird()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("select a.dt_recebimento dt_resumo");
            sb.AppendLine("      , a.id_produto");
            sb.AppendLine("      , a.ds_produto");
            sb.AppendLine("      , sum(case when p.grupo = '80' then a.qt_produto else 0 end) qt_rpa");
            sb.AppendLine("      , sum(case when p.grupo = '86' then a.qt_produto else 0 end) qt_rpt");
            sb.AppendLine("from (select cast(p.dt_recebimento as date) dt_recebimento");
            sb.AppendLine("            , pr.codigo id_produto");
            sb.AppendLine("            , pr.descricao ds_produto");
            sb.AppendLine("            , pri.qt_volume * pri.qt_unidade_volume as qt_produto");
            sb.AppendLine("        from tb_pre_rec p");
            sb.AppendLine("      left join tb_prec_rec_item pri on pri.id_pre_rec = p.id_pre_rec");
            sb.AppendLine("      left join tb_config_gerais c on c.id_config = c.id_config");
            sb.AppendLine("      left join escadpro pr on pr.codigo = pri.id_item_yep");
            sb.AppendLine("       where p.id_fluxo = 4");
            sb.AppendLine("         and pr.grupo in ('80','86')");
            sb.AppendLine("         and extract(month from p.dt_recebimento) = {0}");
            sb.AppendLine("         and extract(year from p.dt_recebimento) = {1}");
            sb.AppendLine(") a");
            sb.AppendLine("left join escadpro p on p.codigo = a.id_produto");
            sb.AppendLine("group by a.dt_recebimento");
            sb.AppendLine("        , a.id_produto");
            sb.AppendLine("        , a.ds_produto");

            return sb.ToString();
        }

        public void ReadData()
        {
            //LogHelper.Log("Gerando dados indice aprovação de tampa");
            //LogHelper.Log(GetSqlFirebird());

            //var dataBase = DateTime.Today;
            //// para garantir que não está sendo duplicado, iremos excluir os registros do dia, se houver
            //_connection.SQLServerContext.Database.ExecuteSqlCommand(string.Format("delete from TB_RESUMO_APROV_TAMPA where month(DT_RESUMO) = {0} and year(DT_RESUMO) = {1}", dataBase.Month, dataBase.Year));

            //List<IndiceAprovTampaDTO> itens = _connection.FirebirdContext.Database.SqlQuery<IndiceAprovTampaDTO>(string.Format(GetSqlFirebird(), dataBase.Month, dataBase.Year)).ToList();
            //foreach (var item in itens)
            //{
            //    LogHelper.Process();

            //    var itemRel = new TB_RESUMO_APROV_TAMPA();
            //    itemRel.DT_RESUMO = item.DT_RESUMO;
            //    itemRel.ID_PRODUTO = item.ID_PRODUTO;
            //    itemRel.QT_RPA = item.QT_RPA;
            //    itemRel.QT_RPT = item.QT_RPT;

            //    _connection.SQLServerContext.TB_RESUMO_APROV_TAMPA.Add(itemRel);
            //}

            //_connection.SQLServerContext.SaveChanges();

            //LogHelper.Log("Relatório indice aprovação de tampa gerado com sucesso");

        }
    }
}
