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
    public class ResumoMovtoFornecExternoHelper
    {
        private static ConnectionHelper _connection;

        public ResumoMovtoFornecExternoHelper(ConnectionHelper connection)
        {
            _connection = connection;
        }

        private string GetSqlFirebird()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select r.dt_romaneio dt_movimento");
            sb.AppendLine("      , recitem.id_item_yep id_produto");
            sb.AppendLine("      , 'S' TP_MOVIMENTO");
            sb.AppendLine("      , c.qt_itens qt_produto");
            sb.AppendLine("      , r.id_romaneio id_lote");
            sb.AppendLine("  from tb_romaneio r");
            sb.AppendLine("inner join tb_romaneio_item ri on ri.id_romaneio = r.id_romaneio");
            sb.AppendLine("inner join tb_romaneio_reserva rr on rr.id_romaneio_item = ri.id_romaneio_item");
            sb.AppendLine("inner join tb_caixa c on c.id_caixa = rr.id_caixa");
            sb.AppendLine("inner join tb_prec_rec_item recitem on recitem.id_pre_rec_item = c.id_pre_rec_item");
            sb.AppendLine("inner join escaddep dep on dep.codigo = r.id_deposito_destino");
            sb.AppendLine(" where r.id_status_romaneio = 17");
            sb.AppendLine("   and r.tp_movimentacao = 1");
            sb.AppendLine("   and r.dt_romaneio = '{0}.{1}.{2}'");
            sb.AppendLine("   and dep.classificacao_dep = 'Prestador de serviço'");
            sb.AppendLine("union all");
            sb.AppendLine("select cast(p.dt_recebimento as date)");
            sb.AppendLine("      , i.id_item_yep");
            sb.AppendLine("      , 'E'");
            sb.AppendLine("      , i.qt_volume * i.qt_unidade_volume");
            sb.AppendLine("      , p.id_pre_rec");
            sb.AppendLine("  from tb_pre_rec p ");
            sb.AppendLine("left join escaddep origem on origem.codigo = p.id_deposito_origem");
            sb.AppendLine("inner join tb_prec_rec_item i on i.id_pre_rec = p.id_pre_rec");
            sb.AppendLine("inner join escaddep dep on dep.codigo = p.id_deposito_origem");
            sb.AppendLine(" where p.id_fluxo = 1 ");
            sb.AppendLine("   and p.st_pre_rec in (19, 20)");
            sb.AppendLine("   and p.id_deposito_origem is not null ");
            sb.AppendLine("   and cast(p.dt_recebimento as date) = '{0}.{1}.{2}'");
            sb.AppendLine("   and dep.classificacao_dep = 'Prestador de serviço'");

            return sb.ToString();
        }

        public void ReadData()
        {
            LogHelper.Log("Gerando dados relatório resumo de movimentação fornecedor externo");
            var dataBase = _connection.DataBase.AddDays(1);
            LogHelper.Log(GetSqlFirebird());
            var ListProd = _connection.SQLServerContext.TB_PRODUTO.Select(s => new { s.ID_PRODUTO, s.CD_PRODUTO }).ToList();

            for (var count = 1; count <= _connection.CountDays; count++)
            {
                dataBase = dataBase.AddDays(-1);
                LogHelper.Log(dataBase.ToString());

                // para garantir que não está sendo duplicado, iremos excluir os registros do dia, se houver
                _connection.SQLServerContext.Database.ExecuteSqlCommand(string.Format("delete from TB_REL_MOV_FORNEC_EXTERNO where day(dt_resumo) = {0} and month(dt_resumo) = {1} and year(dt_resumo) = {2}", dataBase.Day, dataBase.Month, dataBase.Year));

                var sitProds = _connection.SQLServerContext.TB_SITUACAO_PRODUTO.Select(s => new
                {
                    s.TB_PRODUTO.CD_PRODUTO,
                    s.ID_PRODUTO,
                    s.FL_ATIVO
                }).ToList();

                List<ResumoMovtoDTO> itens = _connection.FirebirdContext.Database.SqlQuery<ResumoMovtoDTO>(string.Format(GetSqlFirebird(), dataBase.Day, dataBase.Month, dataBase.Year)).ToList();
                foreach (var item in itens)
                {

                    LogHelper.Process();
                    var prod = sitProds.Where(p => p.CD_PRODUTO == item.ID_PRODUTO).FirstOrDefault();
                    if (prod != null && !prod.FL_ATIVO)
                    {
                        continue;
                    }

                    var itemRel = new TB_REL_MOV_FORNEC_EXTERNO();
                    itemRel.ID_LOTE = item.ID_LOTE;
                    itemRel.DT_RESUMO = item.DT_MOVIMENTO;
                    itemRel.TP_MOVIMENTO = item.TP_MOVIMENTO;
                    itemRel.ID_PRODUTO = ListProd.Where(p => p.CD_PRODUTO == item.ID_PRODUTO).FirstOrDefault().ID_PRODUTO;
                    itemRel.QT_PRODUTO = item.QT_PRODUTO;
                    _connection.SQLServerContext.TB_REL_MOV_FORNEC_EXTERNO.Add(itemRel);
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


            LogHelper.Log("Relatório resumo de movimentação fornecedor externo com sucesso");

        }
    }
}
