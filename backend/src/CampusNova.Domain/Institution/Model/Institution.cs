using CampusNova.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Domain.Institution.Model
{
    public class Institution : BaseEntity
    {
        public long Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Country { get; set; }
        public int City { get; set; }

    }
}
