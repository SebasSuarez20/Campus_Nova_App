using CampusNova.Application.Common;
using CampusNova.Application.Common.Enum;
using CampusNova.Domain.Exceptions;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Text.Json;

namespace CampusNova.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
            => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (SqlException sqlEx)
            {
                await HandleSqlExceptionAsync(context, sqlEx);
            }
            catch (TransactionRollbackException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleSqlExceptionAsync(HttpContext context, SqlException ex)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, message) = ex.Number switch
            {
                2627 or 2601 => (400, "Ya existe un registro con ese valor único"),
                547 => (400, "No tiene valores relacionados."),
                515 => (400, "Campo requerido no puede ser nulo"),
                8152 or 2628 => (400, "El valor excede la longitud permitida del campo"),
                _ => (500, "Tiene un Error en la base de datos.")
            };

            context.Response.StatusCode = statusCode;

            var response = new
            {
                status = ResponseStatus.Failed,
                message = message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                status = ResponseStatus.Failed,
                message = ex.Message,
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
