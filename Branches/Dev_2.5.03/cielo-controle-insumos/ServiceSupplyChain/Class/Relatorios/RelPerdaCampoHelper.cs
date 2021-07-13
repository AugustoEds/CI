using ServiceSupplyChain.DTO;
using ServiceSupplyChain.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.Class
{
    public class RelPerdaCampoHelper
    {
        private static ConnectionHelper _connection;

        public RelPerdaCampoHelper(ConnectionHelper connection)
        {
            _connection = connection;
        }

        private string GetSqlFirebird()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select r.dt_response DT_RESUMO");
            sb.AppendLine("      , coalesce(r.romaneio_transferencia ,0) NR_ROMANEIO");
            sb.AppendLine("      , r.modelo_desc DS_MODELO_EQUIPAMENTO");
            sb.AppendLine("      , r.ident_ativo NR_SERIE_EQUIPAMENTO");
            sb.AppendLine("      , e.numero_serie_fab NR_SERIAL_FABRICANTE");
            sb.AppendLine("      , d.codigo ID_DEPOSITO_ORIGEM");
            sb.AppendLine("      , i.id_insumo ID_PRODUTO");
            sb.AppendLine("      , i.qtde QT_PRODUTO");
            sb.AppendLine("      , r.tipo DS_TIPO");
            sb.AppendLine("   from reversa_call r WITH(NOLOCK) ");
            sb.AppendLine("left join escadeqp e WITH(NOLOCK) on e.numero_serie = r.ind_ident_ativo");
            sb.AppendLine("left join escaddep d WITH(NOLOCK) on d.codigo = r.deposito");
            sb.AppendLine("left join reversa_apt_insumo i WITH(NOLOCK) on i.reversa_id = r.reversa_id");
            sb.AppendLine("left join escadpro p WITH(NOLOCK) on p.codigo = i.id_insumo");
            sb.AppendLine("left join essubpro s WITH(NOLOCK) on s.codigo = p.sub_grupo");
            sb.AppendLine(" where extract(month from r.dt_response) = {0}");
            sb.AppendLine("   and extract(year from r.dt_response) = {1}");
            sb.AppendLine("   and i.fl_recebido = 'N'");

            return sb.ToString();
        }

        public void ReadData()
        {
            //LogHelper.Log("Gerando dados perda de campo");
            //LogHelper.Log(GetSqlFirebird());

            //var dataBase = DateTime.Today;
            //// para garantir que não está sendo duplicado, iremos excluir os registros do dia, se houver
            //_connection.SQLServerContext.Database.ExecuteSqlCommand(string.Format("delete from TB_RESUMO_PERDA_CAMPO where month(DT_RESUMO) = {0} and year(DT_RESUMO) = {1}", dataBase.Month, dataBase.Year));

            //List<RelPerdaCampoDTO> itens = _connection.FirebirdContext.Database.SqlQuery<RelPerdaCampoDTO>(string.Format(GetSqlFirebird(), dataBase.Month, dataBase.Year)).ToList();
            //List<string> sqlInsert = new List<string>();
            //var strInsert = "INSERT INTO TB_RESUMO_PERDA_CAMPO (DT_RESUMO ,NR_ROMANEIO ,DS_MODELO_EQUIPAMENTO ,NR_SERIE_EQUIPAMENTO ,NR_SERIAL_FABRICANTE, ID_DEPOSITO_ORIGEM ,ID_PRODUTO ,1 QT_PRODUTO ,DS_TIPO) VALUES (CONVERT(DATETIME, '{0}', 111),{1},'{2}','{3}','{4}',{5},'{6}',{7},'{8}');";
            //foreach (var item in itens)
            //{
            //    LogHelper.Process();

            //    sqlInsert.Add(string.Format(strInsert, string.Format("{0}-{1}-{2}", item.DT_RESUMO.Year, item.DT_RESUMO.Month, item.DT_RESUMO.Day)
            //                                                , item.NR_ROMANEIO
            //                                                , item.DS_MODELO_EQUIPAMENTO
            //                                                , item.NR_SERIE_EQUIPAMENTO
            //                                                , item.NR_SERIAL_FABRICANTE
            //                                                , item.ID_DEPOSITO_ORIGEM
            //                                                , item.ID_PRODUTO
            //                                                , item.QT_PRODUTO
            //                                                , item.DS_TIPO));
            //}

            //foreach (var item in sqlInsert)
            //{

            //    LogHelper.Process();
            //    _connection.SQLServerContext.Database.ExecuteSqlCommand(item);
            //}

            //_connection.SQLServerContext.SaveChanges();

            //LogHelper.Log("Relatório perda de campo gerado com sucesso");

        }
    }
}
