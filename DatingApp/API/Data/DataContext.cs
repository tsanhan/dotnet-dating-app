using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
 
        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> Likes { get; set; } 
        //1. add DbSet for Messages
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); 

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

            //2. add Fluent API for Messages
            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);// * if the sender deletes the message, the recipient still can see it.
            
            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict); //restrict => if the recipient deletes the message, the sender still can see it

            //3. back to README.md
        }

    }
}