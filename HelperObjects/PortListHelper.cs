using System.Collections.Generic;
using SWE2_TOURPLANNER.Model;

namespace SWE2_TOURPLANNER.HelperObjects
{
    public class PortListHelper
    {
        public List<PortHelper> Ports;

        public PortListHelper(List<PortHelper> ports)
        {
            Ports = ports;
        }
    }
}