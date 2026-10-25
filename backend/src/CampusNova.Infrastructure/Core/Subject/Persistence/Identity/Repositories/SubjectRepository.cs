using CampusNova.Application.Common.Interfaces;
using CampusNova.Domain.Common.Interfaces;
using CampusNova.Domain.Subject.Interfaces;
using CampusNova.Domain.Subject.Model;
using Dapper;
using System.Data;

namespace CampusNova.Infrastructure.Core.Subject.Persistence.Identity.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserServices _currentUserServices;

        public SubjectRepository(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _currentUserServices = currentUserServices;
        }

        private IDbConnection Connection => _unitOfWork.Connection;

        public async Task InsertStudentSubject(string subjectCode, CancellationToken ct = default)
        {
            var command = new CommandDefinition(
                "syn_sp_InsertStudentSubject",
                new
                {
                    InstitutionCode = long.Parse(_currentUserServices.ContextInstitution()),
                    StudentIdentification = long.Parse(_currentUserServices.ContextUser()),
                    SubjectCode = subjectCode
                },
                transaction: _unitOfWork.Transaction,
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct);

            await Connection.ExecuteAsync(command);
        }

        public async Task DeleteStudentSubject(string subjectCode, CancellationToken ct = default)
        {
            var command = new CommandDefinition(
                "syn_sp_DeleteStudentSubject",
                new
                {
                    InstitutionCode = long.Parse(_currentUserServices.ContextInstitution()),
                    StudentIdentification = long.Parse(_currentUserServices.ContextUser()),
                    SubjectCode = subjectCode
                },
                transaction: _unitOfWork.Transaction,
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct);

            await Connection.ExecuteAsync(command);
        }

        public async Task<StudentClassesResultRecord> GetStudentClasses(CancellationToken ct = default)
        {
            var command = new CommandDefinition(
                "syn_sp_GetStudentClasses",
                new
                {
                    InstitutionId = long.Parse(_currentUserServices.ContextInstitution()),
                    StudentIdentification = long.Parse(_currentUserServices.ContextUser())
                },
                transaction: _unitOfWork.Transaction,
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct);

            using var multi = await Connection.QueryMultipleAsync(command);

            var subjects = (await multi.ReadAsync<StudentClassRecord>()).ToList();
            var students = (await multi.ReadAsync<ClassmateRecord>()).ToList();

            return new StudentClassesResultRecord
            {
                Subjects = subjects,
                Students = students
            };
        }
    }
}
