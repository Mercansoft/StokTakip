﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StokTakip.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class StokTakipEntities : DbContext
    {
        public StokTakipEntities()
            : base("name=StokTakipEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Kategori> Kategori { get; set; }
        public virtual DbSet<KatUrun> KatUrun { get; set; }
        public virtual DbSet<Raf> Raf { get; set; }
        public virtual DbSet<RafUrun> RafUrun { get; set; }
        public virtual DbSet<Urun> Urun { get; set; }
        public virtual DbSet<Kullanici> Kullanici { get; set; }
    }
}