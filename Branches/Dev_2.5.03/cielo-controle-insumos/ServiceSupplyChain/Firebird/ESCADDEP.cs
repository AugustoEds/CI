//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceSupplyChain.Firebird
{
    using System;
    using System.Collections.Generic;
    
    public partial class ESCADDEP
    {
        public ESCADDEP()
        {
            this.TB_PRE_REC = new HashSet<TB_PRE_REC>();
            this.TB_PRE_REC1 = new HashSet<TB_PRE_REC>();
        }
    
        public int CODIGO { get; set; }
        public string DESCRICAO { get; set; }
        public string TIPO_DEP { get; set; }
        public Nullable<int> FORNECEDOR { get; set; }
        public Nullable<int> CLIENTE { get; set; }
        public Nullable<System.DateTime> DATA_CAD { get; set; }
        public Nullable<System.DateTime> DATA_ALT { get; set; }
        public string CLASSIFICACAO_DEP { get; set; }
        public Nullable<int> CODIGO_CLASSIFICACAO { get; set; }
        public Nullable<int> FORNCEDOR_NF { get; set; }
        public Nullable<int> FORNECEDOR_NF { get; set; }
        public string FLAG_PORTAL_LAB { get; set; }
        public string CONTROLAESTOQUE { get; set; }
    
        public virtual ICollection<TB_PRE_REC> TB_PRE_REC { get; set; }
        public virtual ICollection<TB_PRE_REC> TB_PRE_REC1 { get; set; }
    }
}
