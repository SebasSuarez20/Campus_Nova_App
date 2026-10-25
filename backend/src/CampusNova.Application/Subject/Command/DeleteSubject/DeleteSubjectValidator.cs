using CampusNova.Application.Subject.Command.EnrollSubject;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Application.Subject.Command.DeleteSubject
{
    public class DeleteSubjectValidator : AbstractValidator<DeleteSubjectCommand>
    {
        public DeleteSubjectValidator() {
            RuleFor(x => x.SubjectCode)
                    .NotEmpty().WithMessage("El código de la materia es obligatorio")
                    .MaximumLength(10).WithMessage("Máximo 10 caracteres");
        }
    }
}
