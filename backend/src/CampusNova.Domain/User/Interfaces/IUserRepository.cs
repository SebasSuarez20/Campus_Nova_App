using CampusNova.Domain;
using CampusNova.Domain.Student.Model;
using CampusNova.Domain.User.Model;
using CampusNova.Domain.User.Model.ReadModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Domain.User.Interfaces
{
    public interface IUserRepository
    {
        public Task<LoginAuthView?> GetAcessToSystem(long username, string password, long institutionCode, CancellationToken ct = default);
        public Task<LoginAuthView?> GetUserForId(long id, long institutionCode, CancellationToken ct = default);
      
    }
}
