using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;
using SWE2_TOURPLANNER.DataAccessLayer;
using SWE2_TOURPLANNER.HelperObjects;
using SWE2_TOURPLANNER.Logger;

namespace SWE2_TOURPLANNER.Services
{
    public class MapQuest
    {
        private readonly string BaseURL = "https://www.mapquestapi.com";
        private readonly string _apiKey;
        private readonly string _imageSource;

        private readonly HttpClient _client;

        private static readonly log4net.ILog _log = LogHelper.GetLogger();

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


            var mapQuestDataHelper = new MapQuestDataHelper(hasToll, hasBridge, hasTunnel, hasHighway, hasFerry, approxTime, distance, sessionId, "");
            return mapQuestDataHelper;
        }

        public Byte[] LoadImage(string sessionId)
        {
            try
            {
                var url = BaseURL + "/staticmap/v5/map?session=" + sessionId + "&key=" + _apiKey;

                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;


                using HttpWebResponse lxResponse = (HttpWebResponse) request.GetResponse();
                using BinaryReader reader = new BinaryReader(lxResponse.GetResponseStream()!);
                Byte[] lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);


                return lnByte;
            }

            catch (Exception e)
            {
                _log.Error("LoadImage: " + e);
                throw;
            }
        }

        public int DoesLocationExist(string location)
        {
            _log.Info("Entered DoesLocationExist");
            var task = Task.Run(() => _client.GetAsync(BaseURL + "/geocoding/v1/address?key=" + _apiKey + "&location=" + location));
            task.Wait();

            var stringJsonResponse = task.Result.Content.ReadAsStringAsync().Result;


            JObject jSonResponse = JObject.Parse(stringJsonResponse);


            if (jSonResponse["results"]?[0]?["locations"]?.Count() > 1)
            {
                return 0;
            }

            return -1;
        }
    }
}