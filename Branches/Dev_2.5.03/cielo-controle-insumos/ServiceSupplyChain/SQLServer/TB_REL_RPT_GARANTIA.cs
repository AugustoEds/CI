//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceSupplyChain.SQLServer
{
    using System;
    using System.Collections.Generic;
    
    public partial class TB_REL_RPT_GARANTIA
    {
        public int ID_RESUMO { get; set; }
        public System.DateTime DT_RESUMO { get; set; }
        public int ID_PRODUTO { get; set; }
        public string TP_MOVIMENTO { get; set; }
        public int QT_PRODUTO { get; set; }
        public int ID_LOTE { get; set; }
    
        public virtual TB_PRODUTO TB_PRODUTO { get; set; }
    }
}
