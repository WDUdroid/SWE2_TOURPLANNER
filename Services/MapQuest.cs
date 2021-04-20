using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;
using SWE2_TOURPLANNER.DataAccessLayer;

namespace SWE2_TOURPLANNER.Services
{
    public class MapQuest
    {
        private static MapQuest instance = new MapQuest();

        private readonly string BaseURL = "https://www.mapquestapi.com";
        private string ApiKey;

        private HttpClient Client;

        private ConfigFetcher Config;

        //private string SessionId;

        private MapQuest()
        {
            Config = ConfigFetcher.Instance;
            ApiKey = Config.MapQuestKey;

            Client = new HttpClient();
        }

        //public async Task<string> GetMapQuestRouteSession(string from, string to)
        //{
        //    var rawResponse = await Client.GetAsync(BaseURL + "/directions/v2/route?key=" + ApiKey + "&from=" + from + "&to=" + to);
        //    var stringJsonResponse = rawResponse.Content.ReadAsStringAsync().Result;

        //    JObject jSonResponse = JObject.Parse(stringJsonResponse);


        //    SessionId = (string)jSonResponse["route"]?["sessionId"];

        //    LoadImage();

        //    return (string)jSonResponse["route"]?["sessionId"];
        //}

        //public string LoadImage()
        //{
        //    var url = BaseURL + "/staticmap/v5/map?session=" + SessionId + "&key=" + ApiKey;

        //    HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
        //    request.AutomaticDecompression = DecompressionMethods.GZip;


        //    using (HttpWebResponse lxResponse = (HttpWebResponse) request.GetResponse())
        //    {
        //        using (BinaryReader reader = new BinaryReader(lxResponse.GetResponseStream()))
        //        {
        //            Byte[] lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
        //            using (FileStream fs = new FileStream(Config.ImageSource + @"\test.png", FileMode.Create))
        //            {
        //                fs.Write(lnByte,0,lnByte.Length);
        //            }
        //        }
        //    }

        //    return "something";
        //}

        public string GetImage(string from, string to)
        {
            var url = BaseURL + "/staticmap/v5/map?key=" + ApiKey + "&start=" + from + "&end=" + to;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            Random rand = new Random();
            var imageName = Convert.ToString(rand.Next(999999999));
            imageName += ".jpg";

            while (File.Exists(Config.ImageSource + @"\" + imageName) == true)
            {
                imageName = Convert.ToString(rand.Next(999999999));
                imageName += ".jpg";
            }


            using (HttpWebResponse lxResponse = (HttpWebResponse)request.GetResponse())
            {
                using (BinaryReader reader = new BinaryReader(lxResponse.GetResponseStream()))
                {
                    Byte[] lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
                    //File.Create(Config.ImageSource+@"\test.jpg").Close();
                    using (FileStream fs = File.Create(Config.ImageSource + @"\" + imageName))
                    {
                        fs.Write(lnByte, 0, lnByte.Length);
                    }
                }
            }

            return Config.ImageSource + @"\" + imageName;
        }


        public static MapQuest Instance => instance;
    }
}