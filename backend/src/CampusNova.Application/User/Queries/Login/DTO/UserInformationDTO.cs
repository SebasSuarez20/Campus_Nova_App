using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Application.User.Queries.Login.DTO
{
    public class UserInformationDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? Identification { get; set; }
        public long InstitutionCode { get; set; }
        public string Token { get; set; }

        public string? Rol { get; set; }
        public string RefreshToken { get; set; }
    }
}
