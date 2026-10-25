using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Application.User.Queries.Login
{
    public class LoginValidator : AbstractValidator<LoginQuery>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("El username es obligatorio");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("El password es obligatorio");

            RuleFor(x => x.InstitutionCode)
                .NotEmpty().WithMessage("La compañia es obligatorio");
        }
    }
}
