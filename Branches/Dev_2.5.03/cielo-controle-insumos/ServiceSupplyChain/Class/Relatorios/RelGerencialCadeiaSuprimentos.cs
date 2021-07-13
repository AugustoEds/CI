using ServiceSupplyChain.DTO;
using ServiceSupplyChain.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.Class
{


    public static class LocalizacaoEstoque
    {
        public static int Recebimento { get { return 1; } }
        public static int Almoxarifado { get { return 2; } }
        public static int Producao { get { return 3; } }
        public static int Triagem { get { return 4; } }
    }

    public class ResSaldoEstoque
    {
        public int? ID_DEPOSITO { get; set; }
        public int? ID_LOCAL_ESTOQUE { get; set; }
        public string ID_PRODUTO { get; set; }
        public int? QT_PRODUTO { get; set; }
    }

    public class PrevRec
    {
        public string NR_PEDIDO_COMPRA { get; set; }
        public DateTime DT_PREVISAO_RECEBIMENTO { get; set; }
        public int QT_PROGRAMACAO { get; set; }
        public int QT_TOT_PENDENTE { get; set; }
    }

    public class RecPrestador
    {
        public string ID_PRODUTO { get; set; }
        public int QT_PRODUTO { get; set; }
    }

    public class RelGerencialCadeiaSuprimentos
    {
        private static ConnectionHelper _connection;

        public RelGerencialCadeiaSuprimentos(ConnectionHelper connection)
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


            return sb.ToString();
        }

        public void ReadData()
        {
            LogHelper.Log("Gerando dados relatório gerencial da cadeia de suprimentos");
            LogHelper.Log(GetSqlFirebird());

            var dataBase = _connection.DataBase.AddDays(1);
            #region Saldo de estoque por local
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT S.ID_PRODUTO, SUM(S.qt_liberado) qt_produto");
            sb.AppendLine("  FROM TB_SALDO_ESTOQUE S where s.id_local_estoque = 1");
            sb.AppendLine(" and s.id_deposito in (" + GetListCD() + ")");
            sb.AppendLine("GROUP BY S.ID_PRODUTO");
            #endregion
            var listEstLocal = _connection.FirebirdContext.Database.SqlQuery<ResSaldoEstoque>(sb.ToString()).ToList();

            #region Saldo de estoque por local
            sb.Clear();
            sb.AppendLine("SELECT S.id_deposito, S.ID_PRODUTO, SUM(S.qt_liberado) qt_produto");
            sb.AppendLine("  FROM TB_SALDO_ESTOQUE S WHERE S.ID_LOCAL_ESTOQUE = 3");
            sb.AppendLine(" and s.id_deposito in (" + GetListCD() + ")");
            sb.AppendLine("GROUP BY S.id_deposito, S.ID_PRODUTO");
            #endregion
            var listEstProdutivo = _connection.FirebirdContext.Database.SqlQuery<ResSaldoEstoque>(sb.ToString()).ToList();

            var listCD = _connection.SQLServerContext.TB_DEPOSITO_CD.Select(p => new { p.ID_CD, p.ID_DEPOSITO }).ToList();
            var listProd = _connection.SQLServerContext.TB_PRODUTO.Select(s => new { s.ID_PRODUTO, s.ID_GRUPO_PRODUTO, s.CD_PRODUTO, s.CD_SAP }).ToList();
            var listComp = _connection.SQLServerContext.TB_PRODUTO_COMPATIVEL.Select(s => new { s.ID_PRODUTO, s.ID_GRUPO_PRODUTO, s.ID_PRODUTO_COMPATIVEL }).ToList();
            var config = _connection.SQLServerContext.TB_CONFIG_GERAIS.FirstOrDefault();
            var listEstoque = _connection.SQLServerContext.TB_SALDO_ESTOQUE.Where(p => p.DT_SALDO_ESTOQUE == _connection.DataBase).ToList();
            var listGroupEstoque = listEstoque.GroupBy(p => p.ID_PRODUTO).Select(s => new { ID_PRODUTO = s.Key }).ToList();
            List<int> listLab = _connection.FirebirdContext.Database.SqlQuery<int>("select codigo from ESCADDEP Where CLASSIFICACAO_DEP = 'Laboratório externo'").ToList();
            List<int> listExt = _connection.FirebirdContext.Database.SqlQuery<int>("select codigo from ESCADDEP Where CLASSIFICACAO_DEP = 'Prestador de serviço'").ToList();
            var listAtivo = _connection.SQLServerContext.TB_SITUACAO_PRODUTO.Select(p => new { p.ID_PRODUTO, p.FL_ATIVO }).ToList();

            #region Saldo de estoque por laboratório
            sb.Clear();
            sb.AppendLine("select s.id_produto, sum(s.qt_liberado) qt_produto");
            sb.AppendLine("  from tb_saldo_estoque s");
            sb.AppendLine("inner join escaddep dep on dep.codigo = s.id_deposito");
            sb.AppendLine("where dep.classificacao_dep = 'Laboratório externo'");
            sb.AppendLine("group by s.id_produto");
            var listEstLab = _connection.FirebirdContext.Database.SqlQuery<ResSaldoEstoque>(sb.ToString()).ToList();
            #endregion

            #region Saldo de estoque por Prestador de serviço
            sb.Clear();
            sb.AppendLine("select s.id_produto, sum(s.qt_liberado) qt_produto");
            sb.AppendLine("  from tb_saldo_estoque s");
            sb.AppendLine("inner join escaddep dep on dep.codigo = s.id_deposito");
            sb.AppendLine("where dep.classificacao_dep = 'Prestador de serviço'");
            sb.AppendLine("group by s.id_produto");
            var listEstExt = _connection.FirebirdContext.Database.SqlQuery<ResSaldoEstoque>(sb.ToString()).ToList();
            #endregion

            #region Saldo de estoque local triagem
            sb.Clear();
            sb.AppendLine("select s.id_produto, sum(s.qt_liberado) qt_produto");
            sb.AppendLine("  from tb_saldo_estoque s");
            sb.AppendLine(" where s.id_local_estoque = 4");
            sb.AppendLine("group by s.id_produto");
            var listEstTriagem = _connection.FirebirdContext.Database.SqlQuery<ResSaldoEstoque>(sb.ToString()).ToList();
            #endregion



            #region Saldo de estoque por almoxarifado
            sb.Clear();
            sb.AppendLine("SELECT S.id_deposito, S.ID_PRODUTO, SUM(S.qt_liberado) qt_produto");
            sb.AppendLine("  FROM TB_SALDO_ESTOQUE S WHERE S.ID_LOCAL_ESTOQUE = 2");
            sb.AppendLine(" and s.id_deposito in (" + GetListCD() + ")");
            sb.AppendLine("GROUP BY S.id_deposito, S.ID_PRODUTO");
            #endregion
            var listEstAlmox = _connection.FirebirdContext.Database.SqlQuery<ResSaldoEstoque>(sb.ToString()).ToList();

            for (var dia = 1; dia <= _connection.CountDays; dia++)
            {
                dataBase = dataBase.AddDays(-1);

                // para garantir que não está sendo duplicado, iremos excluir os registros do dia, se houver
                _connection.SQLServerContext.Database.ExecuteSqlCommand(string.Format("delete from TB_REL_GERAL_CADEIA_SUPRIMENTOS where day(DT_RESUMO) = {0} and month(DT_RESUMO) = {1} and year(DT_RESUMO) = {2}", dataBase.Day, dataBase.Month, dataBase.Year));


                var progProd = _connection.SQLServerContext.TB_PROGRAMACAO_PRODUCAO.Where(p => !p.FL_ABERTO && p.DT_VIGENCIA_INICIAL <= dataBase && p.DT_VIGENCIA_FINAL >= dataBase).FirstOrDefault();
                List<TB_REL_ACOMPANHAMENTO_PRODUCAO> listProducao = new List<TB_REL_ACOMPANHAMENTO_PRODUCAO>();
                if (progProd != null)
                {
                    listProducao = _connection.SQLServerContext.TB_REL_ACOMPANHAMENTO_PRODUCAO.Where(p => p.DT_RESUMO >= progProd.DT_VIGENCIA_INICIAL && p.DT_RESUMO <= progProd.DT_VIGENCIA_FINAL).ToList();
                }

                if (dataBase.Day == 13 && dataBase.Month == 4)
                {
                    LogHelper.Process();
                }

                #region Recebimento de prestadores no periodo
                sb.Clear();
                sb.AppendLine("select i.id_item_yep id_produto, sum(i.qt_volume * i.qt_unidade_volume) QT_PRODUTO");
                sb.AppendLine("  from tb_prec_rec_item i");
                sb.AppendLine("left join tb_pre_rec r on r.id_pre_rec = i.id_pre_rec");
                sb.AppendLine("left join escaddep dep on dep.codigo = r.id_deposito_origem");
                sb.AppendLine(" where r.id_fluxo = 1");
                sb.AppendLine("   and dep.classificacao_dep = 'Prestador de serviço'");
                sb.AppendLine("   and r.dt_confirmacao_recebimento is not null");
                sb.AppendLine("group by i.id_item_yep");
                var listRecPrestador = _connection.FirebirdContext.Database.SqlQuery<RecPrestador>(sb.ToString()).ToList();
                #endregion

                #region Faz a composição da base de produtos para o Relatório
                LogHelper.Log("Compondo produtos");
                var listRel = new List<TB_REL_GERAL_CADEIA_SUPRIMENTOS>();
                //int n = 1;
                foreach (var item in listGroupEstoque)
                {
                    LogHelper.Process();
                    LogHelper.Log("Produto");
                    Console.WriteLine(item.ID_PRODUTO);
                    var prodNPA = listComp.Where(p => p.ID_PRODUTO == item.ID_PRODUTO && p.ID_GRUPO_PRODUTO == config.ID_GRUPO_NPA).FirstOrDefault();
                    var prodRPA = listComp.Where(p => p.ID_PRODUTO == item.ID_PRODUTO && p.ID_GRUPO_PRODUTO == config.ID_GRUPO_RPA).FirstOrDefault();
                    var prodRPTTriagem = listComp.Where(p => p.ID_PRODUTO == item.ID_PRODUTO && p.ID_GRUPO_PRODUTO == config.ID_GRUPO_RPT_TRIAGEM).FirstOrDefault();
                    var prodRPTReparo = listComp.Where(p => p.ID_PRODUTO == item.ID_PRODUTO && p.ID_GRUPO_PRODUTO == config.ID_GRUPO_RPT_REPARO).FirstOrDefault();
                    var prodTecDescont = listComp.Where(p => p.ID_PRODUTO == item.ID_PRODUTO && p.ID_GRUPO_PRODUTO == config.ID_GRUPO_TEC_DESCONTINUADA).FirstOrDefault();
                    var prodRPTGarantia = listComp.Where(p => p.ID_PRODUTO == item.ID_PRODUTO && p.ID_GRUPO_PRODUTO == config.ID_GRUPO_RPT_GARANTIA).FirstOrDefault();
                    var prodSucata = listComp.Where(p => p.ID_PRODUTO == item.ID_PRODUTO && p.ID_GRUPO_PRODUTO == config.ID_GRUPO_SUCATA).FirstOrDefault();

                    if(prodRPTTriagem != null)
                    {
                        var tr = listProd.Where(p => p.ID_PRODUTO == prodRPTTriagem.ID_PRODUTO).FirstOrDefault();
                        if (tr.CD_PRODUTO == "81.42.000006")
                        {
                            LogHelper.Process();
                        }
                    }
                   

                    if (prodRPA != null)
                    {
                        var ativo = listAtivo.Where(p => p.ID_PRODUTO == prodRPA.ID_PRODUTO_COMPATIVEL).FirstOrDefault();
                        if (ativo != null && !ativo.FL_ATIVO)
                        {
                            continue;
                        }
                    }

                    int NPA = prodNPA != null ? prodNPA.ID_PRODUTO_COMPATIVEL : 2; //precisa criar produto para não mostrar nada aqui.
                    int RPA = prodRPA != null ? prodRPA.ID_PRODUTO_COMPATIVEL : 2;
                    int RPT_TRIAGEM = prodRPTTriagem != null ? prodRPTTriagem.ID_PRODUTO_COMPATIVEL : 2;
                    int RPT_REPARO = prodRPTReparo != null ? prodRPTReparo.ID_PRODUTO_COMPATIVEL : 2;
                    int TEC_DESCONTINUADA = prodTecDescont != null ? prodTecDescont.ID_PRODUTO_COMPATIVEL : 2;
                    int RPT_GARANTIA = prodRPTGarantia != null ? prodRPTGarantia.ID_PRODUTO_COMPATIVEL : 2;
                    int PRODUTO_SUCATA = prodSucata != null ? prodSucata.ID_PRODUTO_COMPATIVEL : 2;


                    TB_REL_GERAL_CADEIA_SUPRIMENTOS rel = listRel.Where(p => p.ID_PRODUTO_NPA == NPA &&
                                                                             p.ID_PRODUTO_RPA == RPA &&
                                                                             p.ID_PRODUTO_RPT_TRIAGEM == RPT_TRIAGEM &&
                                                                             p.ID_PRODUTO_RPT_REPARO == RPT_REPARO &&
                                                                             p.ID_PRODUTO_TEC_DESCONTINUADA == TEC_DESCONTINUADA &&
                                                                             p.ID_PRODUTO_RPT_GARANTIA == RPT_GARANTIA &&
                                                                             p.ID_PRODUTO_SUCATA == PRODUTO_SUCATA).FirstOrDefault();
                    if (rel == null)
                    {
                        rel = new TB_REL_GERAL_CADEIA_SUPRIMENTOS();
                        rel.DT_RESUMO = dataBase;
                        rel.ID_PRODUTO_NPA = NPA;
                        rel.ID_PRODUTO_RPA = RPA;
                        rel.ID_PRODUTO_RPT_TRIAGEM = RPT_TRIAGEM;
                        rel.ID_PRODUTO_RPT_REPARO = RPT_REPARO;
                        rel.ID_PRODUTO_TEC_DESCONTINUADA = TEC_DESCONTINUADA;
                        rel.ID_PRODUTO_RPT_GARANTIA = RPT_GARANTIA;
                        rel.ID_PRODUTO_SUCATA = PRODUTO_SUCATA;
                        listRel.Add(rel);
                    }

                }

                var sqlInsert = new StringBuilder();
                foreach (var item in listRel)
                {
                    Console.WriteLine("Insert item: " + item.DT_RESUMO.Year, item.DT_RESUMO.Month,
                         item.DT_RESUMO.Day, item.ID_PRODUTO_NPA, item.ID_PRODUTO_RPA,                                                                                                    item.ID_PRODUTO_RPT_TRIAGEM,
                                                                                                    item.ID_PRODUTO_RPT_REPARO,
                                                                                                    item.ID_PRODUTO_TEC_DESCONTINUADA,
                                                                                                    item.ID_PRODUTO_RPT_GARANTIA,
                                                                                                    item.ID_PRODUTO_SUCATA);
                    //_connection.SQLServerContext.TB_REL_GERAL_CADEIA_SUPRIMENTOS.Add(item);
                    sqlInsert.AppendLine(string.Format("INSERT INTO TB_REL_GERAL_CADEIA_SUPRIMENTOS (DT_RESUMO, ID_PRODUTO_NPA,ID_PRODUTO_RPA,ID_PRODUTO_RPT_TRIAGEM," +
                                                                                                    "ID_PRODUTO_RPT_REPARO, ID_PRODUTO_TEC_DESCONTINUADA,ID_PRODUTO_RPT_GARANTIA," +
                                                                                                    "ID_PRODUTO_SUCATA) VALUES ('{0}-{1}-{2}',{3},{4},{5},{6},{7},{8},{9})",
                                                                                                    item.DT_RESUMO.Year,
                                                                                                    item.DT_RESUMO.Month,
                                                                                                    item.DT_RESUMO.Day,
                                                                                                    item.ID_PRODUTO_NPA,
                                                                                                    item.ID_PRODUTO_RPA,
                                                                                                    item.ID_PRODUTO_RPT_TRIAGEM,
                                                                                                    item.ID_PRODUTO_RPT_REPARO,
                                                                                                    item.ID_PRODUTO_TEC_DESCONTINUADA,
                                                                                                    item.ID_PRODUTO_RPT_GARANTIA,
                                                                                                    item.ID_PRODUTO_SUCATA));
                }
                _connection.SQLServerContext.Database.ExecuteSqlCommand(sqlInsert.ToString());
                _connection.SQLServerContext.SaveChanges();
                #endregion

                #region Analisaremos o total de insumos da cadeia
                var model = _connection.SQLServerContext.TB_REL_GERAL_CADEIA_SUPRIMENTOS.Where(p => p.DT_RESUMO == dataBase).ToList();
                foreach (var item in model)
                {
                    LogHelper.Log("Produto");
                    LogHelper.Process();

                    var prodNPA = listProd.Where(p => p.ID_PRODUTO == item.ID_PRODUTO_NPA).FirstOrDefault();
                    var prodRPA = listProd.Where(p => p.ID_PRODUTO == item.ID_PRODUTO_RPA).FirstOrDefault();
                    var prodRPTTriagem = listProd.Where(p => p.ID_PRODUTO == item.ID_PRODUTO_RPT_TRIAGEM).FirstOrDefault();
                    var prodRPTReparo = listProd.Where(p => p.ID_PRODUTO == item.ID_PRODUTO_RPT_REPARO).FirstOrDefault();
                    var prodTecDescont = listProd.Where(p => p.ID_PRODUTO == item.ID_PRODUTO_TEC_DESCONTINUADA).FirstOrDefault();
                    var prodSucata = listProd.Where(p => p.ID_PRODUTO == item.ID_PRODUTO_SUCATA).FirstOrDefault();
                    var prodGarantia = listProd.Where(p => p.ID_PRODUTO == item.ID_PRODUTO_RPT_GARANTIA).FirstOrDefault();

                    if (prodRPTTriagem.CD_PRODUTO == "81.42.000006")
                    {
                        LogHelper.Process();
                    }


                    #region Estoque
                    item.QT_TOTAL_NPA = 0;
                    item.QT_TOTAL_RPA = 0;
                    item.QT_TOTAL_RPT_TRIAGEM = 0;
                    item.QT_TOTAL_RPT_GARANTIA = 0;
                    LogHelper.Log("CD");
                    foreach (var dep in listCD)
                    {

                        LogHelper.Process();
                        //var itemNPA = listEstoque.Where(p => p.ID_DEPOSITO == dep.ID_DEPOSITO && p.ID_PRODUTO == item.ID_PRODUTO_NPA);
                        //var itemRPA = listEstoque.Where(p => p.ID_DEPOSITO == dep.ID_DEPOSITO && p.ID_PRODUTO == item.ID_PRODUTO_RPA);
                        //var itemRPTTriagem = listEstoque.Where(p => p.ID_DEPOSITO == dep.ID_DEPOSITO && p.ID_PRODUTO == item.ID_PRODUTO_RPT_TRIAGEM);
                        //var itemRPTGarantia = listEstoque.Where(p => p.ID_DEPOSITO == dep.ID_DEPOSITO && p.ID_PRODUTO == item.ID_PRODUTO_RPT_GARANTIA);

                        //if (itemNPA.Count() > 0)
                        //{
                        //    item.QT_TOTAL_NPA = item.QT_TOTAL_NPA + (int)itemNPA.Sum(p => p.QT_PRODUTO);
                        //}
                        //if (itemRPA.Count() > 0)
                        //{
                        //    item.QT_TOTAL_RPA = item.QT_TOTAL_RPA + (int)itemRPA.Sum(p => p.QT_PRODUTO);
                        //}
                        //if (itemRPTTriagem.Count() > 0)
                        //{
                        //    item.QT_TOTAL_RPT_TRIAGEM = item.QT_TOTAL_RPT_TRIAGEM + (int)itemRPTTriagem.Sum(p => p.QT_PRODUTO);
                        //}
                        //if (itemRPTGarantia.Count() > 0)
                        //{
                        //    item.QT_TOTAL_RPT_GARANTIA = item.QT_TOTAL_RPT_GARANTIA + (int)itemRPTGarantia.Sum(p => p.QT_PRODUTO);
                        //}

                        var cdProd = listProd.Where(p => p.ID_PRODUTO == item.ID_PRODUTO_NPA).FirstOrDefault().CD_PRODUTO;
                        var qtProd = listEstAlmox.Where(p => p.ID_DEPOSITO == dep.ID_DEPOSITO && p.ID_PRODUTO == cdProd).Sum(p => p.QT_PRODUTO);
                        item.QT_TOTAL_NPA = item.QT_TOTAL_NPA + (int)qtProd;

                        cdProd = listProd.Where(p => p.ID_PRODUTO == item.ID_PRODUTO_RPA).FirstOrDefault().CD_PRODUTO;
                        qtProd = listEstAlmox.Where(p => p.ID_DEPOSITO == dep.ID_DEPOSITO && p.ID_PRODUTO == cdProd).Sum(p => p.QT_PRODUTO);
                        item.QT_TOTAL_RPA = item.QT_TOTAL_RPA + (int)qtProd;

                        cdProd = listProd.Where(p => p.ID_PRODUTO == item.ID_PRODUTO_RPT_TRIAGEM).FirstOrDefault().CD_PRODUTO;
                        qtProd = listEstAlmox.Where(p => p.ID_DEPOSITO == dep.ID_DEPOSITO && p.ID_PRODUTO == cdProd).Sum(p => p.QT_PRODUTO);
                        item.QT_TOTAL_RPT_TRIAGEM = item.QT_TOTAL_RPT_TRIAGEM + (int)qtProd;

                        cdProd = listProd.Where(p => p.ID_PRODUTO == item.ID_PRODUTO_RPT_GARANTIA).FirstOrDefault().CD_PRODUTO;
                        qtProd = listEstAlmox.Where(p => p.ID_DEPOSITO == dep.ID_DEPOSITO && p.ID_PRODUTO == cdProd).Sum(p => p.QT_PRODUTO);
                        item.QT_TOTAL_RPT_GARANTIA = item.QT_TOTAL_RPT_GARANTIA + (int)qtProd;

                    }

                    // Deposito externo
                    LogHelper.Log("Depósito Externo");
                    item.QT_SALDO_FORNECEDOR = (int)listEstExt.Where(p => p.ID_PRODUTO == prodNPA.CD_PRODUTO ||
                                                                    p.ID_PRODUTO == prodRPA.CD_PRODUTO ||
                                                                    p.ID_PRODUTO == prodRPTTriagem.CD_PRODUTO ||
                                                                    p.ID_PRODUTO == prodRPTReparo.CD_PRODUTO ||
                                                                    p.ID_PRODUTO == prodTecDescont.CD_PRODUTO ||
                                                                    p.ID_PRODUTO == prodGarantia.CD_PRODUTO ||
                                                                    p.ID_PRODUTO == prodSucata.CD_PRODUTO).Sum(p => p.QT_PRODUTO);
                    //foreach (var dep in listExt)
                    //{

                    //    LogHelper.Process();
                    //    var itemDep = listEstoque.Where(p => p.ID_DEPOSITO == dep &&
                    //                                      (p.ID_PRODUTO == item.ID_PRODUTO_NPA ||
                    //                                       p.ID_PRODUTO == item.ID_PRODUTO_RPA ||
                    //                                       p.ID_PRODUTO == item.ID_PRODUTO_RPT_TRIAGEM ||
                    //                                       p.ID_PRODUTO == item.ID_PRODUTO_RPT_REPARO ||
                    //                                       p.ID_PRODUTO == item.ID_PRODUTO_TEC_DESCONTINUADA ||
                    //                                       p.ID_PRODUTO == item.ID_PRODUTO_RPT_GARANTIA ||
                    //                                       p.ID_PRODUTO == item.ID_PRODUTO_SUCATA)).ToList();
                    //    if (itemDep != null)
                    //    {
                    //        item.QT_SALDO_FORNECEDOR = item.QT_SALDO_FORNECEDOR + (int)itemDep.Sum(p => p.QT_PRODUTO);
                    //    }
                    //}

                    // Deposito Laboratórios
                    LogHelper.Log("Laboratório");
                    item.QT_SALDO_LABORATORIO = (int)listEstLab.Where(p => p.ID_PRODUTO == prodNPA.CD_PRODUTO ||
                                                p.ID_PRODUTO == prodRPA.CD_PRODUTO ||
                                                p.ID_PRODUTO == prodRPTTriagem.CD_PRODUTO ||
                                                p.ID_PRODUTO == prodRPTReparo.CD_PRODUTO ||
                                                p.ID_PRODUTO == prodTecDescont.CD_PRODUTO ||
                                                p.ID_PRODUTO == prodGarantia.CD_PRODUTO ||
                                                p.ID_PRODUTO == prodSucata.CD_PRODUTO).Sum(p => p.QT_PRODUTO);
                    //foreach (var dep in listLab)
                    //{

                    //    LogHelper.Process();
                    //    var itemDep = listEstoque.Where(p => p.ID_DEPOSITO == dep &&
                    //                                      (p.ID_PRODUTO == item.ID_PRODUTO_NPA ||
                    //                                       p.ID_PRODUTO == item.ID_PRODUTO_RPA ||
                    //                                       p.ID_PRODUTO == item.ID_PRODUTO_RPT_TRIAGEM ||
                    //                                       p.ID_PRODUTO == item.ID_PRODUTO_RPT_REPARO ||
                    //                                       p.ID_PRODUTO == item.ID_PRODUTO_TEC_DESCONTINUADA ||
                    //                                       p.ID_PRODUTO == item.ID_PRODUTO_RPT_GARANTIA ||
                    //                                       p.ID_PRODUTO == item.ID_PRODUTO_SUCATA)).ToList();
                    //    if (itemDep != null)
                    //    {
                    //        item.QT_SALDO_LABORATORIO = item.QT_SALDO_LABORATORIO + (int)itemDep.Sum(p => p.QT_PRODUTO);
                    //    }
                    //}
                    #endregion

                    #region Cobertura do estoque
                    var qtConsumo = 0;
                    var qtDiasProd = 0;
                    //item.QT_SALDO_WIP = item.QT_TOTAL_NPA + item.QT_TOTAL_RPA;
                    item.QT_SALDO_WIP = listEstProdutivo.Where(p => p.ID_PRODUTO == prodNPA.CD_PRODUTO || p.ID_PRODUTO == prodRPA.CD_PRODUTO).Sum(P => P.QT_PRODUTO);
                    if (progProd != null)
                    {
                        if (progProd.TB_PREVISAO_CONSUMO_PRODUCAO.Where(p => p.ID_INSUMO == item.ID_PRODUTO_NPA || p.ID_INSUMO == item.ID_PRODUTO_RPA).FirstOrDefault() != null)
                        {
                            qtConsumo = progProd.TB_PREVISAO_CONSUMO_PRODUCAO.Where(p => p.ID_INSUMO == item.ID_PRODUTO_NPA || p.ID_INSUMO == item.ID_PRODUTO_RPA).FirstOrDefault().QT_PREVISAO_DIA;
                            qtDiasProd = progProd.QT_DIAS_PREVISAO_PRODUCAO;
                            item.QT_CONSUMO_DIA = qtConsumo;
                            if (item.QT_CONSUMO_DIA > 0)
                            {
                                item.QT_DIAS_COBERTURA = (int)Math.Truncate((decimal)((item.QT_SALDO_WIP + item.QT_TOTAL_NPA + item.QT_TOTAL_RPA) / item.QT_CONSUMO_DIA));
                            }
                        }
                    }
                    #endregion

                    #region Produtos em recebimento
                    var recNPA = listEstLocal.Where(p => p.ID_PRODUTO == prodNPA.CD_PRODUTO).ToList();
                    var recRPA = listEstLocal.Where(p => p.ID_PRODUTO == prodRPA.CD_PRODUTO).ToList();
                    var recRPTTriagem = listEstLocal.Where(p => p.ID_PRODUTO == prodRPTTriagem.CD_PRODUTO).ToList();
                    var recRPTReparo = listEstLocal.Where(p => p.ID_PRODUTO == prodRPTReparo.CD_PRODUTO).ToList();
                    var recTecDescon = listEstLocal.Where(p => p.ID_PRODUTO == prodTecDescont.CD_PRODUTO).ToList();
                    var recSucata = listEstLocal.Where(p => p.ID_PRODUTO == prodSucata.CD_PRODUTO).ToList();

                    if (recNPA != null)
                    {
                        item.QT_EMREC_NPA = (int)recNPA.Sum(p => p.QT_PRODUTO);
                    }
                    if (recRPA != null)
                    {
                        item.QT_EMREC_RPA = (int)recRPA.Sum(p => p.QT_PRODUTO);
                    }
                    if (recRPTTriagem != null)
                    {
                        item.QT_EMREC_RPT_TRIAGEM = (int)recRPTTriagem.Sum(p => p.QT_PRODUTO);
                    }
                    if (recRPTReparo != null)
                    {
                        item.QT_EMREC_RPT_REPARO = (int)recRPTReparo.Sum(p => p.QT_PRODUTO);
                    }
                    if (recTecDescon != null)
                    {
                        item.QT_EMREC_DESCONTINUADO = (int)recTecDescon.Sum(p => p.QT_PRODUTO);
                    }
                    if (recSucata != null)
                    {
                        item.QT_EMREC_SUCATA = (int)recSucata.Sum(p => p.QT_PRODUTO);
                    }
                    #endregion

                    #region Previsao de Entrega
                    LogHelper.Log("Buscando programação para produto " + prodNPA.CD_PRODUTO);
                    PrevRec prevRec = GetQtProximaEntrega(prodNPA.CD_SAP);
                    item.QT_PROXIMA_ENTREGA = prevRec.QT_PROGRAMACAO;
                    item.DT_PROXIMA_ENTREGA = prevRec.DT_PREVISAO_RECEBIMENTO;
                    item.QT_GERAL_ENTREGA = prevRec.QT_TOT_PENDENTE;
                    #endregion


                    #region Necessidades
                    if (qtConsumo > 0)
                    {
                        // Pegaremos o total já consumido baseado no relatório de produção
                        int qtProducao = 0;
                        foreach (var producao in listProducao)
                        {
                            foreach (var estrutura in producao.TB_AGRUPAMENTO_TIPO_PRODUTO.RL_AGRUPAMENTO_TIPOS_PRODUTO.FirstOrDefault().TB_TIPO_PRODUTO.TB_ESTRUTURA_TIPO_PRODUTO)
                            {
                                if (estrutura.ID_INSUMO == prodNPA.ID_PRODUTO ||
                                    estrutura.ID_INSUMO == prodRPA.ID_PRODUTO ||
                                    estrutura.ID_INSUMO == prodRPTReparo.ID_PRODUTO ||
                                    estrutura.ID_INSUMO == prodRPTTriagem.ID_PRODUTO ||
                                    estrutura.ID_INSUMO == prodGarantia.ID_PRODUTO ||
                                    estrutura.ID_INSUMO == prodSucata.ID_PRODUTO ||
                                    estrutura.ID_INSUMO == prodTecDescont.ID_PRODUTO)
                                {
                                    qtProducao = qtProducao + (producao.QT_PRODUCAO * estrutura.QT_INSUMO);
                                }
                            }
                        }

                        var sitProd = _connection.SQLServerContext.TB_SITUACAO_PRODUTO.Where(p => p.ID_PRODUTO == prodRPA.ID_PRODUTO).FirstOrDefault();

                        if (prodRPA.CD_PRODUTO == "80.44.000002")
                        {
                            LogHelper.Process();
                        }

                        // Pega o saldo de estoque no local produçao
                        sb.Clear();
                        sb.AppendLine("SELECT SUM(S.qt_liberado) qt_produto");
                        sb.AppendLine("  FROM TB_SALDO_ESTOQUE S WHERE S.ID_LOCAL_ESTOQUE = 3");
                        sb.AppendLine(" and s.id_deposito in (" + GetListCD() + ")");
                        sb.AppendLine(" and s.id_produto in ('{0}','{1}')");

                        var saldoProd = _connection.FirebirdContext.Database.SqlQuery<ResSaldoEstoque>(string.Format(sb.ToString(), prodNPA.CD_PRODUTO, prodRPA.CD_PRODUTO)).FirstOrDefault();
                        int qtProd = 0;
                        if (saldoProd != null && saldoProd.QT_PRODUTO > 0)
                        {
                            qtProd = (int)saldoProd.QT_PRODUTO;
                        }

                        item.QT_NECESSIDADE_SUPRIMENTOS = (qtConsumo * qtDiasProd) - qtProducao - item.QT_TOTAL_NPA - item.QT_TOTAL_RPA - qtProd;
                        item.QT_NECESSIDADE_TRIAGEM = 0;
                        item.QT_NECESSIDADE_REPARO = 0;
                        item.QT_NECESSIDADE_COMPRA = 0;

                        if (item.QT_NECESSIDADE_SUPRIMENTOS > 0)
                        {


                            if (sitProd != null)
                            {
                                float pcTriagem = 0;
                                if (!sitProd.FL_CALCULO_AUTOMATICO_TRIAGEM)
                                {
                                    if (sitProd.PC_RECUPERACAO_TRIAGEM != null)
                                    {
                                        pcTriagem = (float)sitProd.PC_RECUPERACAO_TRIAGEM;
                                    }
                                }
                                else
                                {
                                    // Buscar base hístórica
                                    var qtRpa = (float)listRecPrestador.Where(p => p.ID_PRODUTO == prodRPA.CD_PRODUTO).Sum(s => s.QT_PRODUTO);
                                    var qtReparo = (float)listRecPrestador.Where(p => p.ID_PRODUTO == prodRPTReparo.CD_PRODUTO).Sum(s => s.QT_PRODUTO);
                                    if (qtRpa + qtReparo > 0)
                                    {
                                        pcTriagem = ((qtRpa / (qtRpa + qtReparo)) * 100);
                                    }

                                }
                                //double triagem = (int)item.QT_NECESSIDADE_SUPRIMENTOS * (pcTriagem / 100);
                                double triagem = (int)item.QT_TOTAL_RPT_TRIAGEM * (pcTriagem / 100);

                                // Fornecedor
                                item.QT_NECESSIDADE_REPARO = (int)Math.Truncate(triagem);

                                // YEP
                                // pegar todo RPA que está no local triagem
                                // PC triagem interna  0;
                                item.QT_NECESSIDADE_TRIAGEM = (int)listEstTriagem.Where(p => p.ID_PRODUTO == prodRPA.CD_PRODUTO).Sum(s => s.QT_PRODUTO);

                                item.QT_NECESSIDADE_COMPRA = item.QT_NECESSIDADE_SUPRIMENTOS - item.QT_NECESSIDADE_TRIAGEM - item.QT_NECESSIDADE_REPARO;
                                if (item.QT_NECESSIDADE_COMPRA < 0)
                                {
                                    item.QT_NECESSIDADE_COMPRA = 0;
                                }

                            }
                        }
                        else
                        {
                            item.QT_NECESSIDADE_SUPRIMENTOS = 0;
                        }

                    }
                    #endregion

                }
                _connection.SQLServerContext.SaveChanges();
                #endregion
            }


            LogHelper.Log("Relatório Gerencial da cadeia de suprimentos gerado com sucesso");
        }

        private PrevRec GetQtProximaEntrega(string CdSAP)
        {
            var retorno = new PrevRec();

            if (CdSAP == "604166")
            {
                LogHelper.Process();
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select NR_PEDIDO_COMPRA, DT_PREVISAO_RECEBIMENTO, QT_PROGRAMACAO");
            sb.AppendLine("  from TB_PROGRAMACAO_RECEBIMENTO");
            sb.AppendLine(" where cd_sap = '{0}'");
            sb.AppendLine("order by dt_previsao_recebimento");

            LogHelper.Log(sb.ToString());
            var prog = _connection.SQLServerContext.Database.SqlQuery<PrevRec>(string.Format(sb.ToString(), CdSAP)).ToList();


            foreach (var item in prog)
            {
                // Verificaremos se já foi efetuado algum recebimento para esse pedido de compra
                var listRec = _connection.FirebirdContext.TB_PRE_REC.Where(p => p.NR_PEDIDO_COMPRA == item.NR_PEDIDO_COMPRA).ToList();
                int QtRec = 0;
                if (listRec != null)
                {
                    foreach (var rec in listRec)
                    {
                        var prod = rec.TB_PREC_REC_ITEM.Where(p => p.ESCADPRO.CODIGO_SAP == CdSAP).ToList();
                        if (prod != null)
                        {
                            QtRec = QtRec + (int)prod.Sum(p => p.QT_UNIDADE_VOLUME * p.QT_VOLUME);
                        }
                    }
                }

                if (QtRec <= item.QT_PROGRAMACAO)
                {
                    if (item.DT_PREVISAO_RECEBIMENTO >= _connection.DataBase && retorno.NR_PEDIDO_COMPRA == null)
                    {
                        retorno.NR_PEDIDO_COMPRA = item.NR_PEDIDO_COMPRA;
                        retorno.DT_PREVISAO_RECEBIMENTO = item.DT_PREVISAO_RECEBIMENTO;
                        retorno.QT_PROGRAMACAO = item.QT_PROGRAMACAO - QtRec;
                    }
                    retorno.QT_TOT_PENDENTE = retorno.QT_TOT_PENDENTE + (item.QT_PROGRAMACAO - QtRec);
                }

            }


            return retorno;
        }

        private int GetDefaultValueInt(double? value)
        {
            if (value == null)
            {
                return 0;
            }
            else
            {
                return (int)value;
            }
        }

    }
}
