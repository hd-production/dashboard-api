using System;
using Npgsql;

namespace HdProduction.Dashboard.Application.NpgsqlOrm
{
  public interface IDatabaseConnector
  {
    NpgsqlConnection NewConnection();
    PostresDataContext NewDataContext();
  }

  public class DatabaseConnector : IDatabaseConnector
  {
    private readonly string _connection;

    public DatabaseConnector(string connection)
    {
      _connection = connection;
    }

    public NpgsqlConnection NewConnection()
    {
      return new NpgsqlConnection(_connection);
    }

    public PostresDataContext NewDataContext()
    {
      return new PostresDataContext(_connection);
    }
  }
}