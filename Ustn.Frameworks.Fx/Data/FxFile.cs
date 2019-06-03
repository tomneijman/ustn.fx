using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Reflection;
using Ustn.Frameworks.Fx.System;

namespace Ustn.Frameworks.Fx.Data
{

    public class FxFile
    {
        public string FileName { get; set; }
        private string _aliasFileName;


        public string AliasFileName
        {
            get { return _aliasFileName; }
            set
            {
                var _aliasFileName = value;
            }
        }

        private void CreateRecordBuffer()
        {
            //Record.Buffer[_aliasFileName] = null;

            var metaData = FxDataAdapter.Instance.GetMetaData(FileName);

            var fieldDict = new Dictionary<string, FxField>();
            foreach (var col in metaData)
            {
                var field = CreateField();
                field.Name = col;
                field.Options = new FxRecordBuffer();
                field.Options["default"] = null;
                fieldDict[col] = field;
            }
            FxRecord.Buffer[_aliasFileName] = fieldDict;
        }

        //private Dictionary<string, string> RetrieveChangedFields()
        //{
        //    var dict = new Dictionary<string, string>();
        //    foreach (var field in CurrentRecord)
        //    {
        //        if (field.Value.ChangedValue != null)
        //        {
        //            dict[field.Value.Name] = (string)field.Value.ChangedValue;
        //        }
        //    }

        //    return dict;
        //}

        //function privateSaveMainFileUpdate()
        //{
        //    $field = $this->recBuffer;
        //    if ($field) {
        //        reset($field);
        //        $subQuery = "set ";
        //            $i = 0;
        //            $k = 0;
        //            $idx = $this->aliasFile;

        //        if (!empty($this->customRecBuffer[$idx]))
        //        {
        //            $customField = $this->customRecBuffer[$idx];
        //            $k = sizeof($customField);
        //        }
        //        $length = sizeof($field) - $k;
        //        while (list($key, $value) = each($field))
        //        {
        //            ++$i;
        //            if ($i <= $length) {
        //                $inValue = addslashes($this->fieldInValue($key));
        //                //$inValue = str_replace("'", "\'", $inValue);
        //                $subQuery.= "`$key` = '$inValue'";
        //                if ($i < $length) {
        //                    $subQuery.= ", ";
        //                }
        //            }
        //        }
        //        $query = "update $this->mainFile $subQuery where id = $field[id]";
        //            //die("Q " . $query);
        //            $this->db->executeQuery($query);
        //        $this->currentRecord = $field["id"];
        //    }
        //} // privateSaveMainFileUpdate




        protected void Update()
        {
            Console.WriteLine($"MainFile {FileName}");

            var buffer = FxRecord.Buffer[FileName];
            var subQuery = "SET ";
            bool isChanged = false;

            foreach (var field in buffer)
            {
                if (field.Value.ChangedValue != null)
                {
                    isChanged = true;

                    subQuery += $"`{field.Key}` = \"{field.Value.ChangedValue}\", ";
                }
            }

            if (isChanged)
            {
                var query = $"UPDATE {FileName} " + subQuery + "WHERE id = " + buffer["id"].Value;
                Console.WriteLine("QUERY = " + query);
            }

        }


        //void addOrUpdate(Dictionary<int, int> dic, int key, int newValue)
        //{
        //    int val;
        //    if (dic.TryGetValue(key, out val))
        //    {
        //        // yay, value exists!
        //        dic[key] = val + newValue;
        //    }
        //    else
        //    {
        //        // darn, lets add the value
        //        dic.Add(key, newValue);
        //    }
        //}

        private FxField CreateField()
        {
            return new FxField();
        }

        public FxFile(string fileName)
        {
            FileName = fileName;
            _aliasFileName = fileName;
            CreateRecordBuffer();
        }
    }
}
