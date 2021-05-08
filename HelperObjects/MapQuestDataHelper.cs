using System;

namespace SWE2_TOURPLANNER.HelperObjects
{
    public class MapQuestDataHelper
    {
        public string HasTollRoad;
        public string HasBridge;
        public string HasTunnel;
        public string HasHighway;
        public string HasFerry;
        public string ApproxTime;
        public string Distance;
        public string SessionId;
        public string TourImage;


        public MapQuestDataHelper(string toll, string bridge, string tunnel, 
                                string highway, string ferry, string approxTime, 
                                string distance, string sessionId, string tourImage)
        {
            HasTollRoad = toll;
            HasBridge = bridge;
            HasTunnel = tunnel;
            HasHighway = highway;
            HasFerry = ferry;
            ApproxTime = approxTime;
            Distance = distance;
            SessionId = sessionId;
            TourImage = tourImage;
        }
    }
}