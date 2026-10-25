using CampusNova.Application.Common.Interfaces;
using System.Linq;
using CampusNova.Application.User.Queries.Login.DTO;
using CampusNova.Domain.User.Interfaces;
using CampusNova.Domain.User.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Application.Auth.Queries.RefreshToken
{
    public class RefreshHandler : IRequestHandler<RefreshQuery, UserInformationDTO?>
    {
        private readonly IUserRepository _dbQueriesUser;
        private readonly IJwtService _jwtService;

        public RefreshHandler(IUserRepository dbQueriesUser, IJwtService jwtService)
        {
            _dbQueriesUser = dbQueriesUser;
            _jwtService = jwtService;
        }

        public async Task<UserInformationDTO?> Handle(RefreshQuery request, CancellationToken cancellationToken)
        {

            var main = _jwtService.ValidateRefreshToken(request.RefreshToken);

            if (main is null) return null;

            // Con el mapeo estándar de claims, ClaimTypes.NameIdentifier funciona directamente.
            var userIdClaim = main.FindFirst("authId");
            var institutionClaim = main.FindFirst("institution");

            if (userIdClaim is null || institutionClaim is null) return null;

            var userId = long.Parse(userIdClaim.Value);


            var user = await _dbQueriesUser.GetUserForId(userId,long.Parse(institutionClaim.Value));
            if (user is null) return null;

            var newAccessToken = _jwtService.GeneratorToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken(user);

            return new UserInformationDTO
            {
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            };

        }
    }
}
