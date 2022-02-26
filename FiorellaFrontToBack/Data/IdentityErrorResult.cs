using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiorellaFrontToBack.Data
{
    public class IdentityErrorResult:IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {   Code="Email",
                Description=" Bu Email movcuddur"
            };
        }
        public override IdentityError DuplicateUserName(string userName)
        {
            return   new IdentityError
            {
                Code = "Username",
                Description = " Bu Username movcuddur"
            }; 
        }
    }
}
