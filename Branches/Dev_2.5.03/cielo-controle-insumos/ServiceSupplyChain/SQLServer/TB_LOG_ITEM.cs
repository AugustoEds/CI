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
    
    public partial class TB_LOG_ITEM
    {
        public int ID_LOG_ITEM { get; set; }
        public int ID_LOG_MASTER { get; set; }
        public string DS_PROCESSO { get; set; }
        public string MENSAGEM { get; set; }
    
        public virtual TB_LOG_MASTER TB_LOG_MASTER { get; set; }
    }
}
