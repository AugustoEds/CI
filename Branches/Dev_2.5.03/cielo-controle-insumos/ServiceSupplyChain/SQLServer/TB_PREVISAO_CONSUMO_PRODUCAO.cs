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
    
    public partial class TB_PREVISAO_CONSUMO_PRODUCAO
    {
        public int ID_PREVISAO { get; set; }
        public int ID_PROGRAMACAO { get; set; }
        public int ID_INSUMO { get; set; }
        public int QT_PREVISAO_DIA { get; set; }
    
        public virtual TB_PRODUTO TB_PRODUTO { get; set; }
        public virtual TB_PROGRAMACAO_PRODUCAO TB_PROGRAMACAO_PRODUCAO { get; set; }
    }
}
