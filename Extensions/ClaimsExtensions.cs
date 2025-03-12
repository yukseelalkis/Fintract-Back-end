using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace api.Extensions
{
    public static class ClaimsExtensions
    {
        //Kullanıcıdan gelen JWT token veya Identity verileri içindeki "givenname" (adı) bilgisini çeker.
        // public static string  GetUserName(this ClaimsPrincipal  user ){
        // return user.Claims
        //     .SingleOrDefault(x => x.Type.Equals("https://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"))
        //     ?.Value ?? "Unknown User";
        // }
        public static string GetUserName(this ClaimsPrincipal user)
{
    return user.Claims
        .FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")?.Value
        ?? "Unknown User";
}

    }
}