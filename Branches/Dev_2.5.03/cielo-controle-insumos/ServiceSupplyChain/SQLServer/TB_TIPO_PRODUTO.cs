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
    
    public partial class TB_TIPO_PRODUTO
    {
        public TB_TIPO_PRODUTO()
        {
            this.RL_AGRUPAMENTO_TIPOS_PRODUTO = new HashSet<RL_AGRUPAMENTO_TIPOS_PRODUTO>();
            this.TB_ESTRUTURA_TIPO_PRODUTO = new HashSet<TB_ESTRUTURA_TIPO_PRODUTO>();
        }
    
        public int ID_TIPO_PRODUTO { get; set; }
        public string DS_TIPO_PRODUTO { get; set; }
        public int ID_PRODUTO { get; set; }
        public int NR_VERSAO { get; set; }
        public System.DateTime DT_VIGENCIA_INICIAL { get; set; }
        public System.DateTime DT_VIGENCIA_FINAL { get; set; }
        public string DS_USUARIO { get; set; }
    
        public virtual ICollection<RL_AGRUPAMENTO_TIPOS_PRODUTO> RL_AGRUPAMENTO_TIPOS_PRODUTO { get; set; }
        public virtual ICollection<TB_ESTRUTURA_TIPO_PRODUTO> TB_ESTRUTURA_TIPO_PRODUTO { get; set; }
        public virtual TB_PRODUTO TB_PRODUTO { get; set; }
    }
}
