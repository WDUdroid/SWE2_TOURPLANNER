using System.Collections.Generic;
using SWE2_TOURPLANNER.Model;

namespace SWE2_TOURPLANNER.HelperObjects
{
    public class PortHelper
    {
        public TourEntry Tour;
        public List<LogEntry> Logs;

        public PortHelper(TourEntry tour, List<LogEntry> logs)
        {
            Tour = tour;
            Logs = logs;
        }
    }
}