using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.DTO
{
    public class ResumoSaldoEstoqueDTO
    {
        public int ID_DEPOSITO { get; set; }
        public string ID_PRODUTO { get; set; }
        public int ID_LOCAL_ESTOQUE { get; set; }
        public int QT_PRODUTO { get; set; }
    }
}
