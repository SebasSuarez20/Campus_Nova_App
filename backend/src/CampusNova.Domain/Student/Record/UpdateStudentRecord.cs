using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Domain.Student.Record
{
    public record UpdateStudentRecord(string FirstName, string LastName, string Email, long Identification,string? Password);
}
