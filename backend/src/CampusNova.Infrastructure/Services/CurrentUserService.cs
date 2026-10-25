using CampusNova.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserServices
    {

        private readonly IHttpContextAccessor _httpContext;

        public CurrentUserService(IHttpContextAccessor httpContext) => _httpContext = httpContext;

        public string ContextUser() => _httpContext.HttpContext?.User?
             .FindFirstValue("authId") ?? "System";

        public string ContextInstitution() => _httpContext.HttpContext?.User?.FindFirstValue("institution") ?? "00-1";

    }
}
