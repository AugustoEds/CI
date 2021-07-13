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
    public class ResumoSaidasProdHelper
    {
        private static ConnectionHelper _connection;

        public ResumoSaidasProdHelper(ConnectionHelper connection)
        {
            _connection = connection;
        }

        private string GetSqlFirebird(DateTime dataReferencia)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select extract(day from p.dt_recebimento)||'/'||extract(month from p.dt_recebimento)||'/'||extract(year from p.dt_recebimento) as dt_recebimento ");
            sb.AppendLine("      , i.id_item_yep id_produto ");
            sb.AppendLine("      , p.id_deposito_destino ID_DEPOSITO ");
            sb.AppendLine("      , sum(i.qt_volume * i.qt_unidade_volume) qt_produto ");
            sb.AppendLine("  from tb_pre_rec p ");
            sb.AppendLine("left ");
            sb.AppendLine("  join tb_prec_rec_item i on i.id_pre_rec = p.id_pre_rec ");
            sb.AppendLine(" where p.id_fluxo = 3 ");
            sb.AppendLine(string.Format("   and p.dt_recebimento >= '{0}.{1}.{2}' ",dataReferencia.Day, dataReferencia.Month, dataReferencia.Year));
            sb.AppendLine("   and p.st_pre_rec in (19, 20) ");
            sb.AppendLine("group by p.dt_recebimento ");
            sb.AppendLine("        , p.id_deposito_destino ");
            sb.AppendLine("        , i.id_item_yep ");

            return sb.ToString();
        }

        public void ReadData()
        {
            LogHelper.Log("Gerando dados relatório resumo de saidas produção");
            var dataBase = _connection.DataBase.AddDays(1);
            LogHelper.Log(GetSqlFirebird(dataBase));
            var listProd = _connection.SQLServerContext.TB_PRODUTO.Select(s => new { s.ID_PRODUTO, s.CD_PRODUTO }).ToList();
            var listDep = _connection.SQLServerContext.TB_DEPOSITO_CD.Select(s => new { s.ID_DEPOSITO, s.ID_CD }).ToList();

            for (var i = 1; i <= _connection.CountDays; i++)
            {
                dataBase = dataBase.AddDays(-1);
                LogHelper.Log(dataBase.ToString());

                // para garantir que não está sendo duplicado, iremos excluir os registros do dia, se houver
                _connection.SQLServerContext.Database.ExecuteSqlCommand(string.Format("delete from TB_REL_SAIDA_PRODUCAO where DAY(DT_RESUMO) = {0} AND MONTH(DT_RESUMO) = {1} AND YEAR(DT_RESUMO) = {2}",dataBase.Day, dataBase.Month, dataBase.Year));


                List<ResumoSaidasProdDTO> itens = _connection.FirebirdContext.Database.SqlQuery<ResumoSaidasProdDTO>(GetSqlFirebird(dataBase)).ToList();
                foreach (var item in itens)
                {
                    LogHelper.Process();

                    var dep = listDep.Where(p => p.ID_DEPOSITO == item.ID_DEPOSITO).FirstOrDefault();

                    if (dep != null)
                    {
                        var prod = listProd.Where(p => p.CD_PRODUTO == item.ID_PRODUTO).FirstOrDefault();

                        var itemRel = new TB_REL_SAIDA_PRODUCAO();
                        itemRel.DT_RESUMO = DateTime.Parse(item.DT_RECEBIMENTO);
                        itemRel.ID_PRODUTO = prod.ID_PRODUTO;
                        itemRel.ID_DEPOSITO_CD = dep.ID_CD;
                        itemRel.QT_PRODUTO = item.QT_PRODUTO;
                        _connection.SQLServerContext.TB_REL_SAIDA_PRODUCAO.Add(itemRel);
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


            LogHelper.Log("Relatório resumo de saidas produção com sucesso");

        }
    }
}
