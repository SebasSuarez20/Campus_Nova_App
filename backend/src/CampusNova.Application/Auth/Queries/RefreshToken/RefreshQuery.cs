using CampusNova.Application.User.Queries.Login.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Application.Auth.Queries.RefreshToken
{
    public class RefreshQuery : IRequest<UserInformationDTO?>
    {
        public string RefreshToken { get; set; }
    }
}
