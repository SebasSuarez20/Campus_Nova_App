using CampusNova.Domain.Professor.Interfaces;
using CampusNova.Domain.Professor.Record;
using MediatR;

namespace CampusNova.Application.Professor.Queries.ConsultProfessor
{
    public class ConsultProfessorHandler : IRequestHandler<ConsultProfessorQuery, SubjectsResultRecord>
    {
        private readonly IProfessorRepository _repository;

        public ConsultProfessorHandler(IProfessorRepository repository)
        {
            _repository = repository;
        }

        public async Task<SubjectsResultRecord> Handle(ConsultProfessorQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetSubjects(cancellationToken);
        }
    }
}
