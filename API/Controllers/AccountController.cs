using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entity;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext conext;
        private readonly ITokenService tokenService;

        public AccountController(DataContext conext, ITokenService tokenService)
        {
            this.conext = conext;
            this.tokenService = tokenService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto userInfo)
        {
            if (await UserExists(userInfo.UserName) == true)
            {
                return BadRequest("Invalid Username");
            }
            using var hmac = new HMACSHA512();
            var user = new AppUser()
            {
                UserName = userInfo.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userInfo.Password)),
                PasswordSalt = hmac.Key
            };

            conext.Users.Add(user);
            await conext.SaveChangesAsync();

            return new UserDto
            {
                Token = tokenService.CretateToken(user),
                Username = userInfo.UserName
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await conext.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.userName);

            if (user == null)
            {
                return Unauthorized("Invalid Username");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));

            for (int index = 0; index < ComputeHash.Length; index++)
            {
                if (ComputeHash[index] != user.PasswordHash[index])
                {
                    return Unauthorized("Invalid password");
                }
            }

            return new UserDto()
            {
                Token = tokenService.CretateToken(user),
                Username = loginDto.userName
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await conext.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        }

    }
}