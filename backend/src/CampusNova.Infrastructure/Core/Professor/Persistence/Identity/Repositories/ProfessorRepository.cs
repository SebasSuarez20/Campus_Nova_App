using CampusNova.Application.Common.Interfaces;
using CampusNova.Domain.Common.Interfaces;
using CampusNova.Domain.Professor.Interfaces;
using CampusNova.Domain.Professor.Record;
using Dapper;
using System.Data;

namespace CampusNova.Infrastructure.Core.Professor.Persistence.Identity.Repositories
{
    public class ProfessorRepository : IProfessorRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserServices _currentUserServices;

        public ProfessorRepository(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _currentUserServices = currentUserServices;
        }

        private IDbConnection Connection => _unitOfWork.Connection;

        public async Task<SubjectsResultRecord> GetSubjects(CancellationToken ct = default)
        {
            var command = new CommandDefinition(
                "syn_sp_GetSubjects",
                new
                {
                    InstitutionId = long.Parse(_currentUserServices.ContextInstitution()),
                    StudentIdentification = long.Parse(_currentUserServices.ContextUser())
                },
                transaction: _unitOfWork.Transaction,
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct);

            using var multi = await Connection.QueryMultipleAsync(command);

            var subjects = (await multi.ReadAsync<SubjectRecord>()).ToList();
            var professors = (await multi.ReadAsync<ProfessorRecord>()).ToList();

            return new SubjectsResultRecord
            {
                Subjects = subjects,
                Professors = professors
            };
        }
    }
}
