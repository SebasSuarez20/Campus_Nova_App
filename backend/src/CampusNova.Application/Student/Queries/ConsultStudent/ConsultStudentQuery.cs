using CampusNova.Domain.Student.Record;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Application.Student.Queries.ConsultStudent
{
    public class ConsultStudentQuery : IRequest<IEnumerable<GetStudentRecord>>
    {
        public long Identification { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
