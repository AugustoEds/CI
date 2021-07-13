using ServiceSupplyChain.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.Class.Relatorios
{
    public class RelAproveitamentoTriagem
    {
        private static ConnectionHelper _connection;

        public RelAproveitamentoTriagem(ConnectionHelper connection)
        {
            _connection = connection;
        }

        private string GetSqlFirebirdAnalitico(int Mes, int Ano)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"select a.dt_recebimento dt_resumo");
            sb.AppendLine($"              , a.id_produto");
            sb.AppendLine($"              , a.ds_produto");
            sb.AppendLine($"              , a.codigo_sap");
            sb.AppendLine($"              , a.cd_grupo");
            sb.AppendLine($"              , a.ds_grupo");
            sb.AppendLine($"              , sum(a.qt_produto) qt_produto");
            sb.AppendLine($"        from (select cast(p.dt_recebimento as date) dt_recebimento");
            sb.AppendLine($"                    , pr.codigo id_produto");
            sb.AppendLine($"                    , pr.descricao ds_produto");
            sb.AppendLine($"                    , pr.codigo_sap");
            sb.AppendLine($"                    , gru.codigo cd_grupo");
            sb.AppendLine($"                    , gru.descricao ds_grupo");
            sb.AppendLine($"                    , pri.qt_volume * pri.qt_unidade_volume as qt_produto");
            sb.AppendLine($"                from tb_pre_rec p");
            sb.AppendLine($"              left join tb_prec_rec_item pri on pri.id_pre_rec = p.id_pre_rec");
            sb.AppendLine($"              left join tb_config_gerais c on c.id_config = c.id_config");
            sb.AppendLine($"              left join escadpro pr on pr.codigo = pri.id_item_yep");
            sb.AppendLine($"              left join esgrupro gru on gru.codigo = pr.grupo");
            sb.AppendLine($"               where p.id_fluxo = 4");
            sb.AppendLine($"                 and pr.grupo in ('80','83')");
            sb.AppendLine($"                 and extract(month from p.dt_recebimento) = {Mes}");
            sb.AppendLine($"                 and extract(year from p.dt_recebimento) = {Ano}) a");
            sb.AppendLine($"group by a.dt_recebimento, a.id_produto, a.ds_produto, a.codigo_sap, a.cd_grupo, a.ds_grupo");
            sb.AppendLine($"order by a.dt_recebimento, a.id_produto, a.cd_grupo");

            return sb.ToString();
        }

        private string GetSqlFirebird(int Mes, int Ano)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"select a.dt_resumo");
            sb.AppendLine($"      ,a.id_produto");
            sb.AppendLine($"      ,a.ds_produto");
            sb.AppendLine($"      ,a.cd_sub_grupo");
            sb.AppendLine($"      ,a.ds_sub_grupo");
            sb.AppendLine($"      ,sum(a.qt_rpa) qt_rpa");
            sb.AppendLine($"      ,sum(a.qt_rpt_reparo) qt_rpt_reparo");
            sb.AppendLine($"  from (select a.dt_recebimento dt_resumo");
            sb.AppendLine($"              , p.npa id_produto");
            sb.AppendLine($"              , a.ds_produto");
            sb.AppendLine($"              , a.cd_sub_grupo");
            sb.AppendLine($"              , a.ds_sub_grupo");
            sb.AppendLine($"              , sum(case when p.grupo = '80' then a.qt_produto else 0 end) qt_rpa");
            sb.AppendLine($"              , sum(case when p.grupo = '83' then a.qt_produto else 0 end) qt_rpt_reparo");
            sb.AppendLine($"        from (select cast(p.dt_recebimento as date) dt_recebimento");
            sb.AppendLine($"                    , pr.codigo id_produto");
            sb.AppendLine($"                    , pr.descricao ds_produto");
            sb.AppendLine($"                    , sub.codigo cd_sub_grupo");
            sb.AppendLine($"                    , sub.descricao ds_sub_grupo");
            sb.AppendLine($"                    , pri.qt_volume * pri.qt_unidade_volume as qt_produto");
            sb.AppendLine($"                from tb_pre_rec p");
            sb.AppendLine($"              left join tb_prec_rec_item pri on pri.id_pre_rec = p.id_pre_rec");
            sb.AppendLine($"              left join tb_config_gerais c on c.id_config = c.id_config");
            sb.AppendLine($"              left join escadpro pr on pr.codigo = pri.id_item_yep");
            sb.AppendLine($"              left join essubpro sub on sub.codigo = pr.sub_grupo");
            sb.AppendLine($"               where p.id_fluxo = 4");
            sb.AppendLine($"                 and pr.grupo in ('80','83')");
            sb.AppendLine($"                 and extract(month from p.dt_recebimento) = {Mes}");
            sb.AppendLine($"                 and extract(year from p.dt_recebimento) = {Ano}");
            sb.AppendLine($"        ) a");
            sb.AppendLine($"        left join escadpro p on p.codigo = a.id_produto");
            sb.AppendLine($"        group by a.dt_recebimento");
            sb.AppendLine($"                , p.npa");
            sb.AppendLine($"                , a.ds_produto");
            sb.AppendLine($"                , p.grupo");
            sb.AppendLine($"                , a.cd_sub_grupo");
            sb.AppendLine($"                , a.ds_sub_grupo) a");
            sb.AppendLine($"group by a.dt_resumo, a.id_produto, a.ds_produto ,a.cd_sub_grupo ,a.ds_sub_grupo");
            sb.AppendLine($" order by a.dt_resumo, a.id_produto");

            return sb.ToString();
        }

        public void ReadData()
        {
            LogHelper.Log("Gerando dados Aproveitamento Triagem");
            LogHelper.Log(GetSqlFirebird(_connection.DataBase.Month, _connection.DataBase.Year));

            var database = _connection.DataBase.AddMonths(-13);
            for (var i = 1; i <= 13; i++)
            {
                _connection = new ConnectionHelper();
                database = database.AddMonths(1);

                // para garantir que não está sendo duplicado, iremos excluir os registros do dia, se houver
                _connection.SQLServerContext.Database.ExecuteSqlCommand(string.Format("delete from TB_APROVEITAMENTO_TRIAGEM where month(DT_RESUMO) = {0} and year(DT_RESUMO) = {1}", database.Month, database.Year));
                List<TB_APROVEITAMENTO_TRIAGEM> itens = _connection.FirebirdContext.Database.SqlQuery<TB_APROVEITAMENTO_TRIAGEM>(GetSqlFirebird(database.Month, database.Year)).ToList();
                foreach (var item in itens)
                {
                    LogHelper.Process();
                    item.QT_TOTAL = item.QT_RPA + item.QT_RPT_REPARO;
                    _connection.SQLServerContext.TB_APROVEITAMENTO_TRIAGEM.Add(item);
                }
                _connection.SQLServerContext.SaveChanges();

                // Agora faremos o resumo dos dados analíticos
                _connection.SQLServerContext.Database.ExecuteSqlCommand(string.Format("delete from TB_APROVEITAMENTO_TRIAGEM_ANALITICO where month(DT_RESUMO) = {0} and year(DT_RESUMO) = {1}", database.Month, database.Year));
                List<TB_APROVEITAMENTO_TRIAGEM_ANALITICO> itensAnalitico = _connection.FirebirdContext.Database.SqlQuery<TB_APROVEITAMENTO_TRIAGEM_ANALITICO>(GetSqlFirebirdAnalitico(database.Month, database.Year)).ToList();
                foreach (var item in itensAnalitico)
                {
                    LogHelper.Process();
                    _connection.SQLServerContext.TB_APROVEITAMENTO_TRIAGEM_ANALITICO.Add(item);
                }
                _connection.SQLServerContext.SaveChanges();
            }


            LogHelper.Log("Relatório indice aproveitamento triagem gerado com sucesso");

        }
    }
}
