using InventarioCxParadaService.DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace InventarioCxParadaService
{
    public static class StatusItemInventarioHelper
    {
        public static int AguardandoPrimeiraContagem { get { return 1; } }
        public static int EmPrimeiraContagem { get { return 2; } }
        public static int AguardandoSegundaContagem { get { return 3; } }
        public static int EmSegundaContagem { get { return 4; } }
        public static int AguardandoTerceiraContagem { get { return 5; } }
        public static int EmTerceiraContagem { get { return 6; } }
        public static int Inventariado { get { return 7; } }
        public static int Reprovado { get { return 8; } }
        public static int Extinto { get { return 9; } }
    }

    public partial class ServiceCXParada : ServiceBase
    {
        System.Timers.Timer tmrCxParada;
        System.Timers.Timer tmrControl;

        public ServiceCXParada()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            tmrControl = new System.Timers.Timer(60000); // Verifica a cada 1 min se tem inventário ativo
            tmrControl.AutoReset = true;
            tmrControl.Elapsed += new System.Timers.ElapsedEventHandler(this.tmrControl_Elapsed);
            tmrControl.Start();


        }

        private void tmrControl_Elapsed(object sender, ElapsedEventArgs e)
        {
            tmrControl.Stop();
            try
            {
                var db = new YepLogEntities();
                var inv = db.TB_INVENTARIO.Where(p => p.ID_STATUS_INVENTARIO == 2).FirstOrDefault(); // Pendente

                if (inv != null && tmrCxParada == null)
                {
                    if (inv.NR_INTERVALO_CX_PARADA != null)
                    {
                        var tempo = ((int)inv.NR_INTERVALO_CX_PARADA * 1000) * 60; // TRANSFORMA EM MINUTOS
                        tmrCxParada = new System.Timers.Timer(tempo);
                        tmrCxParada.AutoReset = true;
                        tmrCxParada.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
                        tmrCxParada.Start();
                    }
                }
                else if (inv == null && tmrCxParada != null)
                {
                    tmrCxParada.Dispose();
                    tmrCxParada = null;
                }
            }
            catch
            {
                
            }
            tmrControl.Start();
        }

        protected override void OnStop()
        {
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            tmrControl.Stop();

            tmrCxParada.Stop();
            try
            {
                var db = new YepLogEntities();
                var inv = db.TB_INVENTARIO.Where(p => p.ID_STATUS_INVENTARIO == 2).FirstOrDefault(); // Pendente

                if (inv != null)
                {
                    if (inv.NR_INTERVALO_CX_PARADA != null)
                    {
                        var dtIniExecucao = DateTime.Now;

                        var tempoParada = (int)inv.NR_TEMPO_CX_PARADA;
                        List<int> status = new List<int>();
                        status.Add(StatusItemInventarioHelper.EmPrimeiraContagem);
                        status.Add(StatusItemInventarioHelper.AguardandoSegundaContagem);
                        status.Add(StatusItemInventarioHelper.EmSegundaContagem);
                        status.Add(StatusItemInventarioHelper.AguardandoTerceiraContagem);
                        status.Add(StatusItemInventarioHelper.EmTerceiraContagem);

                        //var caixas = db.TB_ITENS_INVENTARIO.Where(p => status.Contains((int)p.ID_STATUS_ITEM_INVENTARIO)).ToList();
                        var caixas = inv.TB_ITENS_INVENTARIO.Where(p => status.Contains((int)p.ID_STATUS_ITEM_INVENTARIO)).ToList();
                        List<TB_ITENS_INVENTARIO> paradas = new List<TB_ITENS_INVENTARIO>();
                        foreach (var caixa in caixas)
                        {
                            if (caixa.DT_HR_P_BIPAGEM != null)
                            {
                                if (caixa.ID_STATUS_ITEM_INVENTARIO == StatusItemInventarioHelper.EmPrimeiraContagem)
                                {
                                    if (caixa.DT_HR_P_BIPAGEM < dtIniExecucao.AddMinutes(tempoParada * -1))
                                    {
                                        paradas.Add(caixa);
                                    }
                                }
                                else if (caixa.ID_STATUS_ITEM_INVENTARIO == StatusItemInventarioHelper.AguardandoSegundaContagem ||
                                         caixa.ID_STATUS_ITEM_INVENTARIO == StatusItemInventarioHelper.EmSegundaContagem)
                                {
                                    if (caixa.DT_HR_P_CONTAGEM < dtIniExecucao.AddMinutes(tempoParada * -1))
                                    {
                                        paradas.Add(caixa);
                                    }
                                }
                                else if (caixa.ID_STATUS_ITEM_INVENTARIO == StatusItemInventarioHelper.AguardandoTerceiraContagem ||
                                         caixa.ID_STATUS_ITEM_INVENTARIO == StatusItemInventarioHelper.EmTerceiraContagem)
                                {
                                    if (caixa.DT_HR_S_CONTAGEM < dtIniExecucao.AddMinutes(tempoParada * -1))
                                    {
                                        paradas.Add(caixa);
                                    }
                                }
                            }
                        }

                        if (paradas.Count > 0)
                        {
                            EnviarEmail(db, dtIniExecucao, tempoParada, inv, paradas);
                        }

                    }
                }

            }
            catch (Exception erro)
            {

            }

            tmrCxParada.Start();
            tmrControl.Start();
        }

        private void EnviarEmail(YepLogEntities db, DateTime dtIniExecucao, int tempoParada, TB_INVENTARIO inv, List<TB_ITENS_INVENTARIO> paradas)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Monitoramento de caixas do inventario</br>");
            sb.AppendLine(string.Format("Data : {0}</br>", dtIniExecucao.ToShortDateString()));
            sb.AppendLine(string.Format("Hora : {0}</br>", dtIniExecucao.ToShortTimeString()));
            sb.AppendLine("");

            sb.AppendLine("===========================================================================================================</br>");
            sb.AppendLine("Etiqueta                    Produto                            Status                Última Torre  Tempo</br>");
            sb.AppendLine("===========================================================================================================</br>");

            foreach(var caixa in paradas)
            {
                string linha = "";
                linha = caixa.TB_CAIXA.NR_ETIQUETA;

                var aux = " ";
                aux = aux + caixa.TB_CAIXA.TB_PREC_REC_ITEM.ESCADPRO.DESCRICAO.PadRight(34).Substring(0, 33);
                linha = linha + aux;

                aux = " ";
                aux = aux + caixa.TB_STATUS_ITEM_INVENTARIO.DS_STATUS_ITEM_INVENTARIO.PadRight(21).Substring(0, 20);
                linha = linha + aux;

                TB_TORRE_USR_INVENTARIO torre = null;
                TimeSpan tempo = new TimeSpan();

                if (caixa.ID_STATUS_ITEM_INVENTARIO == StatusItemInventarioHelper.EmPrimeiraContagem)
                {
                    torre = db.TB_TORRE_USR_INVENTARIO.Where(p => p.ID_USR_TORRE_INVENTARIO == caixa.ID_USR_TORRE_P_CONTAGEM).FirstOrDefault();

                    tempo = dtIniExecucao - (DateTime)caixa.DT_HR_P_BIPAGEM;
                }
                else if (caixa.ID_STATUS_ITEM_INVENTARIO == StatusItemInventarioHelper.AguardandoSegundaContagem ||
                         caixa.ID_STATUS_ITEM_INVENTARIO == StatusItemInventarioHelper.EmSegundaContagem)
                {
                    if (caixa.ID_USR_TORRE_S_CONTAGEM == null)
                    {
                        torre = db.TB_TORRE_USR_INVENTARIO.Where(p => p.ID_USR_TORRE_INVENTARIO == caixa.ID_USR_TORRE_P_CONTAGEM).FirstOrDefault();
                    }
                    else
                    {
                        torre = db.TB_TORRE_USR_INVENTARIO.Where(p => p.ID_USR_TORRE_INVENTARIO == caixa.ID_USR_TORRE_S_CONTAGEM).FirstOrDefault();
                    }
                    tempo = dtIniExecucao - (DateTime)caixa.DT_HR_P_CONTAGEM;
                }
                else if (caixa.ID_STATUS_ITEM_INVENTARIO == StatusItemInventarioHelper.AguardandoTerceiraContagem ||
                         caixa.ID_STATUS_ITEM_INVENTARIO == StatusItemInventarioHelper.EmTerceiraContagem)
                {
                    if (caixa.ID_USR_TORRE_T_CONTAGEM == null)
                    {
                        torre = db.TB_TORRE_USR_INVENTARIO.Where(p => p.ID_USR_TORRE_INVENTARIO == caixa.ID_USR_TORRE_S_CONTAGEM).FirstOrDefault();
                    }
                    else
                    {
                        torre = db.TB_TORRE_USR_INVENTARIO.Where(p => p.ID_USR_TORRE_INVENTARIO == caixa.ID_USR_TORRE_T_CONTAGEM).FirstOrDefault();
                    }
                    tempo = dtIniExecucao - (DateTime)caixa.DT_HR_S_CONTAGEM;
                }

                var usuario = db.TB_USUARIO.Where(p => p.ID_USUARIO == torre.ID_USUARIO).FirstOrDefault();
                aux = " ";
                //aux = aux + usuario.NM_USUARIO.PadRight(14).Substring(0, 13);
                aux = aux + torre.TB_TORRE_INVENTARIO.DS_TORRE_INVENTARIO.PadRight(14).Substring(0, 13);
                linha = linha + aux;

                int horas = tempo.Days * 24;
                horas = horas + tempo.Hours;
                int minutos = tempo.Minutes;
                int segundos = tempo.Seconds;

                aux = horas.ToString("D2") + ":" + minutos.ToString("D2") + ":" + segundos.ToString("D2");
                linha = linha + aux;

                sb.AppendLine("" + linha + "</br>");

            }
            sb.AppendLine("===========================================================================================================</br>");
            sb.AppendLine(string.Format("Total                 {0} Etiqueta</br>", paradas.Count));
            sb.AppendLine("===========================================================================================================</br>");

            string emailFrom = inv.DS_EMAIL_CX_PARADA;
            
            var emailService = new YepLogWebReference.ActionToolUi();
            
            emailService.SendEmail("Monitoramento de caixas no Inventário", sb.ToString(), emailFrom, "");

        }
    }
}