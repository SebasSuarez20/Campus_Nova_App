using MediatR;

namespace CampusNova.Application.Subject.Command.EnrollSubject
{
    public class EnrollSubjectCommand : IRequest<string>
    {
        public string SubjectCode { get; set; } = string.Empty;
    }
}
