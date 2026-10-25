using CampusNova.Application.Common.Validators;
using FluentValidation;

namespace CampusNova.Application.Student.Command.CreateStudent
{
    public class CreateStudentValidator : AbstractValidator<CreateStudentCommand>
    {
        public CreateStudentValidator()
        {
            RuleFor(x => x.InstitutionCode)
                .GreaterThan(0).WithMessage("El código institucional es obligatorio");

            RuleFor(x => x.Student.FirstName)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100).WithMessage("Máximo 100 caracteres");

            RuleFor(x => x.Student.LastName)
                .NotEmpty().WithMessage("El apellido es obligatorio")
                .MaximumLength(100).WithMessage("Máximo 100 caracteres");

            RuleFor(x => x.Student.Email)
                .NotEmpty().WithMessage("El correo es obligatorio")
                .EmailAddress().WithMessage("El correo no es válido")
                .MaximumLength(120).WithMessage("Máximo 120 caracteres");

            RuleFor(x => x.Student.Identification)
                .GreaterThan(0).WithMessage("La identificación tiene que ser mayor a cero");

            RuleFor(x => x.Student.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria")
                .MustBeStrongPassword();
        }
    }
}
