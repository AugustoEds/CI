using ServiceSupplyChain.Firebird;
using ServiceSupplyChain.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.Class
{
    public class ProdutoHelper
    {
        private static ConnectionHelper _connection;

        public ProdutoHelper(ConnectionHelper connection)
        {
            _connection = connection;
        }

        public void Sync()
        {
            LogHelper.Log("Sincronizando cadastro de Produtos");
            var prodsF = _connection.FirebirdContext.ESCADPRO.Select(s => new { s.CODIGO, s.DESCRICAO, s.GRUPO, s.SUB_GRUPO, s.CODIGO_SAP }).ToList();
            List<string> lstUpdate = new List<string>();

            var grupos = _connection.SQLServerContext.TB_GRUPO_PRODUTO.ToList();
            var subgrupos = _connection.SQLServerContext.TB_SUBGRUPO_PRODUTO.ToList();
            foreach (var prod in prodsF)
            {
                LogHelper.Process();

                var grupo = grupos.Where(p => p.CD_GRUPO_PRODUTO == prod.GRUPO).FirstOrDefault();
                var subgrupo = subgrupos.Where(p => p.CD_SUBGRUPO_PRODUTO == prod.SUB_GRUPO).FirstOrDefault();

                if (grupo != null && subgrupo != null)
                {
                    lstUpdate.Add(String.Format("MERGE TB_PRODUTO AS target " +
                                            "USING(SELECT '{0}', '{1}','{2}','{3}', '{4}' ) AS source (CD_PRODUTO, DS_PRODUTO, ID_GRUPO_PRODUTO, ID_SUBGRUPO_PRODUTO, CD_SAP) " +
                                            "ON(target.CD_PRODUTO = source.CD_PRODUTO) " +
                                            "WHEN MATCHED " +
                                            "    THEN UPDATE SET target.DS_PRODUTO = source.DS_PRODUTO, " +
                                            "                target.ID_GRUPO_PRODUTO = source.ID_GRUPO_PRODUTO, " +
                                            "                target.ID_SUBGRUPO_PRODUTO = source.ID_SUBGRUPO_PRODUTO, " +
                                            "                target.CD_SAP = source.CD_SAP " +
                                            "WHEN NOT MATCHED BY TARGET " +
                                            "    THEN INSERT(CD_PRODUTO, DS_PRODUTO, ID_GRUPO_PRODUTO, ID_SUBGRUPO_PRODUTO, CD_SAP) VALUES(CD_PRODUTO, DS_PRODUTO, ID_GRUPO_PRODUTO, ID_SUBGRUPO_PRODUTO, CD_SAP);",
                                            prod.CODIGO, prod.DESCRICAO, grupo.ID_GRUPO_PRODUTO, subgrupo.ID_SUBGRUPO_PRODUTO, prod.CODIGO_SAP));

                }               

            }

            foreach (var sql in lstUpdate)
            {
                LogHelper.Process();
                _connection.SQLServerContext.Database.ExecuteSqlCommand(sql);
            }

            _connection.SQLServerContext.SaveChanges();
            LogHelper.Log("Atualizando compatibilidades");
            _connection.SQLServerContext.Database.ExecuteSqlCommand("DELETE FROM TB_PRODUTO_COMPATIVEL");

            List<string> lstComp = _connection.FirebirdContext.Database.SqlQuery<string>("select 'insert into TB_PRODUTO_COMPATIVEL VALUES ( (select id_produto from tb_produto where cd_produto = ''' || prod.codigo || '''), (select id_grupo_produto from tb_grupo_produto where cd_grupo_produto = ''' || comp.grupo || '''),(select id_produto from tb_produto where cd_produto = ''' || comp.codigo || '''));' " +
                                                                                         " from escadpro prod " +
                                                                                         "left " +
                                                                                         " join escadpro comp on comp.codigo = prod.sucata " +
                                                                                         "where comp.sucata is not null " +
                                                                                         "union all " +
                                                                                         "select 'insert into TB_PRODUTO_COMPATIVEL VALUES ( (select id_produto from tb_produto where cd_produto = ''' || prod.codigo || '''), (select id_grupo_produto from tb_grupo_produto where cd_grupo_produto = ''' || comp.grupo || '''),(select id_produto from tb_produto where cd_produto = ''' || comp.codigo || '''));' " +
                                                                                         " from escadpro prod " +
                                                                                         "left " +
                                                                                         " join escadpro comp on comp.codigo = prod.npa " +
                                                                                         "where comp.npa is not null " +
                                                                                         "union all " +
                                                                                         "select 'insert into TB_PRODUTO_COMPATIVEL VALUES ( (select id_produto from tb_produto where cd_produto = ''' || prod.codigo || '''), (select id_grupo_produto from tb_grupo_produto where cd_grupo_produto = ''' || comp.grupo || '''),(select id_produto from tb_produto where cd_produto = ''' || comp.codigo || '''));' " +
                                                                                         " from escadpro prod " +
                                                                                         "left " +
                                                                                         " join escadpro comp on comp.codigo = prod.rpa " +
                                                                                         "where comp.rpa is not null " +
                                                                                         "union " +
                                                                                         "select 'insert into TB_PRODUTO_COMPATIVEL VALUES ( (select id_produto from tb_produto where cd_produto = ''' || prod.codigo || '''), (select id_grupo_produto from tb_grupo_produto where cd_grupo_produto = ''' || comp.grupo || '''),(select id_produto from tb_produto where cd_produto = ''' || comp.codigo || '''));' " +
                                                                                         " from escadpro prod " +
                                                                                         "left " +
                                                                                         " join escadpro comp on comp.codigo = prod.RPT_TRIAGEM " +
                                                                                         "where comp.RPT_TRIAGEM is not null " +
                                                                                         "union " +
                                                                                         "select 'insert into TB_PRODUTO_COMPATIVEL VALUES ( (select id_produto from tb_produto where cd_produto = ''' || prod.codigo || '''), (select id_grupo_produto from tb_grupo_produto where cd_grupo_produto = ''' || comp.grupo || '''),(select id_produto from tb_produto where cd_produto = ''' || comp.codigo || '''));' " +
                                                                                         " from escadpro prod " +
                                                                                         "left " +
                                                                                         " join escadpro comp on comp.codigo = prod.rpt_reparo " +
                                                                                         "where comp.rpt_reparo is not null " +
                                                                                         "union " +
                                                                                         "select 'insert into TB_PRODUTO_COMPATIVEL VALUES ( (select id_produto from tb_produto where cd_produto = ''' || prod.codigo || '''), (select id_grupo_produto from tb_grupo_produto where cd_grupo_produto = ''' || comp.grupo || '''),(select id_produto from tb_produto where cd_produto = ''' || comp.codigo || '''));' " +
                                                                                         " from escadpro prod " +
                                                                                         "left " +
                                                                                         " join escadpro comp on comp.codigo = prod.tecn_descontin " +
                                                                                         "where comp.tecn_descontin is not null " +
                                                                                         "union " +
                                                                                         "select 'insert into TB_PRODUTO_COMPATIVEL VALUES ( (select id_produto from tb_produto where cd_produto = ''' || prod.codigo || '''), (select id_grupo_produto from tb_grupo_produto where cd_grupo_produto = ''' || comp.grupo || '''),(select id_produto from tb_produto where cd_produto = ''' || comp.codigo || '''));' " +
                                                                                         " from escadpro prod " +
                                                                                         "left " +
                                                                                         " join escadpro comp on comp.codigo = prod.rpt_garantia " +
                                                                                         "where comp.rpt_garantia is not null ").ToList();

            foreach (var comp in lstComp)
            {
                LogHelper.Process();
                if (comp.Contains("80.44.000015"))
                {
                    LogHelper.Process();
                }
                _connection.SQLServerContext.Database.ExecuteSqlCommand(comp);

            }

            _connection.SQLServerContext.SaveChanges();


            //
            LogHelper.Log("Atualização dos produtos concluído");
        }

    }
}
