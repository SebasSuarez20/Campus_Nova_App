using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Dapper;
using CampusNova.Domain.Common.Interfaces;
using CampusNova.Infrastructure.Core;

namespace CampusNova.Infrastructure;

/// <summary>
/// Configura Dapper con SQL Server.
/// SqlClient administra internamente el pool de conexiones a partir de la cadena
/// de conexión, por lo que basta con crear una SqlConnection por petición.
/// A diferencia de PostgreSQL, SQL Server no distingue mayúsculas de minúsculas
/// en los identificadores, así que las queries no requieren comillas ni casts.
/// </summary>
public static class DapperConfig
{
    public static IServiceCollection AddDapper(this IServiceCollection services, IConfiguration config)
    {
        var connStr = config.GetConnectionString("CampusNova")
            ?? throw new InvalidOperationException("La cadena de conexión no está configurada.");

        // IDbConnection por petición: Dapper la usa para ejecutar las queries.
        // El contenedor se encarga de hacer Dispose al terminar el scope.
        services.AddScoped<IDbConnection>(_ => new SqlConnection(connStr));
        services.AddScoped<IConnectionFactory, ConnectionFactory>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        DefaultTypeMap.MatchNamesWithUnderscores = true;

        return services;
    }
}

public interface IConnectionFactory
{
    IDbConnection CreateConnection();
}

public class ConnectionFactory : IConnectionFactory
{
    private readonly IDbConnection _connection;
    public ConnectionFactory(IDbConnection connection) => _connection = connection;
    public IDbConnection CreateConnection() => _connection;
}
