using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.DTO
{
    public class ResumoEntradasDTO
    {
        
        public int ID_LOTE { get; set; }
        public string ID_PRODUTO { get; set; }
        public DateTime DT_RESUMO { get; set; }
        public int QT_PRODUTO { get; set; }
    }
}
