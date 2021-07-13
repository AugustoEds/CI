using ServiceSupplyChain.DTO;
using ServiceSupplyChain.SQLServer;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.Class
{
    public class ResumoEntradasHelper
    {
        private static ConnectionHelper _connection;

        public ResumoEntradasHelper(ConnectionHelper connection)
        {
            _connection = connection;
        }

        private string GetSqlFirebird()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select p.id_pre_rec id_lote");
            sb.AppendLine("      , i.id_item_yep id_produto");
            sb.AppendLine("      , p.dt_recebimento dt_resumo");
            sb.AppendLine("      , i.qt_volume * i.qt_unidade_volume qt_produto");
            sb.AppendLine("  from tb_pre_rec p ");
            sb.AppendLine("left join escaddep origem on origem.codigo = p.id_deposito_origem");
            sb.AppendLine("left join tb_prec_rec_item i on i.id_pre_rec = p.id_pre_rec");
            sb.AppendLine("left join escadpro prod on prod.codigo = i.id_item_yep");
            sb.AppendLine("left join tb_config_gerais c on c.id_config = c.id_config");
            sb.AppendLine(" where p.id_fluxo = 1 ");

            sb.AppendLine("   and extract(day from p.DT_CONFIRMACAO_RECEBIMENTO) = {0}");
            sb.AppendLine("   and extract(month from p.DT_CONFIRMACAO_RECEBIMENTO) = {1}");
            sb.AppendLine("   and extract(year from p.DT_CONFIRMACAO_RECEBIMENTO) = {2}");

            sb.AppendLine("   and p.id_tipo_pre_rec in (select id_tipo_pre_rec from tb_tipo_pre_rec where classificacao_dep = 'Fabricante')");
            sb.AppendLine("   and p.st_pre_rec in (12, 19, 20) ");
            sb.AppendLine("   and p.id_deposito_origem is not null ");
            sb.AppendLine("   and prod.grupo = c.id_grupo_npa");

            return sb.ToString();
        }

        public void ReadData()
        {
            LogHelper.Log("Gerando dados relatório resumo de entradas");
            var dataBase = _connection.DataBase.AddDays(1);
            LogHelper.Log(GetSqlFirebird());
            var listaProd = _connection.SQLServerContext.TB_PRODUTO.Select(s => new { s.ID_PRODUTO, s.CD_PRODUTO }).ToList();


            for (var dia = 1; dia <= _connection.CountDays; dia++)
            {
                dataBase = dataBase.AddDays(-1);
                
                if (dataBase.ToShortDateString() == "02/03/2016")
                {
                    LogHelper.Process();
                }

                LogHelper.Log(dataBase.ToString());
                // para garantir que não está sendo duplicado, iremos excluir os registros do dia, se houver
                _connection.SQLServerContext.Database.ExecuteSqlCommand(string.Format("delete from TB_REL_ENTRADA_INSUMOS where DAY(DT_RESUMO) = {0} AND MONTH(DT_RESUMO) = {1} AND YEAR(DT_RESUMO) = {2}", dataBase.Day, dataBase.Month, dataBase.Year));
                
                List<ResumoEntradasDTO> itens = _connection.FirebirdContext.Database.SqlQuery<ResumoEntradasDTO>(string.Format(GetSqlFirebird(), dataBase.Day, dataBase.Month, dataBase.Year)).ToList();
                foreach (var item in itens)
                {
                    LogHelper.Process();
                    var prod = listaProd.Where(p => p.CD_PRODUTO == item.ID_PRODUTO).FirstOrDefault();

                    var itemRel = new TB_REL_ENTRADA_INSUMOS();
                    itemRel.ID_LOTE = item.ID_LOTE;
                    itemRel.DT_RESUMO = dataBase; // item.DT_RESUMO;
                    itemRel.ID_PRODUTO = prod.ID_PRODUTO;
                    itemRel.QT_PRODUTO = item.QT_PRODUTO;
                    _connection.SQLServerContext.TB_REL_ENTRADA_INSUMOS.Add(itemRel);
                }
            }

            try
            {
                _connection.SQLServerContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                string retorno = "ERRO: ";
                foreach (var eve in e.EntityValidationErrors)
                {
                    retorno = retorno + string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        retorno = retorno + string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                LogHelper.Log(retorno);
            }


            LogHelper.Log("Relatório resumo de entradas com sucesso");

        }
    }
}
