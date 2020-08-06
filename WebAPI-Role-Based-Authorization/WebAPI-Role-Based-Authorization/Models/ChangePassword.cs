using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_Role_Based_Authorization.Models
{
    public class ChangePassword
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
    }
}
