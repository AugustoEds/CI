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
    
    public partial class TB_REL_SAIDA_PRODUCAO
    {
        public int ID_RESUMO { get; set; }
        public System.DateTime DT_RESUMO { get; set; }
        public int ID_DEPOSITO_CD { get; set; }
        public int ID_PRODUTO { get; set; }
        public int QT_PRODUTO { get; set; }
    
        public virtual TB_DEPOSITO_CD TB_DEPOSITO_CD { get; set; }
        public virtual TB_PRODUTO TB_PRODUTO { get; set; }
    }
}
