using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.DTO
{
    public class ResumoMovtoDTO
    {
        public int ID_LOTE { get; set; }
        public DateTime DT_MOVIMENTO { get; set; }
        public string ID_PRODUTO { get; set; }
        public string TP_MOVIMENTO { get; set; }
        public int QT_PRODUTO { get; set; }
    }
}
