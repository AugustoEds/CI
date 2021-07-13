using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain.ViewModel
{
    public class RelCadeiaSuprimentosModelView
    {
        public string cd_produto { get; set; }
        public int id_deposito { get; set; }
        public int qt_produto_sucata { get; set; }
        public int qt_produto_rpa { get; set; }
        public int qt_produto_npa { get; set; }
        public int qt_produto_rpt_triagem { get; set; }
        public int qt_produto_rpt_reparo { get; set; }
        public int qt_produto_tec_descontinuada { get; set; }
        public int qt_produto_rpt_garantia { get; set; }
    }
}
