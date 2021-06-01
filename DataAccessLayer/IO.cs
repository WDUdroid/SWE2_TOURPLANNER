using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using IronPdf;
using SWE2_TOURPLANNER.Logger;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;

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

        public int SaveNewTourImage(string fileName, Byte[] imageBytes)
        {
            _log.Info("Entered SaveNewTourImage");

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
                _log.Error("SaveNewTourImage: " + e);
                return -1;
            }
        }

        public int UpdateTourImage(string path, Bitmap image)
        {
            _log.Info("Entered UpdateTourImageWithDrawing");

            image.Save(path);

            return 0;
        }

        public Bitmap FetchImageFormPath(string fileName)
        {
            _log.Info("Entered FetchImageFromPath");

            using (Stream BitmapStream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                Image img = Image.FromStream(BitmapStream);

                var mBitmap = new Bitmap(img);

                return mBitmap;
            }
        }

        public Bitmap ConvertStrokestoImage(StrokeCollection strokes, int width, int height)
        {
            InkCanvas Ink = new InkCanvas();
            Ink.RenderSize = new System.Windows.Size(width, height);
            Ink.Strokes.Add(strokes);
            RenderTargetBitmap bmp = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(Ink);
            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            encoder.Save(stream);
            Bitmap b = new Bitmap(stream);
            return b;
        }

        public Bitmap CombineBitmap(Bitmap under, Bitmap over)
        {
            // Copy the background.
            var combinedBitmap = new Bitmap(under);
            over.MakeTransparent(Color.Black);


            // Add the overlay.

            using (Graphics gr = Graphics.FromImage(combinedBitmap))
            {
                gr.DrawImage(over, 0,0);

            }

            return combinedBitmap;
        }
    }
}