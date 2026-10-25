using CampusNova.Domain.User.Model;
using CampusNova.Domain.User.Model.ReadModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Application.Common.Interfaces
{
    public interface IJwtService
    {
       public string GeneratorToken(LoginAuthView model);
       public string GenerateRefreshToken(LoginAuthView model);
       public ClaimsPrincipal? ValidateRefreshToken(string token);
    }
}
