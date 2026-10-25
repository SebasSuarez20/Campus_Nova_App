using CampusNova.Domain.Student.Interfaces;
using CampusNova.Domain.Student.Record;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Application.Student.Queries.ConsultStudent
{
    public class ConsultStudentHandler : IRequestHandler<ConsultStudentQuery, IEnumerable<GetStudentRecord>?>
    {
        private readonly IStudentRepository _repository;

        public ConsultStudentHandler(IStudentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<GetStudentRecord>?> Handle(ConsultStudentQuery request, CancellationToken cancellationToken)
        {
            var resp = await _repository.GetStudent();
            return resp;
        }
    }
}
