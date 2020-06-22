using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Toolbox.ADO
{
    public class Connection : IConnection
    {
        private string _connectionString;
        private DbProviderFactory _factory;

        public Connection(IConnectionInfo connectionInfo, DbProviderFactory factory)
        {
            _connectionString = connectionInfo.ConnectionString;
            _factory = factory;
            using (DbConnection conn = CreateConnection())
            {
                conn.ConnectionString = _connectionString;
                conn.Open();
            }
        }

        public DataTable GetDataTable(Command command)
        {
            if (command is null)
                throw new NullReferenceException("Command object is not a valid object");

            using (DbConnection cnx = CreateConnection())
            using (DbCommand cmd = CreateCommand(command, cnx))
            using (DbDataAdapter da = _factory.CreateDataAdapter())
            {
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                cnx.Open();
                da.Fill(dt);
                return dt;
            }
        }

        public int ExecuteNonQuery(Command command)
        {
            if (command == null)
                throw new NullReferenceException("Command object is not a valid object");

            using (DbConnection cnx = CreateConnection())
            using (DbCommand cmd = CreateCommand(command, cnx))
            {
                cnx.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public object ExecuteScalar(Command command)
        {
            if (command == null)
                throw new NullReferenceException("Command object is not a valid object");

            using (DbConnection cnx = CreateConnection())
            using (DbCommand cmd = CreateCommand(command, cnx))
            {
                cnx.Open();
                object o = cmd.ExecuteScalar();
                return (o is DBNull) ? null : o;
            }
        }

        public IEnumerable<TResult> ExecuteReader<TResult>(Command command, Func<IDataRecord, TResult> selector)
        {
            if (command == null)
                throw new NullReferenceException("TCommand object is not a valid object");

            using (DbConnection cnx = CreateConnection())
            using (DbCommand cmd = CreateCommand(command, cnx))
            {
                cnx.Open();
                DbDataReader dreader = cmd.ExecuteReader();
                while (dreader.Read())
                    yield return selector(dreader);
            }
        }

        private DbConnection CreateConnection()
        {
            DbConnection cnx = _factory.CreateConnection();
            cnx.ConnectionString = _connectionString;
            return cnx;
        }

        private DbCommand CreateCommand(Command command, DbConnection cnx)
        {
            DbCommand cmd = _factory.CreateCommand();
            cmd.CommandText = command.Query;
            if (command.IsStoredProcedure) cmd.CommandType = CommandType.StoredProcedure;
            if (command.Parameters.Count > 0)
                foreach (KeyValuePair<string, object> item in command.Parameters)
                {
                    DbParameter p = _factory.CreateParameter();
                    p.ParameterName = item.Key;
                    p.Value = item.Value;
                    cmd.Parameters.Add(p);
                }
            cmd.Connection = cnx;
            return cmd;
        }
    }
}
