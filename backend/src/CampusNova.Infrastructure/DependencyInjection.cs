using CampusNova.Application.Auth.Queries.RefreshToken;
using CampusNova.Application.Common.Interfaces;
using CampusNova.Application.Student.Command.CreateStudent;
using CampusNova.Application.Student.Command.DeleteStudent;
using CampusNova.Application.Student.Command.UpdateStudent;
using CampusNova.Application.User.Queries.Login;
using CampusNova.Domain.Common.Interfaces;
using CampusNova.Domain.Student.Interfaces;
using CampusNova.Domain.User.Interfaces;
using CampusNova.Infrastructure.Core;
using CampusNova.Infrastructure.Core.Student.Persistence.Identity.Repositories;
using CampusNova.Infrastructure.Core.User.Persistence.Identity.Repositories;
using CampusNova.Infrastructure.Core.User.Services;
using CampusNova.Infrastructure.Services;
// EF removed - Dapper will be used for data access
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CampusNova.Infrastructure
{
    // Infrastructure/DependencyInjection.cs
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration config)
        {

            services.AddHttpContextAccessor();

            // Dapper con SQL Server
            services.AddDapper(config);


            #region inyeccion de dependencias
            // Los handlers los registra MediatR (AddApplication); IUnitOfWork e IDbConnection los registra AddDapper.
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ICurrentUserServices, CurrentUserService>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<CampusNova.Domain.Professor.Interfaces.IProfessorRepository, CampusNova.Infrastructure.Core.Professor.Persistence.Identity.Repositories.ProfessorRepository>();
            services.AddScoped<CampusNova.Domain.Subject.Interfaces.ISubjectRepository, CampusNova.Infrastructure.Core.Subject.Persistence.Identity.Repositories.SubjectRepository>();

            #endregion

            return services;
        }
    }
}
