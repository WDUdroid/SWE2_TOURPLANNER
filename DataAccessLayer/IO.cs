using System;
using System.IO;
using System.Net;

namespace SWE2_TOURPLANNER.DataAccessLayer
{
    public class IO
    {
        public IO()
        {

        }

        public string GetImportJson(string fileName)
        {
            string json = String.Empty;

            using (StreamReader r = new StreamReader(fileName))
            {
                json = r.ReadToEnd();
            }

            return json;
        }
    }
}