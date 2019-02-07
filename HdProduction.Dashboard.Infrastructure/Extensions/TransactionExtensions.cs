using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace HdProduction.Dashboard.Infrastructure.Extensions
{
  public static class TransactionExtensions
  {
    public static async Task ExecuteSafeAsync(this IDbContextTransaction transaction, Func<Task> action)
    {
      try
      {
        await action();
      }
      catch (Exception)
      {
        transaction.Rollback();
        throw;
      }
      transaction.Commit();
    }
  }
}