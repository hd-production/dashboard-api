using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Exceptions;

namespace HdProduction.Dashboard.Infrastructure.Validation
{
  public interface IValidationRule
  {
    Task CheckAsync(CancellationToken cancellationToken);
  }

  public class ValidationRule<T, TProperty> : IValidationRule
  {
    private readonly T _request;
    private readonly TProperty _property;
    private readonly List<Func<TProperty, Task<bool>>> _simpleChecks;
    private readonly List<Func<TProperty, T, Task<bool>>> _compositeChecks;

    private bool _haveToCheck;
    private string _message;

    public ValidationRule(TProperty property, T request)
    {
      _property = property;
      _request = request;
      _simpleChecks = new List<Func<TProperty, Task<bool>>>();
      _compositeChecks = new List<Func<TProperty, T, Task<bool>>>();
      _message = $"Value {_property} is not allowed";
      _haveToCheck = true;
    }

    public ValidationRule<T, TProperty> Must(Func<TProperty, bool> check)
    {
      _simpleChecks.Add(x => Task.FromResult(check(x)));
      return this;
    }

    public ValidationRule<T, TProperty> Must(Func<TProperty, Task<bool>> check)
    {
      _simpleChecks.Add(check);
      return this;
    }

    public ValidationRule<T, TProperty> Must(Func<TProperty, T, bool> check)
    {
      _compositeChecks.Add((prop, req) => Task.FromResult(check(prop, req)));
      return this;
    }

    public ValidationRule<T, TProperty> Must(Func<TProperty, T, Task<bool>> check)
    {
      _compositeChecks.Add(check);
      return this;
    }

    public ValidationRule<T, TProperty> When(Func<T, bool> whenCondition)
    {
      _haveToCheck = whenCondition(_request);
      return this;
    }

    public ValidationRule<T, TProperty> WithMessage(string message)
    {
      _message = message;
      return this;
    }

    public async Task CheckAsync(CancellationToken cancellationToken)
    {
      if (!_haveToCheck)
      {
        return;
      }
      bool isValid = true;
      foreach (var check in _simpleChecks)
      {
        if (cancellationToken.IsCancellationRequested)
        {
          break;
        }
        isValid = isValid && await check(_property);
      }
      foreach (var check in _compositeChecks)
      {
        if (cancellationToken.IsCancellationRequested)
        {
          break;
        }
        isValid = isValid && await check(_property, _request);
      }

      if (!isValid)
      {
        throw new BusinessLogicException(_message);
      }
    }
  }
}