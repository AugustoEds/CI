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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class YepLogEntities : DbContext
    {
        public YepLogEntities()
            : base("name=YepLogEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<TB_INVENTARIO> TB_INVENTARIO { get; set; }
        public DbSet<TB_INVENTARIO_DESMEMB> TB_INVENTARIO_DESMEMB { get; set; }
        public DbSet<TB_INVENTARIO_PAUSA> TB_INVENTARIO_PAUSA { get; set; }
        public DbSet<TB_ITENS_INVENTARIO> TB_ITENS_INVENTARIO { get; set; }
        public DbSet<TB_CAIXA> TB_CAIXA { get; set; }
        public DbSet<ESCADPRO> ESCADPRO { get; set; }
        public DbSet<TB_PREC_REC_ITEM> TB_PREC_REC_ITEM { get; set; }
        public DbSet<TB_USUARIO> TB_USUARIO { get; set; }
        public DbSet<TB_TORRE_INVENTARIO> TB_TORRE_INVENTARIO { get; set; }
        public DbSet<TB_TORRE_USR_INVENTARIO> TB_TORRE_USR_INVENTARIO { get; set; }
        public DbSet<TB_STATUS_ITEM_INVENTARIO> TB_STATUS_ITEM_INVENTARIO { get; set; }
    }
}
