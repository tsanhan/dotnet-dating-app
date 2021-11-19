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
using System.Linq;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        //1. we'll start with a single user
        //2. what we tals about is something called 'projection'
        public async Task<MemberDto> GetMemberAsync(string username)
        {
            //3. lets say I dont want to use AutoMapper
            // return await _context.Users
            // .Where(x => x.UserName == username)
            // .Select(user => new MemberDto //4. it's all manual
            // {
            //     //6. the manual mapping process
            //     Id = user.Id,
            //     Username = user.UserName
            //     // etc... but AutoMapper can do it for us
            //     // before we see when we done here lets see what we have right now!
            //     // in postman, lets run 'get user by username' and see in the terminal how the query looks like
            //     // we see in the terminal the query of lots of properties.
            //     // some we don't need, what's not efficient!
            //     // lets improve this, it's called '*projecting* from the repository'
            // })
            // .SingleOrDefaultAsync();//5. SingleOrDefaultAsync is when we ACTUALLY execute the query

            //7. we replace the Select with ProjectTo (from Automapper)
            return await _context.Users
            .Where(x => x.UserName == username)
            // 8. ProjectTo accepts a configuration provider
            // this is effectively the AutoMapperProfiles (the configuration we provided to AutoMapper)
            // lets see if we made any improvements (go to UsersController.cs)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            //9. do the same for the GetMembersAsync
            return await _context.Users
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUserNameAsync(string username)
        {
            return await _context.Users
            .Include(x => x.Photos)// we include the photos in the response
            .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
            .Include(x => x.Photos) // we include the photos in the response 
            .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry<AppUser>(user).State = EntityState.Modified;
        }
    }
}