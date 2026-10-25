using CampusNova.Domain.Student.Record;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Application.Student.Command.UpdateStudent
{
    public class UpdateStudentCommand : IRequest<String>
    {
        public UpdateStudentRecord Student { get; set; }
    }
}
