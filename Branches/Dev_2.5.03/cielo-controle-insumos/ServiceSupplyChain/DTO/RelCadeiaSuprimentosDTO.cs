using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.DTO
{
    public class RelCadeiaSuprimentosDTO
    {
        public string DS_SITUACAO { get; set; }
        public string CD_SAP { get; set; }
        public string DS_SUBGRUPO_PRODUTO { get; set; }
        public string DS_PRODUTO { get; set; }
        public int ID_DEPOSITO { get; set; }
        public string DS_DEPOSITO { get; set; }
        public string ID_GRUPO_PRODUTO { get; set; }
        public string DS_GRUPO_PRODUTO { get; set; }
        public int QT_ESTOQUE { get; set; }
    }
}
