using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PSDF_BSS
{
    public interface IUserService
    {
        ClaimsPrincipal GetUser();
    }

    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor accessor;

        public UserService(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
        }
        /// <summary>
        /// //Nasrullah
        /// </summary>
        /// <returns></returns>
        public ClaimsPrincipal GetUser()
        {
            return accessor?.HttpContext?.User as ClaimsPrincipal;
        }
    }
}
