using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatR.Pipeline;

namespace HdProduction.Dashboard.Infrastructure.Validation
{
  public abstract class ValidatorR<T, TR> : IRequestPreProcessor<T> where T : IRequest<TR>
  {
    private readonly List<IValidationRule> _validations;
    private T _request;

    protected ValidatorR()
    {
      _validations = new List<IValidationRule>();
    }

    public async Task Process(T request, CancellationToken cancellationToken)
    {
      _request = request;
      await SetValidations();
      await Task.WhenAll(_validations.Select(v => v.CheckAsync(cancellationToken)).ToArray());
    }

    protected abstract Task SetValidations();

    protected ValidationRule<T, TProperty> RuleFor<TProperty>(Func<T, TProperty> expression)
    {
      var rule = new ValidationRule<T, TProperty>(expression(_request), _request);
      _validations.Add(rule);
      return rule;
    }
  }
}