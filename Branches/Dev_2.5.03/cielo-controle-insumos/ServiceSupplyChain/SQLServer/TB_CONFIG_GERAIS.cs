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
    
    public partial class TB_CONFIG_GERAIS
    {
        public int ID_CONFIG { get; set; }
        public int ID_GRUPO_SUCATA { get; set; }
        public int ID_GRUPO_RPA { get; set; }
        public int ID_GRUPO_NPA { get; set; }
        public int ID_GRUPO_RPT_TRIAGEM { get; set; }
        public int ID_GRUPO_RPT_REPARO { get; set; }
        public int ID_GRUPO_TEC_DESCONTINUADA { get; set; }
        public int ID_GRUPO_RPT_GARANTIA { get; set; }
        public int ID_DEPOSITO_SB { get; set; }
    
        public virtual TB_DEPOSITO TB_DEPOSITO { get; set; }
        public virtual TB_GRUPO_PRODUTO TB_GRUPO_PRODUTO { get; set; }
        public virtual TB_GRUPO_PRODUTO TB_GRUPO_PRODUTO1 { get; set; }
        public virtual TB_GRUPO_PRODUTO TB_GRUPO_PRODUTO2 { get; set; }
        public virtual TB_GRUPO_PRODUTO TB_GRUPO_PRODUTO3 { get; set; }
        public virtual TB_GRUPO_PRODUTO TB_GRUPO_PRODUTO4 { get; set; }
        public virtual TB_GRUPO_PRODUTO TB_GRUPO_PRODUTO5 { get; set; }
        public virtual TB_GRUPO_PRODUTO TB_GRUPO_PRODUTO6 { get; set; }
    }
}
