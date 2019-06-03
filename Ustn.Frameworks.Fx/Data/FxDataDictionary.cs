using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Ustn.Frameworks.Fx.System;

namespace Ustn.Frameworks.Fx.Data
{
    public class FxDataDictionary : FxFile
    {
        private IList<string> _serverFiles = new List<string>();
        private IList<string> _clientFiles = new List<string>();
        private IList<FxDataDictionary> _ddoServers = new List<FxDataDictionary>();
        private IList<FxDataDictionary> _ddoClients = new List<FxDataDictionary>();
        private IDictionary<string, string> _foreignFields = new Dictionary<string, string>();
        public string ForeignField { get; set; }
        public Dictionary<string, FxField> CurrentRecord { get; set; }
        private DataTable _currentDataTable;
        public FxDataDictionary(string fileName) : base(fileName)
        {
            CurrentRecord = FxRecord.Buffer[AliasFileName];
        }


        public void AddServerFile(string fileName)
        {
            _serverFiles.Add(fileName);
        }

        public void AddClientFile(string fileName)
        {
            _clientFiles.Add(fileName);
        }

        private void AttachClient(FxDataDictionary ddoClient)
        {
            _ddoClients.Add(ddoClient);
        }

        public void AttachServer(FxDataDictionary ddoServer, string foreignField = null)
        {
            foreignField = foreignField ?? $"id_{ddoServer.AliasFileName}";

            _foreignFields[ddoServer.AliasFileName] = foreignField;
            _ddoServers.Add(ddoServer);
            AttachClient(this);
        }

        public void SetDefaultValue(string fieldName, object val)
        {
            CurrentRecord[fieldName].Options["default"] = val;
            FxRecord.Buffer[AliasFileName][fieldName].Options["default"] = val;
        }

        // todo Default waarden vullen!!!
        public void ClearRecordBuffer()
        {
            var recBuffer = FxRecord.Buffer[AliasFileName];

            foreach (var field in recBuffer)
            {
                field.Value.Value = null;
                var key = "default";
                if (field.Value.Options.ContainsKey(key) && field.Value.Options[key] != null)
                {
                    field.Value.ChangedValue = field.Value.Options[key];
                }
            }
        }

        public void FindById(int id)
        {
            var table = FxDataAdapter.Instance.FindById(FileName, id);
            _currentDataTable = table;

            if (table != null && table.Rows.Count > 0)
            {
                DataRow row = table.Rows[0];

                var i = 0;
                foreach (var field in FxRecord.Buffer[AliasFileName])
                {
                    SetValue(field.Key, row.ItemArray[i]);
                    i++;
                }

                foreach (var ddoServer in _ddoServers)
                {
                    var ddoServerId = int.Parse(GetValue(_foreignFields[ddoServer.AliasFileName]).ToString());
                   ddoServer.FindById(ddoServerId);
                }
            }
        }

        public void SetChangedValue(string fieldName, object val)
        {
            CurrentRecord[fieldName].ChangedValue = val;
        }

        public void SetValue(string fieldName, object val)
        {
            CurrentRecord[fieldName].Value = val;
        }

        public int CurrentId()
        {
            return int.Parse(CurrentRecord["id"].Value.ToString());
        }

        public void DeleteById(int id)
        {
            FxDataAdapter.Instance.DeleteById(FileName, id);
            ClearRecordBuffer();
        }

        private void UpdateRow()
        {
            bool isChanged = false;
            var changeBuffer = new FxRecordBuffer();

            foreach (var field in CurrentRecord)
            {
                if (field.Value.ChangedValue != null)
                {
                    isChanged = true;
                    changeBuffer[field.Key] = field.Value.ChangedValue.ToString();
                }
            }
            if (isChanged)
            {
                FxDataAdapter.Instance.UpdateRow(FileName, CurrentId(), changeBuffer);
            }
        }

        public void RequestSave()
        {
            if (CurrentRecord["id"].Value != null)
            {
                UpdateRow();
            }
            else
            {
                FxDataAdapter.Instance.InsertRow(FileName, CurrentRecord);
            }
        }


        public object GetValue(string fieldName)
        {
            return CurrentRecord[fieldName].Value;
        }

        public object GetChangedValue(string fieldName)
        {
            return CurrentRecord[fieldName].ChangedValue;
        }

        public object GetFile(string fieldName)
        {
            return CurrentRecord[fieldName].File;
        }
    }
}
