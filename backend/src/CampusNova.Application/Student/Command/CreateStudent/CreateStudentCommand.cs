using CampusNova.Domain.Student.Record;
using MediatR;

namespace CampusNova.Application.Student.Command.CreateStudent
{
    public class CreateStudentCommand : IRequest<string>
    {
        public CreateStudentData Student { get; set; } = default!;
        public long InstitutionCode { get; set; }
    }
}
