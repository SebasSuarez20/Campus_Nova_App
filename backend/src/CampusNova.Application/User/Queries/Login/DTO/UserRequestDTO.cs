using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Application.User.Queries.Login.DTO
{
    public class UserRequestDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
