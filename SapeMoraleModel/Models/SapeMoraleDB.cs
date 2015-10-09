using System;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity.Migrations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SapeMoraleModel.Models
{
    public class SapeMoraleDB : DbContext
    {
        public SapeMoraleDB()
            : base("name=DefaultConnection")
        {
        }

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
