﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eomat.ViewModels
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class eveLocationEntities : DbContext
    {
        public eveLocationEntities()
            : base("name=eveLocationEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<mapConstellations> mapConstellations { get; set; }
        public virtual DbSet<mapDenormalize> mapDenormalize { get; set; }
        public virtual DbSet<mapRegions> mapRegions { get; set; }
        public virtual DbSet<mapSolarSystemJumps> mapSolarSystemJumps { get; set; }
        public virtual DbSet<mapSolarSystems> mapSolarSystems { get; set; }
        public virtual DbSet<staStations> staStations { get; set; }
    }
}