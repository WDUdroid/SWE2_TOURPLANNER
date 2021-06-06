using System;
using System.IO;
using System.Net;
using IronPdf;
using SWE2_TOURPLANNER.Logger;

namespace SWE2_TOURPLANNER.DataAccessLayer
{
    public class IO
    {
        private static readonly log4net.ILog _log = LogHelper.GetLogger();

        private readonly string _imageSource;
        public IO(string imageSource)
        {
            _imageSource = imageSource;
        }

        public string GetImportJson(string fileName)
        {
            try
            {
                _log.Info("Entered GetImportJson");

                string json = String.Empty;

                using (StreamReader r = new StreamReader(fileName))
                {
                    json = r.ReadToEnd();
                }

                return json;
            }

            catch (Exception e)
            {
                _log.Error("GetImportJson: " + e);
                return "ERROR";
            }
        }

        public int SavePdf(PdfDocument pdf, string path)
        {
            try
            {
                if (!path.EndsWith(".pdf"))
                {
                    return -1;
                }


                _log.Info("Entered SafePdf");

                pdf.SaveAs(path);
                return 0;
            }

            catch (Exception e)
            {
                _log.Error("SavePdf: " + e);
                return -1;
            }

        }

        public string AvailableFileName()
        {
            _log.Info("Entered AvailableFileName");

            Random rand = new Random();
            var imageName = Convert.ToString(rand.Next(999999999));
            imageName += ".jpg";

            while (File.Exists(_imageSource + @"\" + imageName) == true)
            {
                imageName = Convert.ToString(rand.Next(999999999));
                imageName += ".jpg";
            }

            return _imageSource + @"\" + imageName;
        }

        public int SaveImage(string fileName, Byte[] imageBytes)
        {
            _log.Info("Entered SaveImage");

            try
            {

                using (FileStream fs = File.Create(fileName))
                {
                    fs.Write(imageBytes, 0, imageBytes.Length);
                }

                return 0;

            }

            catch (Exception e)
            {
                _log.Error("SaveImage: " + e);
                return -1;
            }
        }
    }
}