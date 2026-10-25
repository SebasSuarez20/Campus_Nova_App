using CampusNova.Application.User.Queries.Login;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Application.Auth.Queries.RefreshToken
{
    public class RefreshValidator : AbstractValidator<RefreshQuery>
    {
        public RefreshValidator() {

            RuleFor(x => x.RefreshToken)
               .NotEmpty().WithMessage("El refresh token es obligatorio");
        }

    }
}
