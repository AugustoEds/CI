using ServiceSupplyChain.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.Class
{
    public class SubGrupoProdutoHelper
    {
        private static ConnectionHelper _connection;

        public SubGrupoProdutoHelper(ConnectionHelper connection)
        {
            _connection = connection;
        }

        public void Sync()
        {
            LogHelper.Log("Sincronizando cadastro de SubGrupos de Produtos");
            var prodsFirebird = _connection.FirebirdContext.ESSUBPRO;
            LogHelper.Log(String.Format("{0} registros a serem atualizados", prodsFirebird.Count()));
            var prodsSQLServer = _connection.SQLServerContext.TB_SUBGRUPO_PRODUTO;

            // Primeiro iremos resolver problemas de produtos com SUBgrupos não cadastrados
            _connection.FirebirdContext.Database.ExecuteSqlCommand("insert into ESSUBPRO (CODIGO, DESCRICAO) " +
                                                           "select a.sub_grupo, 'NÃO CADASTRADO' DS_SUBGRUPO " +
                                                           "from( " +
                                                           "select escadpro.sub_grupo " +
                                                           "  from escadpro " +
                                                           " where escadpro.sub_grupo not in (select codigo from essubpro) " +
                                                           "group by escadpro.sub_grupo) a");


            foreach (var prodF in prodsFirebird)
            {
                LogHelper.Process();
                var prodS = prodsSQLServer.Where(s=> s.CD_SUBGRUPO_PRODUTO ==  prodF.CODIGO).FirstOrDefault();
                if (prodS == null)
                {
                    prodS = new TB_SUBGRUPO_PRODUTO();
                    prodS.CD_SUBGRUPO_PRODUTO = prodF.CODIGO;
                    _connection.SQLServerContext.TB_SUBGRUPO_PRODUTO.Add(prodS);
                }
                prodS.DS_SUBGRUPO_PRODUTO = prodF.DESCRICAO;
            }

            _connection.SQLServerContext.SaveChanges();
            LogHelper.Log("Atualização dos SubGrupos de produtos concluído");

        }
    }
}
