using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_Role_Based_Authorization.Data.Identity;
using WebAPI_Role_Based_Authorization.Models;

namespace WebAPI_Role_Based_Authorization.Intefaces
{
    public interface IUserService
    {
        Task<TokenResponse> Authenticate(Login login);
        Task<ApplicationUser> GetById(string id);
        Task<IEnumerable<ListUser>> GetAll();
        Task<bool> Register(User user);
        Task<bool> ChangePassword(ChangePassword changePassword);
        Task<bool> UpdateUser(User user);
    }
}
