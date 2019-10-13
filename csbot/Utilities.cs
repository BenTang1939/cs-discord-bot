using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

class Utilities{

    private static Dictionary<string, string> alerts;
    static Utilities(){
        string json = File.ReadAllText("SystemLang/alerts.json"); //Get values from alerts
        var data = JsonConvert.DeserializeObject<dynamic>(json);
        alerts = data.ToObject<Dictionary<string,string>>(); //Assign to dicitonary as object
    }

    public static string GetAlert(string key){
        if(alerts.ContainsKey(key)) { //Checks for key in .json file
            return alerts[key]; //Return Data
        }
        return ""; //Else return 0
    }
}
