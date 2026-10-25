using Dapper;
using CampusNova.Application.Common.Interfaces;
using CampusNova.Domain.Common.Interfaces;
using CampusNova.Domain.Student.Interfaces;
using CampusNova.Domain.Student.Record;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Infrastructure.Core.Student.Persistence.Identity.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserServices _currentUserServices;

        public StudentRepository(IUnitOfWork unitOfWork,ICurrentUserServices currentUserServices) {
        
            _unitOfWork = unitOfWork;
            _currentUserServices = currentUserServices;
        }

        private IDbConnection Connection => _unitOfWork.Connection;



        public async Task<IEnumerable<GetStudentRecord>> GetStudent()
        {
            var command = new CommandDefinition(
                "syn_sp_GetStudents",
                new
                {
                    InstitutionId = long.Parse(_currentUserServices.ContextInstitution()),
                    StudentIdentification = long.Parse(_currentUserServices.ContextUser())
                },
                transaction: _unitOfWork.Transaction,
                commandType: CommandType.StoredProcedure);

            return await Connection.QueryAsync<GetStudentRecord>(command);
        }

        public async Task<int> CreateStudent(Domain.Student.Model.Student model, long institutionCode, string passwordHash, CancellationToken ct = default)
        {
            var command = new CommandDefinition(
                "syn_sp_InsertStudent",
                new
                {
                    InstitutionCode = institutionCode,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Identification = model.Identification,
                    PasswordHash = passwordHash
                },
                transaction: _unitOfWork.Transaction,
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct);

            await Connection.ExecuteAsync(command);
            return 1;
        }

        public async Task<bool> UpdateStudent(Domain.Student.Model.Student model, string? passwordHash, CancellationToken ct = default)
        {
            var command = new CommandDefinition(
                "syn_sp_UpdateStudent",
                new
                {
                    InstitutionId = long.Parse(_currentUserServices.ContextInstitution()),
                    StudentIdentification = long.Parse(_currentUserServices.ContextUser()),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PasswordHash = passwordHash
                },
                transaction: _unitOfWork.Transaction,
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct);

            await Connection.ExecuteAsync(command);
            return true;
        }

        public async Task<bool> DeleteStudent(CancellationToken ct = default)
        {
            var command = new CommandDefinition(
                "syn_sp_DeleteStudent",
                new
                {
                    InstitutionId = long.Parse(_currentUserServices.ContextInstitution()),
                    StudentIdentification = long.Parse(_currentUserServices.ContextUser())
                },
                transaction: _unitOfWork.Transaction,
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct);

            await Connection.ExecuteAsync(command);
            return true;
        }

       
    }
}
