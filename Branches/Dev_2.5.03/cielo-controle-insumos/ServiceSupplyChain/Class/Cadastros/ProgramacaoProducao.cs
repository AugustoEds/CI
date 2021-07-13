using ServiceSupplyChain.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.Class
{
    public class ProgramacaoProducao
    {
        private static ConnectionHelper _connection;

        public ProgramacaoProducao(ConnectionHelper connection)
        {
            _connection = connection;
        }

        public void Sync()
        {
            LogHelper.Log("Verificando programação da produção");

            _connection.SQLServerContext.SaveChanges();
            var programacoes = _connection.SQLServerContext.TB_PROGRAMACAO_PRODUCAO.Where(p => p.DT_VIGENCIA_INICIAL <= _connection.DataBase && p.FL_ABERTO == true).ToList();

            
            foreach (var item in programacoes)
            {
                LogHelper.Process();
                List<TB_PREVISAO_CONSUMO_PRODUCAO> listPrevisao = new List<TB_PREVISAO_CONSUMO_PRODUCAO>();

                foreach (var agrup in item.TB_AGRUPAMENTO_PROGRAMACAO_PRODUCAO.ToList())
                {
                    LogHelper.Process();

                    var prods = new List<int>();
                    foreach (var rel in agrup.TB_AGRUPAMENTO_TIPO_PRODUTO.RL_AGRUPAMENTO_TIPOS_PRODUTO.ToList())
                    {
                        LogHelper.Log("E");
                        foreach (var estrut in rel.TB_TIPO_PRODUTO.TB_ESTRUTURA_TIPO_PRODUTO.ToList())
                        {
                            LogHelper.Process();
                            if (prods.Where(p => p == estrut.ID_INSUMO).FirstOrDefault() == 0)
                            {
                                var prev = listPrevisao.Where(p => p.ID_PROGRAMACAO == item.ID_PROGRAMACAO && p.ID_INSUMO == estrut.ID_INSUMO).FirstOrDefault();
                                if (prev == null)
                                {
                                    prev = new TB_PREVISAO_CONSUMO_PRODUCAO();
                                    prev.ID_PROGRAMACAO = item.ID_PROGRAMACAO;
                                    prev.ID_INSUMO = estrut.ID_INSUMO;
                                    listPrevisao.Add(prev);
                                }
                                prev.QT_PREVISAO_DIA = prev.QT_PREVISAO_DIA + (agrup.QT_PREVISAO_PRODUCAO_DIA * estrut.QT_INSUMO);
                                prods.Add(estrut.ID_INSUMO);
                            }
                            else
                            {
                                LogHelper.Log("a");
                            }
                        }
                        
                    }

                }

                foreach(var prev in listPrevisao)
                {
                    LogHelper.Log("P");
                    LogHelper.Process();
                    _connection.SQLServerContext.TB_PREVISAO_CONSUMO_PRODUCAO.Add(prev);
                }

                item.FL_ABERTO = false;
                _connection.SQLServerContext.SaveChanges();
            }

            LogHelper.Log("Programação da produção verificada com sucesso");

        }
    }
}
