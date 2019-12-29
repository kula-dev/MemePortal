using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MemesPortal.Models;

namespace MemesPortal.Models
{
    public class MemesDB : DbContext
    {
        public MemesDB(DbContextOptions<MemesDB> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Users>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Memes> Memes { get; set; }
        public DbSet<MemesPortal.Models.Likes> Likes { get; set; }
    }
}
