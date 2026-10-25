using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Infrastructure.Services
{
    public static class CookiesServices
    {

        public static CookieOptions AccessTokenOptions =>
        new()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/",
            MaxAge = TimeSpan.FromMinutes(15)
        };


        public static CookieOptions RefreshTokenOptions =>
      new()
      {
          HttpOnly = true,
          Secure = true,
          SameSite = SameSiteMode.None,
          Path = "/",
          MaxAge = TimeSpan.FromDays(1)
      };

    }
}
