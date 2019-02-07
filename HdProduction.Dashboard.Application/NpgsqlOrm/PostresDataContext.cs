using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;

namespace HdProduction.Dashboard.Application.NpgsqlOrm
{
  public class PostresDataContext : IDisposable
  {
    public const int ParameterAutoSize = 0;
    public const int MinNVarCharParamSize = 4000;
    public const int MinParamSize = 8000;
    public const int MaxParamSize = -1;

    private string _connectionString;
    private Action _onCommit;
    private Action _onRollback;

    #region Initialization

    public PostresDataContext(string connectionString)
    {
      try
      {
        _connectionString = connectionString;
        Command = new NpgsqlCommand();
      }
      catch (Exception)
      {
        Dispose();
        throw;
      }
    }

    #endregion

    public NpgsqlCommand Command { get; private set; }
    public NpgsqlConnection Connection { get; private set; }
    public NpgsqlTransaction Transaction { get; private set;  }

    #region Disposing

    /// <summary>
    /// PostresDataContext cannot be used after disposing (closing).
    /// </summary>
    public void Dispose()
    {
      Close();
    }

    /// <summary>
    /// PostresDataContext cannot be used after closing.
    /// </summary>
    public void Close()
    {
      if (Command != null)
      {
        Command.Dispose();
        Command = null;
      }

      if (Transaction != null)
      {
        Transaction.Dispose();
        Transaction = null;
        var onRollback = _onRollback;
        onRollback?.Invoke();
      }

      if (Connection != null)
      {
        Connection.Dispose();
        Connection = null;
      }
    }

    /// <summary>
    /// Resets PostresDataContext into initial state with same connection string.
    /// </summary>
    /// <returns></returns>
    public PostresDataContext Reset()
    {
      Close();
      Command = new NpgsqlCommand();
      return this;
    }

    /// <summary>
    /// Resets PostresDataContext into initial state.
    /// </summary>
    /// <returns></returns>
    public PostresDataContext Reset(string connectionString)
    {
      Reset();
      _connectionString = connectionString;
      return this;
    }

    #endregion

    #region Commands

    #region Sql and StoredProc

    public PostresDataContext Sql(StringBuilder sql) { return Sql(sql.ToString()); }
    public PostresDataContext Sql(string sql, object arg0) { return Sql(String.Format(sql, arg0)); }
    public PostresDataContext Sql(string sql, object arg0, object arg1) { return Sql(String.Format(sql, arg0, arg1)); }
    public PostresDataContext Sql(string sql, object arg0, object arg1, object arg2) { return Sql(String.Format(sql, arg0, arg1, arg2)); }
    public PostresDataContext Sql(string sql, params object[] args) { return Sql(String.Format(sql, args)); }

    public PostresDataContext Sql(string sql)
    {
      Command.CommandText = sql;
      Command.CommandType = CommandType.Text;
      return this;
    }

    public PostresDataContext StoredProc(string storedProcName)
    {
      Command.CommandText = storedProcName;
      Command.CommandType = CommandType.StoredProcedure;
      return this;
    }

    #endregion

    #region Parameters

    public PostresDataContext Set<T>(string name, T value) { return Set(name, TypeMappings.ToSqlDbType<T>(), value); }
    public PostresDataContext Set(string name, NpgsqlDbType dataType, object value) { SetParam(name, dataType, ParameterAutoSize, value); return this; }
    public PostresDataContext Set(string name, NpgsqlDbType dataType, int size, object value) { SetParam(name, dataType, size, value); return this; }

    public PostresDataContext Out<T>(string name) { return Out(name, TypeMappings.ToSqlDbType<T>()); }
    public PostresDataContext Out(string name, NpgsqlDbType dataType) { SetParam(name, dataType, ParameterDirection.Output); return this; }

    public PostresDataContext SetOut<T>(string name, T value) { return SetOut(name, TypeMappings.ToSqlDbType<T>(), value); }
    public PostresDataContext SetOut(string name, NpgsqlDbType dataType, object value) { SetParam(name, dataType, value, ParameterDirection.InputOutput); return this; }

    public PostresDataContext Return<T>(string name, T value) { return Return(name, TypeMappings.ToSqlDbType<T>(), value); }
    public PostresDataContext Return(string name, NpgsqlDbType dataType, object value) { SetParam(name, dataType, value, ParameterDirection.ReturnValue); return this; }

    private void SetParam(string name, NpgsqlDbType dbType, ParameterDirection direction)
    {
      Command.Parameters.Add(new NpgsqlParameter(name, dbType) {Direction = direction});
    }

