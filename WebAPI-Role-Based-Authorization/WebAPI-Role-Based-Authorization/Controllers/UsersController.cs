using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Role_Based_Authorization.Constant;
using WebAPI_Role_Based_Authorization.Intefaces;
using WebAPI_Role_Based_Authorization.Models;

namespace WebAPI_Role_Based_Authorization.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = AuthorizationConstants.Roles.ADMINISTRATORS)]
    public class UsersController : Controller
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate(Login login)
        {
            var response = await _userService.Authenticate(login);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            var response = await _userService.Register(user);
            if (response == false)
                return BadRequest(new { message = "Fail to create the user" });

            return Ok();
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePassword password)
        {
            var response = await _userService.ChangePassword(password);
            if (response == false)
                return BadRequest(new { message = "Fail to change the password" });

            return Ok();
        }

        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUser(User user)
        {
            var response = await _userService.UpdateUser(user);

            if (response == false)
                return BadRequest(new { message = "Fail to update the user" });

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }
    }
}
