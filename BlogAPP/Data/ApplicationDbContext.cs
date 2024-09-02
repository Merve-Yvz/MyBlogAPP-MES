using BlogAPP.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Reflection.Metadata;

namespace BlogAPP.Data
{
    public class ApplicationDbContext:DbContext  
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryID = 1, CategoryName = "Technology" },
                new Category { CategoryID = 2, CategoryName = "Photography" },
                new Category { CategoryID = 3, CategoryName = "Movie" },
                new Category { CategoryID = 4, CategoryName = "Travel" },
                new Category { CategoryID = 5, CategoryName = "Sports" }


                 );
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleID = 1, RoleName = "Admin" },
                new Role { RoleID = 2, RoleName = "User" });
            modelBuilder.Entity<User>().HasData(
                new User { UserID = 1, UserName = "Merve", UserSurname = "Yavuz", UserEmail = "merve@gmail.com", UserPassword = "1234", RoleID = 1 });


        }
    }
}
