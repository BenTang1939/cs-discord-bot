using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

class config{

    private const string ConfigFolder = "Resources";
    private const string ConfigFile = "config.json";

    static BotConfig bot;
    static config(){
        if (!Directory.Exists(ConfigFolder)){ // If directory is not existant then create folder
            Directory.CreateDirectory(ConfigFolder);
        }
        if (!File.Exists(ConfigFile + "/" + ConfigFolder)){ // If file  is non existant then create file in folder
            bot = new BotConfig();
            string json = JsonConvert.SerializeObject(bot, Formatting.Indented);
            File.WriteAllText(ConfigFolder + "/" + ConfigFile, json);
        }
        else{
            string json = File.ReadAllText(ConfigFile + "/" + ConfigFolder);
            var bot = JsonConvert.DeserializeObject<BotConfig>(json); //Find data from json file
        }
    }

    public struct BotConfig
    {
        public string token;
        public string cmdPrefix;
    }
}

