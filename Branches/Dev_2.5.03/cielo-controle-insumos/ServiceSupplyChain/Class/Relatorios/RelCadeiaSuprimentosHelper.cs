using ServiceSupplyChain.DTO;
using ServiceSupplyChain.SQLServer;
using ServiceSupplyChain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.Class
{
    public class RelCadeiaSuprimentosHelper
    {
        private static ConnectionHelper _connection;

        public RelCadeiaSuprimentosHelper(ConnectionHelper connection)
        {
            _connection = connection;
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

        private string GetSqlFirebird()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select prod.codigo cd_produto");
            sb.AppendLine("      ,sd.id_deposito");
            //sb.AppendLine("      ,sum(case when prod.grupo = config.id_grupo_sucata            then sd.qt_liberado - sd.qt_reservado else 0 end) qt_produto_sucata");
            sb.AppendLine("      ,sum(case when prod.grupo = '82'                              then sd.qt_liberado else 0 end) qt_produto_sucata");
            sb.AppendLine("      ,sum(case when prod.grupo = config.id_grupo_rpa               then sd.qt_liberado else 0 end) qt_produto_rpa");
            sb.AppendLine("      ,sum(case when prod.grupo = config.id_grupo_npa               then sd.qt_liberado else 0 end) qt_produto_npa");
            sb.AppendLine("      ,sum(case when prod.grupo = config.id_grupo_triagem           then sd.qt_liberado else 0 end) qt_produto_rpt_triagem");
            sb.AppendLine("      ,sum(case when prod.grupo = config.id_grupo_rpt_reparo        then sd.qt_liberado else 0 end) qt_produto_rpt_reparo");
            sb.AppendLine("      ,sum(case when prod.grupo = config.id_grupo_tec_descontinuada then sd.qt_liberado else 0 end) qt_produto_tec_descontinuada");
            //sb.AppendLine("      ,sum(case when prod.grupo = config.id_grupo_rpt_garantia      then sd.qt_liberado - sd.qt_reservado else 0 end) qt_produto_rpt_garantia");
            sb.AppendLine("      ,sum(case when prod.grupo = '86'                              then sd.qt_liberado else 0 end) qt_produto_rpt_garantia");
            sb.AppendLine("  from tb_saldo_estoque sd");
            sb.AppendLine("left join escadpro prod on prod.codigo = sd.id_produto");
            sb.AppendLine("left join tb_config_gerais config on 1 = 1");
            sb.AppendLine(" where sd.id_deposito in (" + GetListCD() + ")");
            sb.AppendLine("   and sd.id_local_estoque = 2");
            sb.AppendLine("group by prod.codigo");
            sb.AppendLine("        ,sd.id_deposito");

            return sb.ToString();
        }

        public void ReadData()
        {
            LogHelper.Log("Gerando dados relatório cadeia de suprimentos");
            LogHelper.Log(GetSqlFirebird());
            var listGrupos = _connection.SQLServerContext.TB_GRUPO_PRODUTO.Select(s => new { s.ID_GRUPO_PRODUTO, s.CD_GRUPO_PRODUTO }).ToList();
            var listComp = _connection.SQLServerContext.TB_PRODUTO_COMPATIVEL.Select(s => new { s.ID_PRODUTO, s.ID_GRUPO_PRODUTO, s.ID_PRODUTO_COMPATIVEL }).ToList();
            var listProd = _connection.SQLServerContext.TB_PRODUTO.Select(s => new { s.ID_PRODUTO, s.CD_PRODUTO, s.ID_GRUPO_PRODUTO }).ToList();
            var listCD = _connection.SQLServerContext.TB_DEPOSITO_CD.Select(s => new { s.ID_DEPOSITO, s.ID_CD }).ToList();
            var config = _connection.SQLServerContext.TB_CONFIG_GERAIS.FirstOrDefault();

            var dataBase = _connection.DataBase.AddDays(1);

            for (var i = 1; i <= 1; i++)
            {

                dataBase = dataBase.AddDays(-1);

                // para garantir que não está sendo duplicado, iremos excluir os registros do dia, se houver
                _connection.SQLServerContext.Database.ExecuteSqlCommand(string.Format("delete from TB_REL_CADEIA_SUPRIMENTOS WHERE DAY(DT_RESUMO) = {0} AND MONTH(DT_RESUMO) = {1} AND YEAR(DT_RESUMO) = {2}", dataBase.Day, dataBase.Month, dataBase.Year));

                List<RelCadeiaSuprimentosModelView> itens = _connection.FirebirdContext.Database.SqlQuery<RelCadeiaSuprimentosModelView>(GetSqlFirebird()).ToList();

                foreach (var item in itens)
                {
                    LogHelper.Process();
                    try
                    {
                        var prod = listProd.Where(p => p.CD_PRODUTO == item.cd_produto).FirstOrDefault();
                        var idSucata = listComp.Where(p => p.ID_PRODUTO == prod.ID_PRODUTO && p.ID_GRUPO_PRODUTO == config.ID_GRUPO_SUCATA).FirstOrDefault().ID_PRODUTO_COMPATIVEL;
                        var idRPA = listComp.Where(p => p.ID_PRODUTO == prod.ID_PRODUTO && p.ID_GRUPO_PRODUTO == config.ID_GRUPO_RPA).FirstOrDefault().ID_PRODUTO_COMPATIVEL;
                        var idNPA = listComp.Where(p => p.ID_PRODUTO == prod.ID_PRODUTO && p.ID_GRUPO_PRODUTO == config.ID_GRUPO_NPA).FirstOrDefault().ID_PRODUTO_COMPATIVEL;
                        var idRPTReparo = listComp.Where(p => p.ID_PRODUTO == prod.ID_PRODUTO && p.ID_GRUPO_PRODUTO == config.ID_GRUPO_RPT_REPARO).FirstOrDefault().ID_PRODUTO_COMPATIVEL;
                        var idRPTTriagem = listComp.Where(p => p.ID_PRODUTO == prod.ID_PRODUTO && p.ID_GRUPO_PRODUTO == config.ID_GRUPO_RPT_TRIAGEM).FirstOrDefault().ID_PRODUTO_COMPATIVEL;
                        var idTecDescont = listComp.Where(p => p.ID_PRODUTO == prod.ID_PRODUTO && p.ID_GRUPO_PRODUTO == config.ID_GRUPO_TEC_DESCONTINUADA).FirstOrDefault().ID_PRODUTO_COMPATIVEL;
                        var idRPTGarantia = listComp.Where(p => p.ID_PRODUTO == prod.ID_PRODUTO && p.ID_GRUPO_PRODUTO == config.ID_GRUPO_RPT_GARANTIA).FirstOrDefault().ID_PRODUTO_COMPATIVEL;
                        var idDeposito = listCD.Where(p => p.ID_DEPOSITO == item.id_deposito).FirstOrDefault().ID_CD;

                        TB_REL_CADEIA_SUPRIMENTOS itemRel = _connection.SQLServerContext.TB_REL_CADEIA_SUPRIMENTOS.Where(p => p.ID_DEPOSITO_CD == idDeposito && p.ID_PRODUTO_NPA == idNPA && p.DT_RESUMO == _connection.DataBase).FirstOrDefault();
                        if (itemRel == null)
                        {
                            itemRel = new TB_REL_CADEIA_SUPRIMENTOS();
                            itemRel.DT_RESUMO = dataBase;
                            itemRel.ID_DEPOSITO_CD = idDeposito;
                            itemRel.ID_PRODUTO_NPA = idNPA;
                            itemRel.ID_PRODUTO_RPA = idRPA;
                            itemRel.ID_PRODUTO_RPT_GARANTIA = idRPTGarantia;
                            itemRel.ID_PRODUTO_RPT_REPARO = idRPTReparo;
                            itemRel.ID_PRODUTO_RPT_TRIAGEM = idRPTTriagem;
                            itemRel.ID_PRODUTO_SUCATA = idSucata;
                            itemRel.ID_PRODUTO_TEC_DESCONTINUADA = idTecDescont;
                            _connection.SQLServerContext.TB_REL_CADEIA_SUPRIMENTOS.Add(itemRel);
                        }
                        itemRel.QT_PRODUTO_NPA = itemRel.QT_PRODUTO_NPA + item.qt_produto_npa;
                        itemRel.QT_PRODUTO_RPA = itemRel.QT_PRODUTO_RPA + item.qt_produto_rpa;
                        itemRel.QT_PRODUTO_RPT_GARANTIA = itemRel.QT_PRODUTO_RPT_GARANTIA + item.qt_produto_rpt_garantia;
                        itemRel.QT_PRODUTO_RPT_REPARO = itemRel.QT_PRODUTO_RPT_REPARO + item.qt_produto_rpt_reparo;
                        itemRel.QT_PRODUTO_RPT_TRIAGEM = itemRel.QT_PRODUTO_RPT_TRIAGEM + item.qt_produto_rpt_triagem;
                        itemRel.QT_PRODUTO_SUCATA = itemRel.QT_PRODUTO_SUCATA + item.qt_produto_sucata;
                        itemRel.QT_PRODUTO_TEC_DESCONTINUADA = itemRel.QT_PRODUTO_TEC_DESCONTINUADA + item.qt_produto_tec_descontinuada;
                        _connection.SQLServerContext.SaveChanges();
                    }
                    catch { }
                }

            }

            LogHelper.Log("Relatório de cadeia de suprimentos gerado com sucesso");

        }
    }
}