    private void SetParam(string name, NpgsqlDbType dbType, object value, ParameterDirection direction)
    {
      Command.Parameters.Add(new NpgsqlParameter(name, dbType) {Value = value ?? DBNull.Value, Direction = direction});
    }

    private void SetParam(string name, NpgsqlDbType dbType, int size, object value)
    {
      Command.Parameters.Add(new NpgsqlParameter(name, dbType, size) {Value = value ?? DBNull.Value});
    }

    #endregion

    #region Options

    public PostresDataContext Timeout(int seconds = 30)
    {
      Command.CommandTimeout = seconds;
      return this;
    }

    #endregion

    #region Sql In, Sql Not In

    public string SqlIn<T>(string field, IEnumerable<T> values)
    {
      return SqlIn(field, field, values);
    }

    public string SqlIn<T>(string field, string param, IEnumerable<T> values)
    {
      return SqlIn(field, param, null, TypeMappings.ToSqlDbType<T>(), values);
    }

    public string SqlIn<T>(string field, string param, string alias, IEnumerable<T> values)
    {
      return SqlIn(field, param, alias, TypeMappings.ToSqlDbType<T>(), values);
    }

    public string SqlIn<T>(string field, string param, string alias, NpgsqlDbType dbType, IEnumerable<T> values)
    {
      return SqlInInternal(field, param, alias, dbType, values, false);
    }

    public string SqlNotIn<T>(string field, IEnumerable<T> values)
    {
      return SqlNotIn(field, field, values);
    }

    public string SqlNotIn<T>(string field, string param, IEnumerable<T> values)
    {
      return SqlNotIn(field, param, null, TypeMappings.ToSqlDbType<T>(), values);
    }

    public string SqlNotIn<T>(string field, string param, string alias, IEnumerable<T> values)
    {
      return SqlNotIn(field, param, alias, TypeMappings.ToSqlDbType<T>(), values);
    }

    public string SqlNotIn<T>(string field, string param, string alias, NpgsqlDbType dbType, IEnumerable<T> values)
    {
      return SqlInInternal(field, param, alias, dbType, values, true);
    }

    #endregion

    #region Sql Union Select

    public string SqlUnionSelect<T>(string field, IEnumerable<T> values, bool unionAll = false)
    {
      return SqlUnionSelect(field, field, values, unionAll);
    }

    public string SqlUnionSelect<T>(string field, string param, IEnumerable<T> values, bool unionAll = false)
    {
      return SqlUnionSelect(field, param, TypeMappings.ToSqlDbType<T>(), values, unionAll);
    }

    public string SqlUnionSelect<T>(string field, string param, NpgsqlDbType dataType, IEnumerable<T> values, bool unionAll)
    {
      var enumerator = values?.GetEnumerator();

      if (enumerator == null || !enumerator.MoveNext())
        return String.Empty;

      param = "@" + param;

      int i = 0;
      int count = 0;
      do
      {
        Set(param + i, dataType, enumerator.Current);
        i++;
        count++;
      } while (enumerator.MoveNext());


      var sb = new StringBuilder();
      sb.AppendFormat("SELECT ({0}0)", param);
      for (int j = 0; j < count; j++)
      {
        sb.AppendFormat(" UNION{0} SELECT({1}{2})", unionAll ? " ALL " : String.Empty, param, j);
      }

      return sb.ToString();
    }

    #endregion

    #region Execution

    public int Execute()
    {
      return ExecuteSafe(() => Command.ExecuteNonQuery());
    }

    public async Task<int> ExecuteAsync()
    {
      return await ExecuteAsync(CancellationToken.None);
    }

    public async Task<int> ExecuteAsync(CancellationToken cancellationToken)
    {
      return await ExecuteSafeAsync(() => Command.ExecuteNonQueryAsync(cancellationToken), cancellationToken);
    }

    public NpgsqlDataReader ExecuteReader(CommandBehavior commandBehavior = CommandBehavior.Default)
    {
      return ExecuteSafe(() => Command.ExecuteReader(commandBehavior));
    }

    #endregion

    #region Reading

    #region ReadOut

    public T ReadOut<T>(string outParamName)
    {
      return ExecuteSafe(() =>
      {
        Out<T>(outParamName);
        Command.ExecuteNonQuery();
        return (T)Command.Parameters[outParamName].Value;
      });
    }

