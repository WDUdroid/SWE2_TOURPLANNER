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
using SWE2_TOURPLANNER.HelperObjects;

namespace SWE2_TOURPLANNER.Services
{
    public class MapQuest
    {
        private readonly string BaseURL = "https://www.mapquestapi.com";
        private readonly string _apiKey;
        private readonly string _imageSource;

        private readonly HttpClient _client;



        public MapQuest(string key, string imageSource)
        {
            _apiKey = key;
            _imageSource = imageSource;

            _client = new HttpClient();
        }

        public MapQuestDataHelper GetMapQuestRouteSession(string from, string to, string routeType)
        {
            var task = Task.Run(() => _client.GetAsync(BaseURL + "/directions/v2/route?key=" + _apiKey + "&from=" +
                                                       @from + "&to=" + to + "&routeType=" + routeType));
            task.Wait();
            //var rawResponse = await Client.GetAsync(BaseURL + "/directions/v2/route?key=" + ApiKey + "&from=" + @from + "&to=" + to + "&routeType=" + routeType);
            var stringJsonResponse = task.Result.Content.ReadAsStringAsync().Result;

            JObject jSonResponse = JObject.Parse(stringJsonResponse);

            var hasToll = (string)jSonResponse["route"]?["hasTollRoad"];
            var hasBridge = (string)jSonResponse["route"]?["hasBridge"];
            var hasTunnel = (string)jSonResponse["route"]?["hasTunnel"];
            var hasHighway = (string)jSonResponse["route"]?["hasHighway"];
            var hasFerry = (string)jSonResponse["route"]?["hasFerry"];
            var approxTime = (string)jSonResponse["route"]?["formattedTime"];
            var distance = (string)jSonResponse["route"]?["distance"];
            var sessionId = (string)jSonResponse["route"]?["sessionId"];

            var imageDir = LoadImage(sessionId);

            var mapQuestDataHelper = new MapQuestDataHelper(hasToll, hasBridge, hasTunnel, hasHighway, hasFerry, approxTime, distance, sessionId, imageDir);
            return mapQuestDataHelper;
        }

        public string LoadImage(string sessionId)
        {
            var url = BaseURL + "/staticmap/v5/map?session=" + sessionId + "&key=" + _apiKey;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;


            Random rand = new Random();
            var imageName = Convert.ToString(rand.Next(999999999));
            imageName += ".jpg";

            while (File.Exists(_imageSource + @"\" + imageName) == true)
            {
                imageName = Convert.ToString(rand.Next(999999999));
                imageName += ".jpg";
            }


            using HttpWebResponse lxResponse = (HttpWebResponse)request.GetResponse();
            using BinaryReader reader = new BinaryReader(lxResponse.GetResponseStream());
            Byte[] lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
            using (FileStream fs = File.Create(_imageSource + @"\" + imageName))
            {
                fs.Write(lnByte, 0, lnByte.Length);
            }

            return _imageSource + @"\" + imageName;
        }

        public string GetImage(string from, string to)
        {
            var url = BaseURL + "/staticmap/v5/map?key=" + _apiKey + "&start=" + from + "&end=" + to;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            Random rand = new Random();
            var imageName = Convert.ToString(rand.Next(999999999));
            imageName += ".jpg";

            while (File.Exists(_imageSource + @"\" + imageName) == true)
            {
                imageName = Convert.ToString(rand.Next(999999999));
                imageName += ".jpg";
            }


            using HttpWebResponse lxResponse = (HttpWebResponse)request.GetResponse();
            using BinaryReader reader = new BinaryReader(lxResponse.GetResponseStream());
            Byte[] lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
            using FileStream fs = File.Create(_imageSource + @"\" + imageName);
            fs.Write(lnByte, 0, lnByte.Length);

            return _imageSource + @"\" + imageName;
        }
    }
}