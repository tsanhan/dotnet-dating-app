using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    //1. switch DbContext to IdentityDbContext
    // * we don't have IdentityDbContext instance in our project
    // * we need to install Microsoft.AspNetCore.Identity.EntityFrameworkCore package from Nuget
    // *** don't forget to select .net 5 ***
    // * now, because we want to access the user roles 
    // * we need to provide IdentityDbContext with a type parameters how these are mapped to the database
    //   * now if there was only one role per user (no joint table it wat easy and we only need to add typing: <AppUser,AppRole,int>. user, role, how they connet )
    //   * but because we want to have a joint table (we want many2many relationship between users and roles)
    //   * we need to add all the types involved with user, roles, claims, other types we need to add to the identity:
    public class DataContext : IdentityDbContext<
        AppUser, 
        AppRole,
        int, // identified using an int
        IdentityUserClaim<int>, // user claim will int as key
        AppUserRole,  // user role will be mapped to the joint table
        IdentityUserLogin<int>, // user login will int as key
        IdentityRoleClaim<int>, // role claim will int as key
        IdentityUserToken<int> // user token will int as key
        > //DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        //2. notice we have a warning here about us overriding the Users table
        // * so we don't need it, IdentityDbContext already has it
        // public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> Likes { get; set; } 
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); 

            //3. configure the relationship between AppUser, AppRole through many2many relationship
            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(aur => aur.User)
                .HasForeignKey(aur => aur.UserId)
                .IsRequired();
            //4. and the other side of this relationship
            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(aur => aur.Role)
                .HasForeignKey(aur => aur.RoleId)
                .IsRequired();
            //5. back to README.md
            builder.Entity<UserLike>()
                .HasKey(k => new { k.LikedUserId, k.SourceUserId });

            builder.Entity<UserLike>()
                .HasOne(u => u.SourceUser)
                .WithMany(u => u.LikedUsers)
                .HasForeignKey(u => u.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.Entity<UserLike>()
                .HasOne(u => u.LikedUser)
                .WithMany(u => u.LikedByUsers)
                .HasForeignKey(u => u.LikedUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict); 

        }

    }
}