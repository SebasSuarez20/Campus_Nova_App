using FluentValidation;

namespace CampusNova.Application.Common.Validators
{
    public static class PasswordRules
    {
        /// <summary>
        /// Contraseña fuerte: mínimo 8 caracteres, mayúscula, minúscula, número,
        /// un símbolo y sin espacios. La misma regla se valida en el frontend.
        /// </summary>
        public static IRuleBuilderOptions<T, string> MustBeStrongPassword<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .MinimumLength(8).WithMessage("La contraseña debe tener mínimo 8 caracteres")
                .MaximumLength(64).WithMessage("La contraseña no puede superar 64 caracteres")
                .Matches("[A-Z]").WithMessage("La contraseña debe tener al menos una mayúscula")
                .Matches("[a-z]").WithMessage("La contraseña debe tener al menos una minúscula")
                .Matches("[0-9]").WithMessage("La contraseña debe tener al menos un número")
                .Matches(@"[^a-zA-Z0-9\s]").WithMessage("La contraseña debe tener al menos un símbolo")
                .Must(p => !p.Any(char.IsWhiteSpace)).WithMessage("La contraseña no puede tener espacios");
        }
    }
}
