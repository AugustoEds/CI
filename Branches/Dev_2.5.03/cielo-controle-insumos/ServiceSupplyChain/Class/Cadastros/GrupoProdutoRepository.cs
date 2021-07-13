using ServiceSupplyChain.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.Class
{
    public class GrupoProdutoRepository
    {
        private static ConnectionHelper _connection;

        public GrupoProdutoRepository(ConnectionHelper connection)
        {
            _connection = connection;
        }

        public void Sync()
        {
            LogHelper.Log("Sincronizando cadastro de Grupos de Produtos");
            var prodsFirebird = _connection.FirebirdContext.ESGRUPRO;   
            LogHelper.Log(String.Format("{0} registros a serem atualizados", prodsFirebird.Count()));
            var prodsSQLServer = _connection.SQLServerContext.TB_GRUPO_PRODUTO;

            // Primeiro iremos resolver problemas de produtos com grupos não cadastrados
            _connection.FirebirdContext.Database.ExecuteSqlCommand("insert into ESGRUPRO (CODIGO, DESCRICAO) "+
                                                           "select a.grupo, 'NÃO CADASTRADO' DS_GRUPO_PRODUTO " +
                                                           "from( " +
                                                           "select escadpro.grupo " +
                                                           "  from escadpro " +
                                                           " where escadpro.grupo not in (select codigo from esgrupro) " +
                                                           "group by escadpro.grupo) a");

            foreach (var prodF in prodsFirebird)
            {
                LogHelper.Process();
                var prodS = prodsSQLServer.Where(p=>p.CD_GRUPO_PRODUTO == prodF.CODIGO).FirstOrDefault();
                if (prodS == null)
                {
                    prodS = new TB_GRUPO_PRODUTO();
                    prodS.CD_GRUPO_PRODUTO = prodF.CODIGO;
                    _connection.SQLServerContext.TB_GRUPO_PRODUTO.Add(prodS);
                }
                prodS.DS_GRUPO_PRODUTO = prodF.DESCRICAO;
            }

            _connection.SQLServerContext.SaveChanges();
            LogHelper.Log("Atualização dos grupos de produtos concluído");

        }
    }
}
