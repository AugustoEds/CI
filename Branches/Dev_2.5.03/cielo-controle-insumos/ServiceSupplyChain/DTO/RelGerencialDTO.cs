using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.DTO
{
    public class RelGerencialDTO
    {
        public string CD_PRODUTO { get; set; }
        public int QT_ESTOQUE_RPA { get; set; }
        public int QT_ESTOQUE_TRIAGEM { get; set; }
        public int QT_ESTOQUE_TEC_DESCONTINUADA { get; set; }
        public int QT_ESTOQUE_NPA { get; set; }
        public int QT_ESTOQUE_RPT_REPARO { get; set; }
        public int QT_ESTOQUE_SUCATA { get; set; }
        public int QT_ESTOQUE_RPT_GARANTIA { get; set; }

    }
}
