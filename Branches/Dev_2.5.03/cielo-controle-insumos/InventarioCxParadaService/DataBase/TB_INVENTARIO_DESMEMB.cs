//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InventarioCxParadaService.DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class TB_INVENTARIO_DESMEMB
    {
        public int ID_DESEMEMB { get; set; }
        public Nullable<int> ID_INVENTARIO { get; set; }
        public Nullable<int> ID_CAIXA_ORIGINAL { get; set; }
        public Nullable<int> ID_CAIXA_DESTINO { get; set; }
    
        public virtual TB_INVENTARIO TB_INVENTARIO { get; set; }
        public virtual TB_CAIXA TB_CAIXA { get; set; }
        public virtual TB_CAIXA TB_CAIXA1 { get; set; }
    }
}