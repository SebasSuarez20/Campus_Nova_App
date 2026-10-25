using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Application.Subject.Command.DeleteSubject
{
    public class DeleteSubjectCommand : IRequest<string>
    {
        public string SubjectCode { get; set; } = string.Empty;

    }
}
