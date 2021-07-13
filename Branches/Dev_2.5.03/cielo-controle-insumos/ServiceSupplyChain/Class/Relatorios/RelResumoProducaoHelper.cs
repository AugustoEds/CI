using ServiceSupplyChain.DTO;
using ServiceSupplyChain.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.Class
{
    public class RelResumoProducaoHelper
    {
        private static ConnectionHelper _connection;

        public RelResumoProducaoHelper(ConnectionHelper connection)
        {
            _connection = connection;
        }

        private string GetSqlFirebird()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select m.data_sit_indoor DT_MOVIMENTO");
            sb.AppendLine("      , e.tipo DS_TIPO");
            sb.AppendLine("      , count(1) QT_PRODUCAO");
            sb.AppendLine("  from esmoveqp m WITH(NOLOCK) ");
            sb.AppendLine("left join escadeqp e WITH(NOLOCK) on e.numero_serie = m.numero_serie");
            sb.AppendLine("left join eseqpfab f WITH(NOLOCK) on f.codigo = e.fabricante");
            sb.AppendLine("left join eseqptip t WITH(NOLOCK) on t.codigo = e.tipo");
            sb.AppendLine("left join eseqpmod g WITH(NOLOCK) on g.codigo = e.modelo");
            sb.AppendLine("left join escadpro j WITH(NOLOCK) on j.codigo = e.item_estoque");
            sb.AppendLine("where m.situacao_indoor     = 'Recuperado embalado' and");
            sb.AppendLine("       m.deposito_destino    = 5 and");
            sb.AppendLine("      m.data_sit_indoor = '{0}'");
            sb.AppendLine("group by m.data_sit_indoor");
            sb.AppendLine("      , e.tipo");


            return sb.ToString();
        }

        public void ReadData()
        {
            LogHelper.Log("Gerando dados resumo da produção por modelo");
            LogHelper.Log(GetSqlFirebird());
            var dataBase = _connection.DataBase.AddDays(+1);

            for (var mes = 1; mes <= _connection.CountDays; mes++)
            {
                dataBase = dataBase.AddDays(-1);
                LogHelper.Log("");
                LogHelper.Log(dataBase.ToString());

                // para garantir que não está sendo duplicado, iremos excluir os registros do dia, se houver
                _connection.SQLServerContext.Database.ExecuteSqlCommand("delete from TB_REL_ACOMPANHAMENTO_PRODUCAO where YEAR(dt_resumo) = " + dataBase.Year.ToString() + " AND MONTH(DT_RESUMO) = " + dataBase.Month.ToString() + " AND DAY(DT_RESUMO) = "  + dataBase.Day.ToString());
                List<ProdModelDTO> itens = _connection.FirebirdContext.Database.SqlQuery<ProdModelDTO>(string.Format(GetSqlFirebird(), dataBase.ToString("dd.MM.yyyy"))).ToList();

                foreach (var item in itens)
                {
                    LogHelper.Process();

                    // Buscaremos o agrupamento do tipo
                    var tipo = _connection.SQLServerContext.TB_TIPO_PRODUTO.Where(p => p.DS_TIPO_PRODUTO == item.DS_TIPO).OrderByDescending(o => o.NR_VERSAO).FirstOrDefault();
                    if (tipo != null && tipo.RL_AGRUPAMENTO_TIPOS_PRODUTO.Count() > 0)
                    {
                        var idAgrupamento = tipo.RL_AGRUPAMENTO_TIPOS_PRODUTO.FirstOrDefault().ID_AGRUPAMENTO;
                        var itemRel = _connection.SQLServerContext.TB_REL_ACOMPANHAMENTO_PRODUCAO.Where(p => p.DT_RESUMO == item.DT_MOVIMENTO && p.ID_AGRUPAMENTO == idAgrupamento).FirstOrDefault();
                        if (itemRel == null)
                        {
                            itemRel = new TB_REL_ACOMPANHAMENTO_PRODUCAO();
                            itemRel.DT_RESUMO = item.DT_MOVIMENTO;
                            itemRel.ID_AGRUPAMENTO = tipo.RL_AGRUPAMENTO_TIPOS_PRODUTO.FirstOrDefault().ID_AGRUPAMENTO;
                            itemRel.QT_PRODUCAO = item.QT_PRODUCAO;
                            _connection.SQLServerContext.TB_REL_ACOMPANHAMENTO_PRODUCAO.Add(itemRel);
                        }
                        itemRel.QT_PRODUCAO = item.QT_PRODUCAO;
                        _connection.SQLServerContext.SaveChanges();
                    }
                }
            }

            LogHelper.Log("Resumo da produção por modelo gerado com sucesso");

        }
    }
}
