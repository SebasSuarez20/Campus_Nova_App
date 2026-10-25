using CampusNova.Domain.Subject.Interfaces;
using CampusNova.Domain.Subject.Model;
using MediatR;

namespace CampusNova.Application.Subject.Queries.ConsultStudentClasses
{
    public class ConsultStudentClassesHandler : IRequestHandler<ConsultStudentClassesQuery, StudentClassesResultRecord>
    {
        private readonly ISubjectRepository _repository;

        public ConsultStudentClassesHandler(ISubjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<StudentClassesResultRecord> Handle(ConsultStudentClassesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetStudentClasses(cancellationToken);
        }
    }
}
