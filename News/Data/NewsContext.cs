using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using News.Models;

namespace News.Data
{
    public class NewsContext : DbContext
    {
        public NewsContext (DbContextOptions<NewsContext> options)
            : base(options)
        {
        }

        public DbSet<News.Models.Post> Post { get; set; } = default!;

        public DbSet<News.Models.Category> Category { get; set; } = default!;

        public DbSet<User>? Users { get; set; }
        public DbSet<Role>? Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new Role { RoleId = 1, RoleName = "admin" });
            modelBuilder.Entity<Role>().HasData(new Role { RoleId = 2, RoleName = "user" });
            modelBuilder.Entity<User>().HasData(new User { UserId = 1, UserName = "admin", Password = "11111", RoleId = 1 });
        }

    }
}
