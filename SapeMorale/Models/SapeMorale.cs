namespace SapeMorale.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Data.Entity.Migrations;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class SapeMoraleDB : DbContext
    {
        // Your context has been configured to use a 'test' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'VGWagers.Models.test' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'test' 
        // connection string in the application configuration file.
        public SapeMoraleDB()
            : base("name=DefaultConnection")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<sape_morale> sape_morale { get; set; }
    }   

    public partial class sape_morale
    {
        [Key]
        public int MORALERECORDID { get; set; }

        public string IPADDRESS { get; set; }
        public string MORALE { get; set; }
        public System.DateTime LASTUPDATEDDATE { get; set; }
    }

    
}