    public T ReadOutOrDefault<T>(string outParamName, T defaultValue = default(T))
    {
      return ExecuteSafe(() =>
      {
        Out<T>(outParamName);
        Command.ExecuteNonQuery();
        var value = Command.Parameters[outParamName].Value;
        return value == null || DBNull.Value.Equals(value) ? defaultValue : (T)value;
      });
    }

    public async Task<T> ReadOutAsync<T>(string outParamName)
    {
      return await ReadOutAsync<T>(outParamName, CancellationToken.None);
    }

    public async Task<T> ReadOutAsync<T>(string outParamName, CancellationToken cancellationToken)
    {
      return await ExecuteSafeAsync(async () =>
      {
        Out<T>(outParamName);
        await Command.ExecuteNonQueryAsync(cancellationToken);
        return (T)Command.Parameters[outParamName].Value;
      }, cancellationToken);
    }

    public async Task<T> ReadOutOrDefaultAsync<T>(string outParamName, T defaultValue = default(T))
    {
      return await ReadOutOrDefaultAsync(outParamName, CancellationToken.None, defaultValue);
    }

    public async Task<T> ReadOutOrDefaultAsync<T>(string outParamName, CancellationToken cancellationToken, T defaultValue = default(T))
    {
      return await ExecuteSafeAsync(async () =>
      {
        Out<T>(outParamName);
        await Command.ExecuteNonQueryAsync(cancellationToken);
        var value = Command.Parameters[outParamName].Value;
        return value == null || DBNull.Value.Equals(value) ? defaultValue : (T)value;
      }, cancellationToken);
    }

    #endregion

    #region Read

    public T Read<T>()
    {
      return ExecuteSafe(() => (T)Command.ExecuteScalar());
    }

    public async Task<T> ReadAsync<T>()
    {
      return await ReadAsync<T>(CancellationToken.None);
    }

    public async Task<T> ReadAsync<T>(CancellationToken cancellationToken)
    {
      return await ExecuteSafeAsync(async () => (T)await Command.ExecuteScalarAsync(cancellationToken), cancellationToken);
    }

    public T ReadOrDefault<T>(T defaultValue = default(T))
    {
      return ExecuteSafe(() =>
      {
        var value = Command.ExecuteScalar();
        return value == null || DBNull.Value.Equals(value) ? defaultValue : (T)value;
      });
    }

    public async Task<T> ReadOrDefaultAsync<T>(T defaultValue = default(T))
    {
      return await ReadOrDefaultAsync(CancellationToken.None, defaultValue);
    }

    public async Task<T> ReadOrDefaultAsync<T>(CancellationToken cancellationToken, T defaultValue = default(T))
    {
      return await ExecuteSafeAsync(async () =>
      {
        var value = await Command.ExecuteScalarAsync(cancellationToken);
        return value == null || DBNull.Value.Equals(value) ? defaultValue : (T)value;
      }, cancellationToken);
    }

    public T ReadOrDefault<T>(Func<DataRecord, T> readItem)
    {
      return ExecuteSafe(() =>
      {
        var record = new DataRecord();
        using (var reader = Command.ExecuteReader(CommandBehavior.SingleRow))
        {
          if (!record.Read(reader))
            return default(T);
        }

        return readItem(record);
      });
    }

    public async Task<T> ReadOrDefaultAsync<T>(Func<DataRecord, T> readItem)
    {
      return await ReadOrDefaultAsync(readItem, CancellationToken.None);
    }

    public async Task<T> ReadOrDefaultAsync<T>(Func<DataRecord, T> readItem, CancellationToken cancellationToken)
    {
      return await ExecuteSafeAsync(async () =>
      {
        var record = new DataRecord();
        using (var reader = Command.ExecuteReader(CommandBehavior.SingleRow))
        {
          if (!await record.ReadAsync(reader, cancellationToken))
            return default(T);
        }
        return readItem(record);
      }, cancellationToken);
    }

    public DataRecord ReadRecord()
    {
      return ExecuteSafe(() =>
      {
        var record = new DataRecord();
        using (var reader = Command.ExecuteReader(CommandBehavior.SingleRow))
          if (!record.Read(reader))
            return null;
        return record;
      });
    }

    public async Task<DataRecord> ReadRecordAsync()
    {
      return await ReadRecordAsync(CancellationToken.None);
    }

    public async Task<DataRecord> ReadRecordAsync(CancellationToken cancellationToken)
    {
      return await ExecuteSafeAsync(async () =>
      {
        var record = new DataRecord();
        using (var reader = Command.ExecuteReader(CommandBehavior.SingleRow))
          if (!await record.ReadAsync(reader, cancellationToken))
            return null;

        return record;
      }, cancellationToken);
    }

