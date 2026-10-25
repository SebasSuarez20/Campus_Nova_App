using CampusNova.Application.Common;
using CampusNova.Application.Common.Interfaces;
using CampusNova.Application.User.Queries.Login.DTO;
using CampusNova.Domain.Common.Services;
using CampusNova.Domain.User.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Application.User.Queries.Login
{
    public class LoginHandler : IRequestHandler<LoginQuery, UserInformationDTO?>
    {
        private readonly IUserRepository _dbQueriesUser;
        private readonly IJwtService _jwtService;

        public LoginHandler(IUserRepository dbQueriesUser, IJwtService jwtService) {
             _dbQueriesUser = dbQueriesUser;
             _jwtService = jwtService;
        }

        
        public async Task<UserInformationDTO?> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            request.Password = HashServices.Hash(request.Password);
            var respAccess = await _dbQueriesUser.GetAcessToSystem(request.Username, request.Password,request.InstitutionCode);

            if (respAccess is not null)
            {
                if (!string.Equals(request.Password,respAccess.Password,StringComparison.OrdinalIgnoreCase)) return null;
                var modelResponse = new UserInformationDTO
                {
                    FirstName = respAccess.FirstName,
                    LastName = respAccess.LastName,
                    Identification = respAccess.Identification,
                    InstitutionCode = respAccess.Institution_Code ?? -1,
                    Token = _jwtService.GeneratorToken(respAccess),
                    Rol = respAccess.Rol ?? string.Empty,
                    RefreshToken = _jwtService.GenerateRefreshToken(respAccess)
                };

                return modelResponse;
            }

            return null;
        }




    }
}
