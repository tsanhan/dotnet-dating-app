using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Cryptography;
using API.Data;
using API.DTOs;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        //1. we need to inject the context to the repository
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        //2. we need to implement the methods
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUserNameAsync(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            // SaveChangesAsync returns the number of changes made to the database, I want to make suer at least one change was made
            return await _context.SaveChangesAsync() > 0; 
        }

        public void Update(AppUser user)
        {
            // just updating a flag to say 'yep that has been modified'
             _context.Entry<AppUser>(user).State = EntityState.Modified;
        }
    }
}