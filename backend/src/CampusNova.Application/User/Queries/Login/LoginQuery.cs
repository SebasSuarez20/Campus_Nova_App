using CampusNova.Application.Common;
using CampusNova.Application.User.Queries.Login.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Application.User.Queries.Login
{
    public class LoginQuery : IRequest<UserInformationDTO?>
    {
        public long Username { get; set; }
        public string Password { get; set; }
        public long InstitutionCode { get; set; }
    }
}
