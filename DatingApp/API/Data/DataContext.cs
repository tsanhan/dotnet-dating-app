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

        //1. we didint need a DBset for photos, because we are not using it in the API.
        //  we do however go something withe the likes.
        public DbSet<UserLike> Likes { get; set; } // 2. table name is Likes

        //3. we need to give the entities some configuration
        // the way to do that is to overide a method in DbContext called OnModelCreating
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); //4. first of all, do the thing that the base class does 

            builder.Entity<UserLike>()
                //5. configure a PK for the table (we didn't configure a primary key)
                // and it will be the combination of the two PKs
                .HasKey(k => new { k.LikedUserId, k.SourceUserId });

            //6. configure the relationship between the two PKs
            builder.Entity<UserLike>()
                .HasOne(u => u.SourceUser)
                .WithMany(u => u.LikedUsers)
                .HasForeignKey(u => u.SourceUserId)
                // 7. cascade delete: if we delete the user we also delete the entity (if you using SQL SERVER, use NoAction, other wise you'll get a migration error)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.Entity<UserLike>()
                .HasOne(u => u.LikedUser)
                .WithMany(u => u.LikedByUsers)
                .HasForeignKey(u => u.LikedUserId)
                .OnDelete(DeleteBehavior.Cascade);
                //8. back to readme.md
        }
    }
}