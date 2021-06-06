using System;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SWE2_TOURPLANNER_TEST
{
    public class TestConfigFetcher
    {
        public string DatabaseSource;
        public string ImageSource;
        public string MapQuestKey;
        public string ConfigFileLocation = @"D:\testconfig.json";

        public TestConfigFetcher()
        {
            JObject configFileContent;


            using (StreamReader file = File.OpenText(ConfigFileLocation))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                configFileContent = (JObject)JToken.ReadFrom(reader);
            }

            if (configFileContent.ContainsKey("DatabaseSource") && configFileContent.ContainsKey("ImageSource"))
            {
                DatabaseSource = (string)configFileContent["DatabaseSource"];
                ImageSource = (string)configFileContent["ImageSource"];
                MapQuestKey = (string)configFileContent["MapQuestKey"];
            }
        }
    }
}
