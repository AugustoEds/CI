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
    public class ResumoSaidasHelper
    {
        private static ConnectionHelper _connection;

        public ResumoSaidasHelper(ConnectionHelper connection)
        {
            _connection = connection;
        }

        private string GetSqlFirebird()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select r.dt_romaneio dt_movimento");
            sb.AppendLine("      , recitem.id_item_yep id_produto");
            sb.AppendLine("      , c.qt_itens qt_produto");
            sb.AppendLine("      , r.id_romaneio id_lote");
            sb.AppendLine("      , r.id_deposito_origem id_deposito");
            sb.AppendLine("  from tb_romaneio r");
            sb.AppendLine("inner join tb_romaneio_item ri on ri.id_romaneio = r.id_romaneio");
            sb.AppendLine("inner join tb_romaneio_reserva rr on rr.id_romaneio_item = ri.id_romaneio_item");
            sb.AppendLine("inner join tb_caixa c on c.id_caixa = rr.id_caixa");
            sb.AppendLine("inner join tb_prec_rec_item recitem on recitem.id_pre_rec_item = c.id_pre_rec_item");
            sb.AppendLine(" where r.id_status_romaneio = 17");
            sb.AppendLine("   and r.tp_movimentacao = 1");
            sb.AppendLine("   and r.dt_romaneio = '{0}.{1}.{2}'");

            return sb.ToString();
        }

        public void ReadData()
        {
            LogHelper.Log("Gerando dados relatório resumo de saídas");
            var dataBase = _connection.DataBase.AddDays(1);
            LogHelper.Log(GetSqlFirebird());


            var ListProd = _connection.SQLServerContext.TB_PRODUTO.Select(s => new { s.ID_PRODUTO, s.CD_PRODUTO }).ToList();
            var listCD = _connection.SQLServerContext.TB_DEPOSITO_CD.Select(s => new { s.ID_CD, s.ID_DEPOSITO }).ToList();

            for (var dia = 1; dia <= _connection.CountDays; dia++)
            {
                dataBase = dataBase.AddDays(-1);
                LogHelper.Log(dataBase.ToString());

                // para garantir que não está sendo duplicado, iremos excluir os registros do dia, se houver
                _connection.SQLServerContext.Database.ExecuteSqlCommand(string.Format("delete from TB_REL_SAIDA_INSUMOS where day(dt_resumo) = {0} and month(dt_resumo) = {1} and year(dt_resumo) = {2}", dataBase.Day, dataBase.Month, dataBase.Year));


                List<ResumoMovtoSaidaDTO> itens = _connection.FirebirdContext.Database.SqlQuery<ResumoMovtoSaidaDTO>(string.Format(GetSqlFirebird(), dataBase.Day, dataBase.Month, dataBase.Year)).ToList();
                foreach (var item in itens)
                {
                    LogHelper.Process();

                    var dep = listCD.Where(p => p.ID_DEPOSITO == item.ID_DEPOSITO).FirstOrDefault();

                    if (dep != null)
                    {
                        var itemRel = new TB_REL_SAIDA_INSUMOS();
                        itemRel.ID_LOTE = item.ID_LOTE;
                        itemRel.DT_RESUMO = item.DT_MOVIMENTO;
                        itemRel.ID_PRODUTO = ListProd.Where(p => p.CD_PRODUTO == item.ID_PRODUTO).FirstOrDefault().ID_PRODUTO;
                        itemRel.QT_PRODUTO = item.QT_PRODUTO;
                        itemRel.ID_DEPOSITO_CD = dep.ID_CD;
                        _connection.SQLServerContext.TB_REL_SAIDA_INSUMOS.Add(itemRel);
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
            }


            LogHelper.Log("Relatório resumo de saídas com sucesso");

        }
    }
}
