using MediatR;

namespace CampusNova.Application.Student.Command.DeleteStudent
{
    /// <summary>Elimina el registro del estudiante que tiene la sesión abierta.</summary>
    public record DeleteStudentCommand() : IRequest<string>;
}
