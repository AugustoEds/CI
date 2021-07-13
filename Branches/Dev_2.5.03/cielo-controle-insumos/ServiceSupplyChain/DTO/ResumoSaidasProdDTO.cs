using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.DTO
{
    public class ResumoSaidasProdDTO
    {
        public string DT_RECEBIMENTO { get; set; }
        public string ID_PRODUTO { get; set; }
        public int ID_DEPOSITO { get; set; }
        public int QT_PRODUTO { get; set; }
    }
}
