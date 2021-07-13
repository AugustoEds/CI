using ServiceSupplyChain.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.Class
{
    public class LogHelper
    {
        public static void Log(string Message)
        {
            // O log sempre vai ter uma nova instancia, pois devemos sempre gravar independente de erro ou não
            SQLServerEntities _sqlServerContext = new SQLServerEntities();

            Console.WriteLine("");
            Console.WriteLine(Message);
        }

        public static void Process()
        {
            Console.Write(".");
        }

        public static int GerarLogMaster(string Mensagem)
        {
            using (var context = new SQLServerEntities())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        TB_LOG_MASTER master = new TB_LOG_MASTER();
                        master.DT_INICIO_EXEC = DateTime.Now;
                        master.MENSAGEM = Mensagem;
                        context.TB_LOG_MASTER.Add(master);
                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        return master.ID_LOG_MASTER;

                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        return 0;
                    }
                }
            }
        }

        public static void GerarLogMasterFinalizada(int IdLog, string Mensagem)
        {
            if (IdLog != 0)
            {
                using (var context = new SQLServerEntities())
                {
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            TB_LOG_MASTER master = context.TB_LOG_MASTER.Where(p => p.ID_LOG_MASTER == IdLog).FirstOrDefault();
                            if (master != null)
                            {
                                master.DT_FIM_EXEC = DateTime.Now;
                                master.MENSAGEM = master.MENSAGEM + "//" + Mensagem;
                                context.SaveChanges();
                                dbContextTransaction.Commit();
                            }
                        }
                        catch (Exception)
                        {
                            dbContextTransaction.Rollback();
                        }
                    }
                }
            }
        }

        public static void GerarLogItem(int IdLog, string Mensagem, string Processo)
        {
            if (IdLog != 0)
            {
                using (var context = new SQLServerEntities())
                {
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            TB_LOG_ITEM item = new TB_LOG_ITEM();
                            item.ID_LOG_MASTER = IdLog;
                            item.DS_PROCESSO = Processo;
                            item.MENSAGEM = Mensagem;
                            context.TB_LOG_ITEM.Add(item);
                            context.SaveChanges();
                            dbContextTransaction.Commit();

                        }
                        catch (Exception)
                        {
                            dbContextTransaction.Rollback();
                        }
                    }
                }
            }
        }

    }
}
