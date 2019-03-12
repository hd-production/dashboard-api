using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HdProduction.Dashboard.Infrastructure.Validation
{
  public interface IValidator
  {
    Task CheckAsync<T>(T model, CancellationToken cancellationToken);
  }

  public abstract class Validator<T> : IValidator where T : class
  {
    private readonly List<IValidationRule> _validations;
    private T _model;

    protected Validator()
    {
      _validations = new List<IValidationRule>();
    }

    public virtual async Task CheckAsync(T model, CancellationToken cancellationToken)
    {
      _model = model;
      await SetValidations();
      foreach (var validation in _validations)
      {
        await validation.CheckAsync(cancellationToken);
      }
    }

    protected abstract Task SetValidations();

    protected ValidationRule<T, TProperty> RuleFor<TProperty>(Func<T, TProperty> expression)
    {
      var rule = new ValidationRule<T, TProperty>(expression(_model), _model);
      _validations.Add(rule);
      return rule;
    }

    Task IValidator.CheckAsync<T1>(T1 model, CancellationToken cancellationToken)
    {
      if (!(model is T tModel))
      {
        throw new ArgumentException("Wrong type of model");
      }

      return CheckAsync(tModel, cancellationToken);
    }
  }
}