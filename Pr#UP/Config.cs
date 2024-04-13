using Microsoft.EntityFrameworkCore;
using Pr_UP.Models;

namespace Pr_UP
{
    public class BaseDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /* try
      {*/
            optionsBuilder.UseSqlServer("Data Source=SUPERCOMP;Initial Catalog=IceRink;Integrated Security=True;Trusted_Connection = True; TrustServerCertificate = True");
       /* }
        catch (Exception ex) { 
            Console.WriteLine(ex.ToString());
        }*/
        }
    }

    public class IceRinkDB : BaseDbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Tickets> Tickets { get; set; }
        public DbSet<TicketType> TicketType { get; set; }
        public DbSet<Equipments> Equipments { get; set; }
        public DbSet<EquipmentType> EquipmentType { get; set; }
        public DbSet<Rental> Rental { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<Pass> Pass { get; set; }
        public DbSet<Qualification> Qualification { get; set; }
        public DbSet<Coaches> Coaches { get; set; }
        public DbSet<Training> Training { get; set; }


    }
}
