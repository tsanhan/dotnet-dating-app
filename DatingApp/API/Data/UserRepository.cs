using System;
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

            var query = _context.Users.AsQueryable();

            query = query.Where(x => x.UserName != userParams.CurrnetUsername);
            query = query.Where(x => x.Gender == userParams.Gender);
            query = query.Where(x => x.Gender == userParams.Gender);


            var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1); //i still want this year to be included
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

            query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

            // 1. add orderBy to the query (switch statement - since c# 8)

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                _ => query.OrderByDescending(x => x.LastActive),
            };
            // 2. test in postman section 13: 'Get Users orderedby Created', works
            // 3. test in postman section 13: 'Get Users orderedby lastActive', works
            return await PagedList<MemberDto>.CreateAsync(
                query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking(),
                userParams.PageNumber,
                userParams.PageSize);
            // 4. back to readme.md
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