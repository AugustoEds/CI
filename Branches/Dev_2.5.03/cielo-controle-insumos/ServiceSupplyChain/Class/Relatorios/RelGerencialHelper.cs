using ServiceSupplyChain.DTO;
using ServiceSupplyChain.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.Class
{
    public class RelGerencialHelper
    {
        private static ConnectionHelper _connection;

        public RelGerencialHelper(ConnectionHelper connection)
        {
            _connection = connection;
        }

        private string GetSqlFirebird()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select prod.codigo cd_produto");
            sb.AppendLine("      , sum(case when prod.grupo = config.id_grupo_rpa               then sd.qt_liberado else 0 end ) QT_ESTOQUE_RPA");
            sb.AppendLine("      , sum(case when prod.grupo = config.id_grupo_triagem           then sd.qt_liberado else 0 end ) QT_ESTOQUE_TRIAGEM");
            sb.AppendLine("      , sum(case when prod.grupo = config.id_grupo_tec_descontinuada then sd.qt_liberado else 0 end ) QT_ESTOQUE_TEC_DESCONTINUADA");
            sb.AppendLine("      , sum(case when prod.grupo = config.id_grupo_npa               then sd.qt_liberado else 0 end ) QT_ESTOQUE_NPA");
            sb.AppendLine("      , sum(case when prod.grupo = config.id_grupo_rpt_reparo        then sd.qt_liberado else 0 end ) QT_ESTOQUE_RPT_REPARO");
            sb.AppendLine("      , sum(case when prod.grupo = config.id_grupo_sucata            then sd.qt_liberado else 0 end ) QT_ESTOQUE_SUCATA");
            sb.AppendLine("      , sum(case when prod.grupo = config.id_grupo_rpt_garantia      then sd.qt_liberado else 0 end ) QT_ESTOQUE_RPT_GARANTIA");
            sb.AppendLine("  from tb_saldo_estoque sd");
            sb.AppendLine("left join escadpro prod on prod.codigo = sd.id_produto");
            sb.AppendLine("left join essubpro sub on sub.codigo = prod.sub_grupo");
            sb.AppendLine("left join tb_config_gerais config on 1=1");
            sb.AppendLine("where sd.id_deposito in (" + GetListCD() + ")");
            sb.AppendLine("  and sd.ID_LOCAL_ESTOQUE = 2");
            sb.AppendLine("group by prod.codigo");

            return sb.ToString();
        }

        private string GetListaEstoqueLocal()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" select  DEPOSITO as id_deposito");
            sb.AppendLine("         ,codigo_item as ID_PRODUTO ");
            sb.AppendLine("        ,sum(QT_CAIXA) as qt_produto ");
            sb.AppendLine(" from ( ");
            sb.AppendLine(" select ");
            sb.AppendLine(" pr.id_deposito_destino         as DEPOSITO ");
            sb.AppendLine(" ,lt.id_lote                    as LOTE ");
            sb.AppendLine(" ,cx.id_caixa                   as ID_CAIXA ");
            sb.AppendLine(" ,cx.nr_etiqueta                as SERIAL_ETIQUETA ");
            sb.AppendLine(" ,ll.ds_locacao                 as LOCACAO_CX ");
            sb.AppendLine(" ,sc.id_status_caixa            as id_status_caixa ");
            sb.AppendLine(" ,sc.ds_status_caixa            as DS_STATUS_CAIXA ");
            sb.AppendLine(" ,pi.id_item_yep                as CODIGO_ITEM ");
            sb.AppendLine(" ,sg.codigo                     as SUBPRODUTO ");
            sb.AppendLine(" ,sg.descricao                  as SUBPROD_DESCRICAO ");
            sb.AppendLine(" ,cd.descricao                  as ITEM ");
            sb.AppendLine(" ,cx.qt_itens                   as QT_CAIXA ");
            sb.AppendLine(" ,pr.dt_recebimento             as DT_CRIACAO_VOLUME ");
            sb.AppendLine(" ,mv.dt_hr_movimento            as DATA_ALMOXARIFADO ");
            sb.AppendLine(" from      tb_caixa             cx ");
            sb.AppendLine(" left join tb_prec_rec_item     pi on cx.id_pre_rec_item     = pi.id_pre_rec_item ");
            sb.AppendLine(" left join tb_pre_rec           pr on pi.id_pre_rec          = pr.id_pre_rec ");
            sb.AppendLine(" left join tb_situacao_pre_rec  sp on pr.st_pre_rec          = sp.st_pre_rec ");
            sb.AppendLine(" left join tb_lote              lt on pr.id_pre_rec          = lt.id_pre_rec ");
            sb.AppendLine(" left join tb_status_caixa      sc on cx.id_status_caixa     = sc.id_status_caixa ");
            sb.AppendLine(" left join tb_locacao_local     ll on cx.id_locacao_local    = ll.id_locacao_local ");
            sb.AppendLine(" left join escadpro             cd on pi.id_item_yep         = cd.codigo ");
            sb.AppendLine(" left join essubpro             sg on cd.sub_grupo         = sg.codigo ");
            sb.AppendLine(" left join tb_fluxo             fl on pr.id_fluxo            = fl.id_fluxo ");
            sb.AppendLine(" left join tb_fase              fa on cx.id_fase             = fa.id_fase ");
            sb.AppendLine(" left join tb_romaneio_reserva  rr on cx.id_caixa            = rr.id_caixa ");
            sb.AppendLine(" left join tb_romaneio_item     ri on rr.id_romaneio_item    = ri.id_romaneio_item ");
            sb.AppendLine(" left join tb_romaneio          ro on ri.id_romaneio         = ro.id_romaneio ");
            sb.AppendLine(" left join tb_status_romaneio   sr on ro.id_status_romaneio  = sr.id_status_romaneio ");
            sb.AppendLine(" left join tb_motivo_recusa     mr on mr.id_motivo_recusa    = pr.id_motivo_recusa ");
            sb.AppendLine(" left join tb_mov_caixa         mv on mv.id_caixa = cx.id_caixa ");
            sb.AppendLine(" where ");
            sb.AppendLine(" pr.id_deposito_destino in (5,630) ");
            sb.AppendLine(" and cx.id_status_caixa in (14,15) ");
            sb.AppendLine(" and mv.rdb$db_key = (SELECT top 1 MV1.rdb$db_key ");
            sb.AppendLine("                         FROM tb_mov_caixa mv1 ");
            sb.AppendLine("                         WHERE mv1.id_caixa = mv.id_caixa ");
            sb.AppendLine("                         ORDER BY mv1.dt_hr_movimento DESC)) a ");
            sb.AppendLine(" group by DEPOSITO, codigo_item ");
            return sb.ToString();
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

        public void ReadData()
        {
            LogHelper.Log("Gerando dados relatório gerencial");
            LogHelper.Log(GetSqlFirebird());

            var dataBase = _connection.DataBase.AddDays(1);
            var listComp = _connection.SQLServerContext.TB_PRODUTO_COMPATIVEL.Select(p => new { p.ID_PRODUTO, p.ID_GRUPO_PRODUTO, p.ID_PRODUTO_COMPATIVEL }).ToList();
            var config = _connection.SQLServerContext.TB_CONFIG_GERAIS.FirstOrDefault();
            var listProd = _connection.SQLServerContext.TB_PRODUTO.Select(p => new { p.ID_PRODUTO, p.CD_PRODUTO }).ToList();
            var progProd = _connection.SQLServerContext.TB_PROGRAMACAO_PRODUCAO.Where(p => !p.FL_ABERTO && p.DT_VIGENCIA_INICIAL <= dataBase && p.DT_VIGENCIA_FINAL >= dataBase).FirstOrDefault();
            
            #region Saldo de estoque por local
            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine("SELECT S.id_deposito, S.ID_PRODUTO, SUM(S.qt_liberado) qt_produto");
            //sb.AppendLine("  FROM TB_SALDO_ESTOQUE S WHERE S.ID_LOCAL_ESTOQUE = 2");
            //sb.AppendLine("GROUP BY S.id_deposito, S.ID_PRODUTO");
            #endregion
            var listEstLocal = _connection.FirebirdContext.Database.SqlQuery<ResSaldoEstoque>(GetListaEstoqueLocal()).ToList();

            var listAtivo = _connection.SQLServerContext.TB_SITUACAO_PRODUTO.Select(p => new { p.ID_PRODUTO, p.FL_ATIVO }).ToList();

            for (var i = 1; i <= 1; i++)
            {
                dataBase = dataBase.AddDays(-1);
                LogHelper.Log(dataBase.ToString());

                // para garantir que não está sendo duplicado, iremos excluir os registros do dia, se houver
                _connection.SQLServerContext.Database.ExecuteSqlCommand(string.Format("delete from TB_REL_GERENCIAL where day(DT_RELATORIO) = {0} and month(DT_RELATORIO) = {1} and year(DT_RELATORIO) = {2}", dataBase.Day, dataBase.Month, dataBase.Year));

                List<RelGerencialDTO> itens = _connection.FirebirdContext.Database.SqlQuery<RelGerencialDTO>(GetSqlFirebird()).ToList();
                foreach (var item in itens)
                {
                    LogHelper.Process();
                    var prod = listProd.Where(p => p.CD_PRODUTO == item.CD_PRODUTO).FirstOrDefault();
                    var prodNPA = listComp.Where(p => p.ID_PRODUTO == prod.ID_PRODUTO && p.ID_GRUPO_PRODUTO == config.ID_GRUPO_NPA).FirstOrDefault();
                    var prodRPA = listComp.Where(p => p.ID_PRODUTO == prod.ID_PRODUTO && p.ID_GRUPO_PRODUTO == config.ID_GRUPO_RPA).FirstOrDefault();
                    var prodRPTTriagem = listComp.Where(p => p.ID_PRODUTO == prod.ID_PRODUTO && p.ID_GRUPO_PRODUTO == config.ID_GRUPO_RPT_TRIAGEM).FirstOrDefault();
                    var prodRPTReparo = listComp.Where(p => p.ID_PRODUTO == prod.ID_PRODUTO && p.ID_GRUPO_PRODUTO == config.ID_GRUPO_RPT_REPARO).FirstOrDefault();
                    var prodTecDescontinuada = listComp.Where(p => p.ID_PRODUTO == prod.ID_PRODUTO && p.ID_GRUPO_PRODUTO == config.ID_GRUPO_TEC_DESCONTINUADA).FirstOrDefault();
                    var prodRPTGarantia = listComp.Where(p => p.ID_PRODUTO == prod.ID_PRODUTO && p.ID_GRUPO_PRODUTO == config.ID_GRUPO_RPT_GARANTIA).FirstOrDefault();
                    var prodSucata = listComp.Where(p => p.ID_PRODUTO == prod.ID_PRODUTO && p.ID_GRUPO_PRODUTO == config.ID_GRUPO_SUCATA).FirstOrDefault();

                    if(prodRPA !=  null)
                    {
                        var ativo = listAtivo.Where(p => p.ID_PRODUTO == prodRPA.ID_PRODUTO_COMPATIVEL).FirstOrDefault();
                        if (ativo != null && !ativo.FL_ATIVO)
                        {
                            continue;
                        }
                    }

                    if(prodNPA !=null)
                    {
                        var itemRel = _connection.SQLServerContext.TB_REL_GERENCIAL.Where(p => p.DT_RELATORIO == dataBase && p.ID_PRODUTO_NPA == prodNPA.ID_PRODUTO_COMPATIVEL).FirstOrDefault();
                        if (itemRel == null)
                        {
                            itemRel = new TB_REL_GERENCIAL();

                            itemRel.DT_RELATORIO = dataBase;
                            itemRel.ID_PRODUTO_NPA = prodNPA.ID_PRODUTO_COMPATIVEL;
                            itemRel.ID_PRODUTO_RPA = prodRPA == null? 2 : prodRPA.ID_PRODUTO_COMPATIVEL;
                            itemRel.ID_PRODUTO_RPT_TRIAGEM = prodRPTTriagem == null ? 2 : prodRPTTriagem.ID_PRODUTO_COMPATIVEL;
                            itemRel.ID_PRODUTO_RPT_REPARO = prodRPTReparo == null ? 2 : prodRPTReparo.ID_PRODUTO_COMPATIVEL;
                            itemRel.ID_PRODUTO_TEC_DESCONTINUADA = prodTecDescontinuada == null ? 2 : prodTecDescontinuada.ID_PRODUTO_COMPATIVEL;
                            itemRel.ID_PRODUTO_RPT_GARANTIA = prodRPTGarantia == null ? 2 : prodRPTGarantia.ID_PRODUTO_COMPATIVEL;
                            itemRel.ID_PRODUTO_SUCATA = prodSucata == null ? 2 : prodSucata.ID_PRODUTO_COMPATIVEL;
                            var cdProdNPA = prodNPA == null ? "2" : listProd.Where(p => p.ID_PRODUTO == itemRel.ID_PRODUTO_NPA).FirstOrDefault().CD_PRODUTO;
                            var cdProdRPA = prodRPA == null ? "2" :  listProd.Where(p => p.ID_PRODUTO == itemRel.ID_PRODUTO_RPA).FirstOrDefault().CD_PRODUTO;

                            itemRel.QT_SALDO_PRODUTIVO = listEstLocal.Where(p => p.ID_PRODUTO == cdProdNPA || p.ID_PRODUTO == cdProdRPA).Sum(P => (int)P.QT_PRODUTO);
                            _connection.SQLServerContext.TB_REL_GERENCIAL.Add(itemRel);
                        }


                        itemRel.QT_ESTOQUE_RPA = itemRel.QT_ESTOQUE_RPA + item.QT_ESTOQUE_RPA;
                        itemRel.QT_ESTOQUE_TRIAGEM = itemRel.QT_ESTOQUE_TRIAGEM + item.QT_ESTOQUE_TRIAGEM;
                        itemRel.QT_ESTOQUE_TEC_DESCONTINUADA = itemRel.QT_ESTOQUE_TEC_DESCONTINUADA + item.QT_ESTOQUE_TEC_DESCONTINUADA;
                        itemRel.QT_ESTOQUE_NPA = itemRel.QT_ESTOQUE_NPA + item.QT_ESTOQUE_NPA;
                        itemRel.QT_ESTOQUE_RPT_REPARO = itemRel.QT_ESTOQUE_RPT_REPARO + item.QT_ESTOQUE_RPT_REPARO;
                        itemRel.QT_ESTOQUE_SUCATA = itemRel.QT_ESTOQUE_SUCATA + item.QT_ESTOQUE_SUCATA;
                        itemRel.QT_ESTOQUE_RPT_GARANTIA = itemRel.QT_ESTOQUE_RPT_GARANTIA + item.QT_ESTOQUE_RPT_GARANTIA;


                        //itemRel.QT_SALDO_PRODUTIVO = item.QT_ESTOQUE_RPA + item.QT_ESTOQUE_NPA;

                        itemRel.QT_CONSUMO_DIARIO = 0;
                        itemRel.QT_DIAS_CONSUMO = 0;
                        if (progProd != null)
                        {
                            if (progProd.TB_PREVISAO_CONSUMO_PRODUCAO.Where(p => p.ID_INSUMO == itemRel.ID_PRODUTO_NPA || p.ID_INSUMO == itemRel.ID_PRODUTO_RPA).FirstOrDefault() != null)
                            {
                                var qtConsumo = progProd.TB_PREVISAO_CONSUMO_PRODUCAO.Where(p => p.ID_INSUMO == itemRel.ID_PRODUTO_NPA || p.ID_INSUMO == itemRel.ID_PRODUTO_RPA).FirstOrDefault().QT_PREVISAO_DIA;
                                itemRel.QT_CONSUMO_DIARIO = qtConsumo;
                                if (itemRel.QT_CONSUMO_DIARIO > 0)
                                {
                                    itemRel.QT_DIAS_CONSUMO = (int)Math.Truncate((decimal)(itemRel.QT_SALDO_PRODUTIVO / itemRel.QT_CONSUMO_DIARIO));
                                }
                                //if (configRel != null)
                                //{
                                //    var configSAP = configRel.TB_CONFIG_REL_GERENCIAL_ITEM.Where(p => p.CD_SAP == item.CD_SAP).FirstOrDefault();
                                //    if (configSAP != null)
                                //    {
                                //        itemRel.QT_CONSUMO_DIARIO = configSAP.QT_CONSUMO_DIARIO;
                                //        itemRel.QT_DIAS_CONSUMO = itemRel.QT_SALDO_PRODUTIVO / itemRel.QT_CONSUMO_DIARIO;
                                //    }
                                //}
                            }
                        }

                        _connection.SQLServerContext.SaveChanges();
                    }
                    
                }

                
            }

            LogHelper.Log("Relatório Gerencial gerado com sucesso");

        }
    }
}
