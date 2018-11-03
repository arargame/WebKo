using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using WebKo.Model.General;

namespace Reporting.Core.Data
{
    public enum ConnectionType
    {
        Oracle,
        MsSql
    }

    public interface IDbConnection
    {
        ConnectionType ConnectionType { get; set; }

        string ConnectingString { get; set; }

        DbConnection Connection { get; set; }

        IDbConnection SetConnectionString(string connectionString);

        IDbConnection SetConnection(DbConnection connection);

        IDbConnection Connect();

        IDbConnection Disconnect();
    }

    public class ConnectionString
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public static List<ConnectionString> List = new List<ConnectionString>();
    }

    public abstract class CustomConnection : BaseObject , IDbConnection
    {
        public CustomConnection(string provider,string connectionString)
        {
            if (!string.IsNullOrEmpty(connectionString))
                SetConnectionString(connectionString);

            Connection = DbProviderFactories.GetFactory(provider)
                                            .CreateConnection();

            Connection.ConnectionString = ConnectingString;
        }

        public ConnectionType ConnectionType { get; set; }

        public string ConnectingString { get; set; }

        public DbConnection Connection { get; set; }

        public IDbConnection SetConnectionString(string connectionStringName)
        {
            try
            {
                ConnectingString = ConnectionString.List.SingleOrDefault(cs => cs.Key == connectionStringName).Value;
            }
            catch (Exception ex)
            {
                Log.Create(new Log(,));
            }

            return this;
        }

        public IDbConnection SetConnection(DbConnection connection)
        {
            Connection = connection;

            return this;
        }

        public IDbConnection Connect()
        {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();

            //message = "Veritabanı açık";

            return this;
        }

        public IDbConnection Disconnect()
        {
            if (Connection.State != ConnectionState.Closed)
                Connection.Close();

           // message = "Veritabanı kapalı";

            return this;
        }

        public IDbCommand CreateCommand()
        {
            return Connection.CreateCommand();
        }
    }

    public class OracleConnection : CustomConnection 
    {
        public OracleConnection(string provider = "System.Data.OracleClient", string connectionString = "OracleConnectionString")
            : base(provider, connectionString)
        {
            ConnectionType = Data.ConnectionType.Oracle;
        }
    }

    public class MsSqlConnection : CustomConnection
    {
        public MsSqlConnection(string provider = "System.Data.SqlClient", string connectionString = "MsSqlConnectionString")
            : base(provider, connectionString)
        {
            ConnectionType = Data.ConnectionType.MsSql;
        }
    }
}
