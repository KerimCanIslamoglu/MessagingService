using MessagingService.DataAccess.Identity;
using MessagingService.Entities.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.DataAccess.Concrete
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=MSI\MSSQLSERVER14;Database=MessagingServiceDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Message> Messages { get; set; }
        public DbSet<BlockedUser> BlockedUsers { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<ErrorLogs> ErrorLogs { get; set; }

    }
}
