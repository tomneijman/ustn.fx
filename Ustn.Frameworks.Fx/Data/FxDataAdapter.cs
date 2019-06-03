using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Ustn.Frameworks.Fx.Data
{
    public class FxDataAdapter : DbDataAdapter
    {
        private static readonly Lazy<FxDataAdapter> Lazy = new Lazy<FxDataAdapter>(() => new FxDataAdapter());

        public static FxDataAdapter Instance => Lazy.Value;
        private DbProviderFactory _providerFactory;
        private DbConnection _connection;
        private DbDataAdapter _dataAdapter;
        private DbCommandBuilder _commandBuilder;

        private FxDataAdapter()
        {
            FxProviderFactories.RegisterFactories();
            _providerFactory = DbProviderFactories.GetFactory(FxDatabaseSettings.GetProviderInvariant());
            _connection = _providerFactory.CreateConnection();
           if (_connection != null)
            {
                _connection.ConnectionString = FxDatabaseSettings.GetConnectionString();
                _dataAdapter = _providerFactory.CreateDataAdapter();

                _commandBuilder = _providerFactory.CreateCommandBuilder();
                _commandBuilder.DataAdapter = _dataAdapter;
            }
            else
            {
                throw new Exception("Connection not created");
            }
        }

        public List<string> GetMetaData(string fileName)
        {
            var metaData = new List<string>();

            var query = $"SHOW COLUMNS FROM {fileName}";
            var table = ExecuteQuery(query);

            foreach (DataRow row in table.Rows)
            {
                metaData.Add((string) row[0]);
            }

            return metaData;
        }

        private DbCommand CreateCommand(string query)
        {
            DbCommand command = _providerFactory.CreateCommand();
            command.CommandText = query;
            command.Connection = _connection;
            return command;
        }

        public DataTable FindById(string file, int id)
        {
            var query = $"SELECT * FROM {file} WHERE id = {id}";
            return ExecuteQuery(query);
        }
             
        public DataTable ExecuteQuery(string query)
        {
            DataTable table = new DataTable();
            using (_connection)
            {
                var command = CreateCommand(query);
               
                _dataAdapter.SelectCommand = command;
                _dataAdapter.Fill(table);
            }
            return table;
        }

        public void UpdateRow(string file, Dictionary<string, FxField> row)
        {
            bool isChanged = false;

            int id = int.Parse(row["id"].Value.ToString());

            // Refind record...
            var table = FindById(file, id);

            _dataAdapter.UpdateCommand = _commandBuilder.GetUpdateCommand();

            foreach (var field in row)
            {
                if (field.Value.ChangedValue != null)
                {
                    isChanged = true;
                    table.Rows[0][field.Value.Name] = field.Value.ChangedValue;
                }
            }

            if (isChanged)
            {
                _dataAdapter.Update(table);
            }          
        }

        internal void UpdateRow(string fileName, int id, FxRecordBuffer changeBuffer)
        {
            var initSubQuery = "SET ";
            var subQuery = initSubQuery;
            int bufferSize = changeBuffer.Count;
            int counter = 0;
            foreach (var field in changeBuffer)
            {
                counter++;
                subQuery += $"{field.Key} = '{field.Value}'";
                if (counter < bufferSize)
                {
                    subQuery += ", ";
                }
            }
            if (subQuery != initSubQuery)
            {
                using (_connection)
                {
                    var query = $"UPDATE {fileName} "
                                + subQuery
                                + $" WHERE id = {id}";
                    _connection.Open();
                    DbCommand command = CreateCommand(query);

                    command.ExecuteNonQuery();
                    _connection.Close();
                }

            }
        }

        public DataTable InitializeInsertCommand(string file)
        {
            return FindById(file, -1);
        }

        public void InsertRow(string fileName, Dictionary<string, FxField> row)
        {

            var table = InitializeInsertCommand(fileName);

            _dataAdapter.InsertCommand = _commandBuilder.GetInsertCommand();

            DataRow newRow = table.NewRow();
            foreach (var field in row)
            {
                if (field.Value.ChangedValue != null)
                {
                    newRow[field.Value.Name] = field.Value.ChangedValue;
                }
            }
            table.Rows.Add(newRow);
            _dataAdapter.Update(table);
        }

        public void DeleteById(string fileName, int id)
        {
            using (_connection)
            {
                _connection.Open();
                DbCommand command = CreateCommand($"DELETE FROM {fileName} WHERE id = {id}");
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }
    }
}
