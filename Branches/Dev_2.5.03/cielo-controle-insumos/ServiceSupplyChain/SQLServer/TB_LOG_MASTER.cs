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
    
    public partial class TB_LOG_MASTER
    {
        public TB_LOG_MASTER()
        {
            this.TB_LOG_ITEM = new HashSet<TB_LOG_ITEM>();
        }
    
        public int ID_LOG_MASTER { get; set; }
        public Nullable<System.DateTime> DT_INICIO_EXEC { get; set; }
        public Nullable<System.DateTime> DT_FIM_EXEC { get; set; }
        public string MENSAGEM { get; set; }
    
        public virtual ICollection<TB_LOG_ITEM> TB_LOG_ITEM { get; set; }
    }
}
