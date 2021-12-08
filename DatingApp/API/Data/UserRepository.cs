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
using API.Helpers;

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

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
            .Where(x => x.UserName == username)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            // 4. use different approach
            // var query = _context.Users
            // .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            // .AsNoTracking();
            
            //5. get direct access to the entities
            var query = _context.Users.AsQueryable();

            //6. filter
            query = query.Where(x => x.UserName != userParams.CurrnetUsername);
            query = query.Where(x => x.Gender == userParams.Gender);

            //1. filter by 
            // per of 4.: query = query.Where(x => x.Username != userParams.CurrnetUsername);
            //2. this is a bad practice because we comparing properties before and after mapping via projection
            //  * we don't want to be working with the MemberDto, we want to filter before that
            //  * so we'll change strategy.


            return await PagedList<MemberDto>.CreateAsync(
                /*7. mapping the results only*/query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking(), 
                userParams.PageNumber, 
                userParams.PageSize);
            
            // 8. test in postman: section 13: Get Users No QS:
            //  * will get all users except me (logged in) and members of the opposite gender
            //  * test in postman section 13: 'Get Users with gender' works, get members of the specified gender
            // 9. back to readme.md
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUserNameAsync(string username)
        {
            return await _context.Users
            .Include(x => x.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
            .Include(x => x.Photos) 
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