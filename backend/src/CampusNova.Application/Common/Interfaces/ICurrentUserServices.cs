using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Application.Common.Interfaces
{
    public interface ICurrentUserServices
    {

        public string ContextUser();
        public string ContextInstitution();

    }
}
