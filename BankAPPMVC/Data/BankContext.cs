using BankPPP.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace BankPPP.Data
{
    public class BankContext : DbContext
    {
        public BankContext(DbContextOptions<BankContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Account>  Accounts { get; set; }
        public DbSet<Transaction>? Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Account>().ToTable("Accounts");
            modelBuilder.Entity<Transaction>().ToTable("Transactions");
           
            modelBuilder.Entity<User>().HasData(
               new User
               {
                   userId = "User_1",// for test only  Guid.NewGuid().ToString("N").ToUpper(),
                   name = "Nader Ssam",
                   date_created = DateTime.Now
               }
           );

        }
       
    }
    public static class Configuration
    {
        public static void Seed(BankContext context)
        {
            if (context.Users!= null)
            {
                if (!context.Users.Any())
                {
                    var users = new List<User>
                {
                   new User
                   {
                       userId = "User_1",// for test only  Guid.NewGuid().ToString("N").ToUpper(),
                       name = "Nader Ssam",
                       date_created = DateTime.Now
                   }
                };
                    context.AddRange(users);
                    context.SaveChanges();
                }
            }
          
              
        }
    }
}