    #endregion

    #region ReadAll

    public List<T> ReadAll<T>(Func<DataRecord, T> readItem)
    {
      return ReadAll<List<T>, T>(readItem);
    }

    public HashSet<T> ReadAllHashSet<T>(Func<DataRecord, T> readItem)
    {
      return ReadAll<HashSet<T>, T>(readItem);
    }

    public TCollection ReadAll<TCollection, TValue>(Func<DataRecord, TValue> readItem)
      where TCollection : ICollection<TValue>, new()
    {
      return ExecuteSafe(() =>
      {
        var collection = new TCollection();
        var record = new DataRecord();
        using (var reader = Command.ExecuteReader())
        {
          while (record.Read(reader))
            collection.Add(readItem(record));
        }

        return collection;
      });
    }

    public async Task<List<T>> ReadAllAsync<T>(Func<DataRecord, T> readItem)
    {
      return await ReadAllAsync<List<T>, T>(readItem, CancellationToken.None);
    }

    public async Task<List<T>> ReadAllAsync<T>(Func<DataRecord, T> readItem, CancellationToken cancellationToken)
    {
      return await ReadAllAsync<List<T>, T>(readItem, cancellationToken);
    }

    public async Task<HashSet<T>> ReadAllHashSetAsync<T>(Func<DataRecord, T> readItem)
    {
      return await ReadAllAsync<HashSet<T>, T>(readItem, CancellationToken.None);
    }

    public async Task<HashSet<T>> ReadAllHashSetAsync<T>(Func<DataRecord, T> readItem, CancellationToken cancellationToken)
    {
      return await ReadAllAsync<HashSet<T>, T>(readItem, cancellationToken);
    }

    public async Task<TCollection> ReadAllAsync<TCollection, TValue>(Func<DataRecord, TValue> readItem)
      where TCollection : ICollection<TValue>, new()
    {
      return await ReadAllAsync<TCollection, TValue>(readItem, CancellationToken.None);
    }

    public async Task<TCollection> ReadAllAsync<TCollection, TValue>(Func<DataRecord, TValue> readItem, CancellationToken cancellationToken)
      where TCollection : ICollection<TValue>, new()
    {
      return await ExecuteSafeAsync(async () =>
      {
        var collection = new TCollection();
        var record = new DataRecord();
        using (var reader = Command.ExecuteReader())
        {
          while (await record.ReadAsync(reader, cancellationToken))
            collection.Add(readItem(record));
        }

        return collection;
      }, cancellationToken);
    }

    public IEnumerable<T> ReadAllByYield<T>(Func<DataRecord, T> readItem)
    {
      PreExecute();
      try
      {
        var record = new DataRecord();
        using (var reader = Command.ExecuteReader())
          while (record.Read(reader))
            yield return readItem(record);
      }
      finally
      {
        ClearCommand();
      }
    }

    public List<DataRecord> ReadAllRecords()
    {
      return ExecuteSafe(() =>
      {
        var records = new List<DataRecord>();
        using (var reader = Command.ExecuteReader())
        {
          var record = new DataRecord();
          while (record.Read(reader))
          {
            records.Add(record);
            record = new DataRecord();
          }
        }

        return records;
      });
    }

    public async Task<List<DataRecord>> ReadAllRecordsAsync()
    {
      return await ReadAllRecordsAsync(CancellationToken.None);
    }

    public async Task<List<DataRecord>> ReadAllRecordsAsync(CancellationToken cancellationToken)
    {
      return await ExecuteSafeAsync(async () =>
      {
        var records = new List<DataRecord>();
        using (var reader = Command.ExecuteReader())
        {
          var record = new DataRecord();
          while (await record.ReadAsync(reader, cancellationToken))
          {
            records.Add(record);
            record = new DataRecord();
          }
        }

        return records;
      }, cancellationToken);
    }

    #endregion

    #endregion

    #region Transaction

    public void OnCommit(Action onCommit)
    {
      _onCommit = (Action)Delegate.Combine(_onCommit, onCommit);
    }

    public void OnRollback(Action onRollback)
    {
      _onRollback = (Action)Delegate.Combine(_onRollback, onRollback);
    }

    public PostresDataContext BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.RepeatableRead)
    {
      if (Transaction != null)
        throw new InvalidOperationException("Transaction already exists");

      OpenConnectionIfClosed();
      Transaction = Connection.BeginTransaction(isolationLevel);
      return this;
    }

