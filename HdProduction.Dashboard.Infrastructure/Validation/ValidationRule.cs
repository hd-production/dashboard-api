using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Exceptions;
using Microsoft.EntityFrameworkCore.Internal;

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

    private IValidator _childValidator;
    private bool _haveToCheck;
    private string _message;
    private bool _throwsNotFound;

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

    public ValidationRule<T, TProperty> ThrowsNotFound()
    {
      _throwsNotFound = true;
      return this;
    }

    public ValidationRule<T, TProperty> SetModelValidator(IValidator validator)
    {
      _childValidator = validator;
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
        if (_throwsNotFound)
        {
          throw new EntityNotFoundException(_message);
        }
        throw new ValidationException(_message);
      }

      if (_childValidator != null)
      {
        await _childValidator.CheckAsync(_property, cancellationToken);
      }
    }

    #region Basic must

    public ValidationRule<T, TProperty> Equal(TProperty val) => Must(p => p.Equals(val));
    
    public ValidationRule<T, TProperty> NotEqual(TProperty val) => Must(p => !p.Equals(val));
    
    public ValidationRule<T, TProperty> GreaterThan(TProperty val) =>
      Must(p => p is IComparable comparable && comparable.CompareTo(val) > 0);
    
    public ValidationRule<T, TProperty> LessThan(TProperty val) =>
      Must(p => p is IComparable comparable && comparable.CompareTo(val) < 0);
    
    public ValidationRule<T, TProperty> GreaterOrEqual(TProperty val) =>
      Must(p => p is IComparable comparable && comparable.CompareTo(val) >= 0);
    
    public ValidationRule<T, TProperty> LessOrEqual(TProperty val) =>
      Must(p => p is IComparable comparable && comparable.CompareTo(val) <= 0);
    
    public ValidationRule<T, TProperty> Null() => Must(p => p == null);

    public ValidationRule<T, TProperty> NotNull() => Must(p => p != null);
    
    public ValidationRule<T, TProperty> Empty() =>
      Must(p => p == null || p.Equals(default)
          || p is string str && string.IsNullOrWhiteSpace(str)
          || p is IEnumerable enumerable && enumerable.Any());

    public ValidationRule<T, TProperty> NotEmpty() =>
      Must(p => p != null && !p.Equals(default)
        && (p is string str && string.IsNullOrWhiteSpace(str) || p is IEnumerable enumerable && enumerable.Any()));
    
    public ValidationRule<T, TProperty> ValidEnum() => Must(p => Enum.IsDefined(typeof(TProperty), p));

    #endregion
  }
}