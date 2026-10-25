using CampusNova.Application.Common.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Application.Student.Command.UpdateStudent
{
    public class UpdateStudentValidator : AbstractValidator<UpdateStudentCommand>
    {

        public UpdateStudentValidator() {

            ///<sumary>
            ///Se valida primero los campos de empleado.
            /// </sumary>

            RuleFor(x => x.Student.FirstName)
               .NotEmpty().WithMessage("En el modelo de empleado el nombre es obligatorio")
               .MaximumLength(100).WithMessage("Máximo 100 caracteres");

            RuleFor(x => x.Student.LastName)
               .NotEmpty().WithMessage("En el modelo de empleado el Apellido es obligatorio")
               .MaximumLength(100).WithMessage("Máximo 100 caracteres");

            RuleFor(x => x.Student.Email)
                .EmailAddress().WithMessage("En el modelo de empleado el Correo no autorizado")
               .NotEmpty().WithMessage("En el modelo de empleado el nombre es obligatorio")
               .MaximumLength(120).WithMessage("Máximo 100 caracteres");

            RuleFor(x => x.Student.Email).
                NotEmpty().WithMessage("En el modelo de empleado el Identificación obligatoria");

            RuleFor(x => x.Student.Password!)
                .MustBeStrongPassword()
                .When(x => !string.IsNullOrWhiteSpace(x.Student.Password));
        }

    }
}
