using System;
using System.Data.Common;

namespace Ustn.Frameworks.Fx.Data
{
    /// <summary>
    /// Extension methods for System.Data.Common.DbProviderFactory.
    /// </summary>
    internal static class DbProviderFactoryExtensions
    {
        /// <summary>
        /// Returns a new instance of the provider's class that implements the
        /// System.Data.Common.DbConnection class.
        /// </summary>
        /// <param name="factory">
        /// The System.Data.Common.DbProviderFactory used to create the
        /// System.Data.Common.DbConnection.
        /// </param>
        /// <param name="connectionString">
        /// The string used to open the connection.
        /// </param>
        /// <returns>
        /// A new instance of System.Data.Common.DbConnection.
        /// </returns>
        public static DbConnection CreateConnection(
            this DbProviderFactory factory,
            string connectionString)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "factory should not be null.");

            DbConnection connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;
            return connection;
        }

        /// <summary>
        /// Returns a new instance of the provider's class that implements the
        /// System.Data.Common.DbCommand class.
        /// </summary>
        /// <param name="factory">
        /// The System.Data.Common.DbProviderFactory used to create the
        /// System.Data.Common.DbCommand.
        /// </param>
        /// <param name="commandText">
        /// The text command to run against the data source.
        /// </param>
        /// <returns>
        /// A new instance of System.Data.Common.DbCommand.
        /// </returns>
        public static DbCommand CreateCommand(
            this DbProviderFactory factory,
            string commandText)
        {
            return CreateCommand(factory, commandText, null, null);
        }

        /// <summary>
        /// Returns a new instance of the provider's class that implements the
        /// System.Data.Common.DbCommand class.
        /// </summary>
        /// <param name="factory">
        /// The System.Data.Common.DbProviderFactory used to create the
        /// System.Data.Common.DbCommand.
        /// </param>
        /// <param name="commandText">
        /// The text command to run against the data source.
        /// </param>
        /// <param name="connection">
        /// The System.Data.Common.DbConnection used by the System.Data.Common.DbCommand.
        /// </param>
        /// <returns>
        /// A new instance of System.Data.Common.DbCommand.
        /// </returns>
        public static DbCommand CreateCommand(
            this DbProviderFactory factory,
            string commandText,
            DbConnection connection)
        {
            return CreateCommand(factory, commandText, connection, null);
        }

        /// <summary>
        /// Returns a new instance of the provider's class that implements the
        /// System.Data.Common.DbCommand class.
        /// </summary>
        /// <param name="factory">
        /// The System.Data.Common.DbProviderFactory used to create the
        /// System.Data.Common.DbCommand.
        /// </param>
        /// <param name="commandText">
        /// The text command to run against the data source.
        /// </param>
        /// <param name="connection">
        /// The System.Data.Common.DbConnection used by the System.Data.Common.DbCommand.
        /// </param>
        /// <param name="transaction">
        /// The System.Data.Common.DbTransaction within which this
        /// System.Data.Common.DbCommand object executes.
        /// </param>
        /// <returns>
        /// A new instance of System.Data.Common.DbCommand.
        /// </returns>
        public static DbCommand CreateCommand(
            this DbProviderFactory factory,
            string commandText,
            DbConnection connection,
            DbTransaction transaction)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "factory should not be null.");

            DbCommand command = factory.CreateCommand();
            command.CommandText = commandText;
            command.Connection = connection;
            command.Transaction = transaction;
            return command;
        }

        /// <summary>
        /// Returns a new instance of the provider's class that implements the
        /// System.Data.Common.DbParameter class.
        /// </summary>
        /// <param name="factory">
        /// The System.Data.Common.DbProviderFactory used to create the
        /// System.Data.Common.DbParameter.
        /// </param>
        /// <param name="parameterName">
        /// The name of the parameter.
        /// </param>
        /// <param name="value">
        /// The value of the parameter.
        /// </param>
        /// <returns>
        /// A new instance of System.Data.Common.DbParameter.
        /// </returns>
        public static DbParameter CreateParameter(
            this DbProviderFactory factory,
            string parameterName,
            object value)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "factory should not be null.");

            DbParameter parameter = factory.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;
            return parameter;
        }

        /// <summary>
        /// Returns a new instance of the provider's class that implements the
        /// System.Data.Common.DbDataAdapter class.
        /// </summary>
        /// <param name="factory">
        /// The System.Data.Common.DbProviderFactory used to create the
        /// System.Data.Common.DbDataAdapter.
        /// </param>
        /// <param name="selectCommand">
        /// The command used to select records in the data source.
        /// </param>
        /// <returns>
        /// A new instance of System.Data.Common.DbDataAdapter.
        /// </returns>
        public static DbDataAdapter CreateDataAdapter(
            this DbProviderFactory factory,
            DbCommand selectCommand)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "factory should not be null.");

            DbDataAdapter adapter = factory.CreateDataAdapter();
            adapter.SelectCommand = selectCommand;
            return adapter;
        }

        /// <summary>
        /// Returns a new instance of the provider's class that implements the
        /// System.Data.Common.DbDataAdapter class.
        /// </summary>
        /// <param name="factory">
        /// The System.Data.Common.DbProviderFactory used to create the
        /// System.Data.Common.DbDataAdapter.
        /// </param>
        /// <param name="selectCommandText">
        /// The text command used to select records in the data source.
        /// </param>
        /// <param name="selectConnectionString">
        /// The string used to open the connection of the select command.
        /// </param>
        /// <returns>
        /// A new instance of System.Data.Common.DbDataAdapter.
        /// </returns>
        public static DbDataAdapter CreateDataAdapter(
            this DbProviderFactory factory,
            string selectCommandText,
            string selectConnectionString)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "factory should not be null.");

            DbDataAdapter adapter = factory.CreateDataAdapter();
            adapter.SelectCommand = factory.CreateCommand();
            adapter.SelectCommand.CommandText = selectCommandText;
            adapter.SelectCommand.Connection = factory.CreateConnection();
            adapter.SelectCommand.Connection.ConnectionString = selectConnectionString;
            return adapter;
        }

        /// <summary>
        /// Returns a new instance of the provider's class that implements the
        /// System.Data.Common.DbCommandBuilder class.
        /// </summary>
        /// <param name="factory">
        /// The System.Data.Common.DbProviderFactory used to create the
        /// System.Data.Common.DbCommandBuilder.
        /// </param>
        /// <param name="adapter">
        /// The System.Data.Common.DbDataAdapter for which SQL statements
        /// are automatically generated.
        /// </param>
        /// <returns>
        /// A new instance of System.Data.Common.DbCommandBuilder.
        /// </returns>
        public static DbCommandBuilder CreateCommandBuilder(
            this DbProviderFactory factory,
            DbDataAdapter adapter)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "factory should not be null.");

            DbCommandBuilder builder = factory.CreateCommandBuilder();
            builder.DataAdapter = adapter;
            return builder;
        }

        /// <summary>
        /// Returns a new instance of the provider's class that implements the
        /// System.Data.Common.DbConnectionStringBuilder class.
        /// </summary>
        /// <param name="factory">
        /// The System.Data.Common.DbProviderFactory used to create the
        /// System.Data.Common.DbConnectionStringBuilder.
        /// </param>
        /// <param name="connectionString">
        /// Connection string associated with the System.Data.Common.DbConnectionStringBuilder.
        /// </param>
        /// <returns>
        /// A new instance of System.Data.Common.DbConnectionStringBuilder.
        /// </returns>
        public static DbConnectionStringBuilder CreateConnectionStringBuilder(
            this DbProviderFactory factory,
            string connectionString)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "factory should not be null.");

            DbConnectionStringBuilder builder = factory.CreateConnectionStringBuilder();
            builder.ConnectionString = connectionString;
            return builder;
        }
    }
}
