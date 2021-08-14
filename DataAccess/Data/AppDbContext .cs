using iread_school_ms.DataAccess.Data.Entity;
using Microsoft.EntityFrameworkCore;

using System;

namespace iread_school_ms.DataAccess.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        //entities
        public DbSet<School> Schools { get; set; }
        public DbSet<SchoolManager> SchoolManagers { get; set; }

    }
}
