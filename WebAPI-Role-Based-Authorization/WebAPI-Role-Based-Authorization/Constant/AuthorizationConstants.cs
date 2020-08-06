using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_Role_Based_Authorization.Constant
{
    public class AuthorizationConstants
    {
        public static class Roles
        {
            public const string ADMINISTRATORS = "Administrators";
        }

        // TODO: Don't use this in production
        public const string DEFAULT_PASSWORD = "Pass@word1";
    }
}
