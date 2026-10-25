using FluentValidation;

namespace CampusNova.Application.Subject.Command.EnrollSubject
{
    public class EnrollSubjectValidator : AbstractValidator<EnrollSubjectCommand>
    {
        public EnrollSubjectValidator()
        {
            RuleFor(x => x.SubjectCode)
                .NotEmpty().WithMessage("El código de la materia es obligatorio")
                .MaximumLength(10).WithMessage("Máximo 10 caracteres");
        }
    }
}
