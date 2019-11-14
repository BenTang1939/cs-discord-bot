using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace csbot
{
    class DataStorage
    {
        internal Dictionary<string,string> pairs = new Dictionary<string,string>();
        internal DataStorage(){
            ValidateStorageFile("DataStorage.json");
            string json = File.ReadAllText("DataStorage.json");
            pairs = JsonConvert.DeserializeObject<Dictionary<string,string>>(json);
        }

        internal void SaveData(){
            string json = JsonConvert.SerializeObject(pairs, Formatting.Indented);
            File.WriteAllText("DataStorage.json", json);
        }

        private static bool ValidateStorageFile(string file){
            if (!File.Exists(file)){
                File.WriteAllText(file, "");
                SaveData();
                return false;
            }
            return true;
        }
    }
}