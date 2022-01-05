using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API.Controllers
{

    public class AccountController : BaseApiController
    {
        // private readonly DataContext _context;// 1. now that Identity takes care of AppUser entity, we don't need this
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(
            //DataContext context, //2. remove this
            UserManager<AppUser> userManager,//4. we'll use userManager insted (create and assign field field from parameter)
            SignInManager<AppUser> signInManager,// 5. we'll also use signInManager  (create and assign field field from parameter)
            ITokenService tokenService,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
            //_context = context; //3. remove this

        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExist(registerDto.Username)) return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(registerDto);

            user.UserName = registerDto.Username.ToLower();

            //6. up until now it's ok.
            // * instead of using the context, we use the userManager
            // _context.Users.Add(user);
            // await _context.SaveChangesAsync();
            // * this will both create the user and save the changes to the database
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            //7. if the user did not created successfully, we'll return the errors
            if (!result.Succeeded) return BadRequest(result.Errors);

            //8. if the user is created successfully, we want to return the user
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            //9. we'll get the Users table from the manager
            // var user = await this._context.Users
            var user = await this._userManager.Users
                .Include(x => x.Photos)
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("invalid username");

            //10. we'll use out signInManager to sign the user in
            // * this will both check the password and sign in the user
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, /*not to lock out on failure*/false);

            //11. if the user is not signed in successfully, we'll return the errors
            if (!result.Succeeded) return Unauthorized("invalid password");

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender

            };
        }

        private async Task<bool> UserExist(string username)
        {
            //12. use userManager
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
            //13. back to README.md
        }
    }
}