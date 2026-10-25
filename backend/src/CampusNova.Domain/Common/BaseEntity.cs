using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Domain.Common
{
    public abstract class BaseEntity
    {
        public long Institution_Code  { get; set; }
        public int Id { get;  set; }
        public bool Enabled { get; set; } = true;
        public DateTime AuditDate { get; private set; }
        public string CreatedBy { get;  set; }
    }

}
