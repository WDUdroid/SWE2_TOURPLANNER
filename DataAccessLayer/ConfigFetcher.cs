using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SWE2_TOURPLANNER.HelperObjects;

namespace SWE2_TOURPLANNER.DataAccessLayer
{
    public class ConfigFetcher
    {
        private static ConfigFetcher instance = new ConfigFetcher();

        public string DatabaseSource;
        public string ImageSource;

        private ConfigFetcher()
        {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string configFileLocation = projectDirectory + @"\Config\config.json";
            JObject configFileContent;
            
            if (!File.Exists(configFileLocation))
            {
                MessageBox.Show("Fatal Error: Config File does not exist");
                //System.Environment.Exit(1);
            }

            using (StreamReader file = File.OpenText(configFileLocation))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                configFileContent = (JObject)JToken.ReadFrom(reader);
                MessageBox.Show(configFileContent.ToString());
            }

            if (configFileContent.ContainsKey("DatabaseSource") && configFileContent.ContainsKey("ImageSource"))
            {
                DatabaseSource = (string) configFileContent["DatabaseSource"];
                ImageSource = (string) configFileContent["ImageSource"];
            }

            MessageBox.Show(DatabaseSource);
            MessageBox.Show(ImageSource);
        }

        public static ConfigFetcher Instance => instance;


    }
}
