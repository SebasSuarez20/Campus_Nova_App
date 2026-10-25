using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Domain.User.Model.ReadModel
{
    public class LoginAuthView
    {
        public int Id { get; set; }
        public long? Identification { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Rol { get; set; }
        public long? Institution_Code { get; set; }
        public string? Email { get; set; }

    }

}