    public async Task<PostresDataContext> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.RepeatableRead)
    {
      return await BeginTransactionAsync(CancellationToken.None, isolationLevel);
    }

    public async Task<PostresDataContext> BeginTransactionAsync(CancellationToken cancellationToken, IsolationLevel isolationLevel = IsolationLevel.RepeatableRead)
    {
      if (Transaction != null)
        throw new InvalidOperationException("Transaction already exists");

      await OpenConnectionIfClosedAsync(cancellationToken);
      Transaction = Connection.BeginTransaction(isolationLevel);
      return this;
    }

    public PostresDataContext Commit()
    {
      Transaction.Commit();
      Transaction.Dispose();
      Transaction = null;

      var onCommit = _onCommit;
      onCommit?.Invoke();

      _onCommit = null;
      _onRollback = null;

      return this;
    }

    public async Task<PostresDataContext> CommitAsync()
    {
      return await CommitAsync(CancellationToken.None);
    }

    public async Task<PostresDataContext> CommitAsync(CancellationToken cancellationToken)
    {
      await Transaction.CommitAsync(cancellationToken);
      Transaction.Dispose();
      Transaction = null;

      Action onCommit = _onCommit;
      onCommit?.Invoke();

      _onCommit = null;
      _onRollback = null;

      return this;
    }

    public PostresDataContext Rollback()
    {
      Transaction.Rollback();
      Transaction.Dispose();
      Transaction = null;

      _onCommit = null;
      var onRollback = _onRollback;
      onRollback?.Invoke();

      _onRollback = null;

      return this;
    }

    public async Task<PostresDataContext> RollbackAsync()
    {
      return await RollbackAsync(CancellationToken.None);
    }

    public async Task<PostresDataContext> RollbackAsync(CancellationToken cancellationToken)
    {
      await Transaction.RollbackAsync(cancellationToken);
      Transaction.Dispose();
      Transaction = null;

      _onCommit = null;
      var onRollback = _onRollback;
      onRollback?.Invoke();

      _onRollback = null;

      return this;
    }

    #endregion

    #region Private

    private string SqlInInternal<T>(string field, string param, string alias, NpgsqlDbType dbType, IEnumerable<T> values, bool notIn)
    {
      var enumerator = values?.GetEnumerator();

      if (enumerator == null || !enumerator.MoveNext())
        return "0=1";

      param = "@" + param;

      var i = 0;
      var count = 0;
      do
      {
        Set(param + i, dbType, enumerator.Current);
        i++;
        count++;
      } while (enumerator.MoveNext());

      var sb = new StringBuilder();
      if (!String.IsNullOrEmpty(alias))
        sb.Append(alias).Append(".");
      sb.AppendFormat("`{0}` {1}IN ({2}0", field, notIn ? "NOT " : String.Empty, param);
      for (i = 1; i < count; i++)
        sb.AppendFormat(", {0}{1}", param, i);
      sb.Append(")");
      return sb.ToString();
    }

    private void OpenConnectionIfClosed()
    {
      if (Connection == null)
        Connection = new NpgsqlConnection(_connectionString);
      if (Connection.State == ConnectionState.Closed)
        Connection.Open();
    }

    private async Task OpenConnectionIfClosedAsync(CancellationToken cancellationToken)
    {
      if (Connection == null)
        Connection = new NpgsqlConnection(_connectionString);
      if (Connection.State == ConnectionState.Closed)
        await Connection.OpenAsync(cancellationToken).ConfigureAwait(false);
    }

    private void PreExecute()
    {
      OpenConnectionIfClosed();
      Command.Connection = Connection;
      Command.Transaction = Transaction;
    }

    private async Task PreExecuteAsync(CancellationToken cancellationToken)
    {
      await OpenConnectionIfClosedAsync(cancellationToken).ConfigureAwait(false);
      Command.Connection = Connection;
      Command.Transaction = Transaction;
    }

    private void ClearCommand()
    {
      Command.CommandText = "";
      Command.Parameters.Clear();
    }

    private async Task<T> ExecuteSafeAsync<T>(Func<Task<T>> executionFunction, CancellationToken cancellationToken)
    {
      await PreExecuteAsync(cancellationToken);
      try
      {
        return await executionFunction.Invoke();
      }
      finally
      {
        ClearCommand();
      }
    }

    private T ExecuteSafe<T>(Func<T> executionFunction)
    {
      PreExecute();
      try
      {
        return executionFunction.Invoke();
      }
      finally
      {
        ClearCommand();
      }
    }

    #endregion

    #endregion
  }
}