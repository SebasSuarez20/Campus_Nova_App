using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Application.Common.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse>
         : IPipelineBehavior<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
            => _validators = validators;

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            if (!_validators.Any()) return await next();

            var context = new ValidationContext<TRequest>(request);
            var errors = _validators
                .SelectMany(v => v.Validate(context).Errors)
                .Where(e => e != null)
                .Select(e => e.ErrorMessage)
                .ToList();

            if (errors.Any())
                throw new ValidationException(string.Join(", ", errors));

            return await next();
        }
    }
}
