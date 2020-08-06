using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI_Role_Based_Authorization.Data.Identity;
using WebAPI_Role_Based_Authorization.Helpers;
using WebAPI_Role_Based_Authorization.Intefaces;
using WebAPI_Role_Based_Authorization.Models;

namespace WebAPI_Role_Based_Authorization.Services
{
    public class UserService : IUserService
    {

        private readonly AppSettings _appSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppIdentityDbContext _dbContext;

        public UserService(
            IOptions<AppSettings> appSettings,
            UserManager<ApplicationUser> userManager,
            AppIdentityDbContext dbContext
            )
        {
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<TokenResponse> Authenticate(Login login)
        {
            var user = await _userManager.FindByNameAsync(login.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
            {
                var token = await GenerateJwtTokenAsync(user);
                return new TokenResponse(user, token);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> ChangePassword(ChangePassword changePassword)
        {
            var user = await _userManager.FindByNameAsync(changePassword.Username);

            if (user == null)
                return false;

            await _userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.Password);
            return true;

        }

        public async Task<IEnumerable<ListUser>> GetAll()
        {
            return await (from c in _dbContext.Users
                          select new ListUser
                          {
                              Username = c.UserName,
                              FirstName = c.FirstName,
                              LastName = c.LastName
                          }).ToListAsync();
        }

        public async Task<ApplicationUser> GetById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<bool> Register(User user)
        {
            var applicationUser = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = user.Username, Email = user.Username, FirstName = user.FirstName, LastName = user.LastName };
            var result = await _userManager.CreateAsync(applicationUser, user.Password);

            if (result.Succeeded)
            {
                applicationUser = await _userManager.FindByNameAsync(user.Username);
                await _userManager.AddToRolesAsync(applicationUser, user.Roles);

                return true;
            }
            return false;
        }

        public async Task<bool> UpdateUser(User user)
        {
            var _user = await _userManager.FindByNameAsync(user.Username);

            if (_user == null)
                return false;

            _user.FirstName = user.FirstName;
            _user.LastName = user.LastName;

            await _userManager.UpdateAsync(_user);

            var oldRoles = await _userManager.GetRolesAsync(_user);
            await _userManager.RemoveFromRolesAsync(_user, oldRoles);

            await _userManager.AddToRolesAsync(_user, user.Roles);

            return true;
        }


        private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, user.Id.ToString()));
            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
