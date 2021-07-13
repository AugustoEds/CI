using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.DTO
{
    public class RelPerdaCampoDTO
    {
        public DateTime DT_RESUMO { get; set; }
        public int NR_ROMANEIO { get; set; }
        public string DS_MODELO_EQUIPAMENTO { get; set; }
        public string NR_SERIE_EQUIPAMENTO { get; set; }
        public string NR_SERIAL_FABRICANTE { get; set; }
        public int ID_DEPOSITO_ORIGEM { get; set; }
        public string ID_PRODUTO { get; set; }
        public int QT_PRODUTO { get; set; }
        public string DS_TIPO { get; set; }

    }
}
