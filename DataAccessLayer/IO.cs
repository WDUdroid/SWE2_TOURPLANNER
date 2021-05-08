using System;
using System.IO;
using System.Net;
using IronPdf;

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

        public int SavePdf(PdfDocument pdf, string path)
        {
            pdf.SaveAs(path);
            return 0;
        }
    }
}