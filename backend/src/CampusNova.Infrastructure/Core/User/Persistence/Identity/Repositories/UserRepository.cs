using Dapper;
using CampusNova.Application.Common.Interfaces;
using CampusNova.Domain.Common.Interfaces;
using CampusNova.Domain.Common.Services;
using CampusNova.Domain.Student.Model;
using CampusNova.Domain.User.Interfaces;
using CampusNova.Domain.User.Model;
using CampusNova.Domain.User.Model.ReadModel;
using CampusNova.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace CampusNova.Infrastructure.Core.User.Persistence.Identity.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserServices _currentUser;

    public UserRepository(IUnitOfWork unitOfWork, ICurrentUserServices currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    private IDbConnection Connection => _unitOfWork.Connection;

    public async Task<LoginAuthView?> GetAcessToSystem(long username, string password, long institutionCode, CancellationToken ct = default)
    {
        const string sp = "syn_sp_GetAccessToSystem";

        var command = new CommandDefinition(
            sp,
            new { Username = username, InstitutionCode = institutionCode, Enabled = true },
            transaction: _unitOfWork.Transaction,
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct);

        return await Connection.QueryFirstOrDefaultAsync<LoginAuthView>(command);
    }

    public async Task<LoginAuthView?> GetUserForId(long id, long institutionCode, CancellationToken ct = default)
    {
            const string sql = @"SELECT * FROM syn_GetForUserIdView u
                               WHERE u.Identification = @Id
                                 AND u.Institution_Code    = @InstitutionCode";


            var resp = await Connection.QueryFirstOrDefaultAsync<LoginAuthView>(sql, new
            {
                Id = id,
                InstitutionCode = institutionCode
            }, transaction: _unitOfWork.Transaction);

            return resp;
    }
}

