using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    // ctrl+. on the class name to add a nice constructor 
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
 
        // there is a reference to the Users Table
        public DbSet<AppUser> Users { get; set; }
    }
}