using CampusNova.Domain.Subject.Model;
using MediatR;

namespace CampusNova.Application.Subject.Queries.ConsultStudentClasses
{
    public class ConsultStudentClassesQuery : IRequest<StudentClassesResultRecord>
    {
    }
}
