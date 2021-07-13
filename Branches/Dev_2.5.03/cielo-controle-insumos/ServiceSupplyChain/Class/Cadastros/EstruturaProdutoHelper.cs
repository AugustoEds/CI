using ServiceSupplyChain.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.Class
{
    public class EstruturaProdutoHelper
    {
        private static ConnectionHelper _connection;

        public EstruturaProdutoHelper(ConnectionHelper connection)
        {
            _connection = connection;
        }

        public void Sync()
        {
            LogHelper.Log("Sincronizando estrutura de Produtos");
            var tiposFirebird = _connection.FirebirdContext.PCESTPRO_H;
            LogHelper.Log(String.Format("{0} registros a serem atualizados", tiposFirebird.Count()));
            var tiposSQLServer = _connection.SQLServerContext.TB_TIPO_PRODUTO;

            foreach (var prodF in tiposFirebird)
            {
                LogHelper.Process();
                var prodS = tiposSQLServer.Where(s=> s.ID_TIPO_PRODUTO ==  prodF.ESTRUT_ID).FirstOrDefault();
                if (prodS == null)
                {
                    var p = _connection.SQLServerContext.TB_PRODUTO.Where(s => s.CD_PRODUTO == prodF.COD_ITEM).FirstOrDefault();
                    prodS = new TB_TIPO_PRODUTO();
                    prodS.ID_TIPO_PRODUTO = prodF.ESTRUT_ID;
                    prodS.DS_TIPO_PRODUTO = prodF.TIPO;
                    prodS.NR_VERSAO = (int)prodF.VERSAO;
                    prodS.ID_PRODUTO = p.ID_PRODUTO;
                    _connection.SQLServerContext.TB_TIPO_PRODUTO.Add(prodS);
                }
                prodS.DT_VIGENCIA_INICIAL = (DateTime)prodF.DT_INICIO;
                prodS.DT_VIGENCIA_FINAL = (DateTime)prodF.DT_TERMINO;
                prodS.DS_USUARIO = prodF.USUARIO;
            }

            _connection.SQLServerContext.SaveChanges();
            LogHelper.Log("Atualização da estrutura de produtos concluído");

            SyncEstrutura();

        }

        private void SyncEstrutura()
        {
            LogHelper.Log("Sincronizando itens estrutura de Produtos");
            var tiposFirebird = _connection.FirebirdContext.PCESTPRO_I;
            LogHelper.Log(String.Format("{0} registros a serem atualizados", tiposFirebird.Count()));
            var tiposSQLServer = _connection.SQLServerContext.TB_ESTRUTURA_TIPO_PRODUTO;

            foreach (var prodF in tiposFirebird)
            {
                LogHelper.Process();
                var prodS = tiposSQLServer.Where(s => s.ID_TIPO_PRODUTO == prodF.ESTRUT_ID && s.ID_ESTRUTURA == prodF.ITEM_ID).FirstOrDefault();

                if (prodS == null)
                {
                    prodS = new TB_ESTRUTURA_TIPO_PRODUTO();
                    prodS.ID_TIPO_PRODUTO = prodF.ESTRUT_ID;
                    prodS.ID_ESTRUTURA = prodF.ITEM_ID;
                    _connection.SQLServerContext.TB_ESTRUTURA_TIPO_PRODUTO.Add(prodS);
                }
                var p = _connection.SQLServerContext.TB_PRODUTO.Where(s => s.CD_PRODUTO == prodF.INSUMO).FirstOrDefault();
                var pai = _connection.SQLServerContext.TB_PRODUTO.Where(s => s.CD_PRODUTO == prodF.INSUMO_PAI).FirstOrDefault();

                prodS.ID_INSUMO = p.ID_PRODUTO;
                prodS.NR_NIVEL = prodF.NIVEL;
                prodS.QT_INSUMO = (int)prodF.QTDE;
                if (pai != null)
                {
                    prodS.ID_INSUMO_PAI = pai.ID_PRODUTO;
                }
            }

            _connection.SQLServerContext.SaveChanges();
            LogHelper.Log("Atualização dos itens da estrutura de produtos concluído");

        }
    }
}
