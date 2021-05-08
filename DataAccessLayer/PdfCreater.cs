using System.Collections.Generic;
using IronPdf;
using SWE2_TOURPLANNER.Model;

namespace SWE2_TOURPLANNER.DataAccessLayer
{
    public class PdfCreater
    {
        public PdfCreater()
        {}

        public PdfDocument CreatePdf(TourEntry tour, List<LogEntry> logs)
        {
            string buildPDF = $"<h1 style=\"font-family:Courier;\"> Report of Tour: {tour.TourName}</h1>" +
                              $"<h2 style=\"font-family:Courier;\"> Tour description: {tour.TourDescription}</h2>" +
                              $"<h2 style=\"font-family:Courier;\">Route information: {tour.RouteInformation}</h2>" +
                              $"<h2 style=\"font-family:Courier;\">Start: {tour.TourFrom}</h2>" +
                              $"<h2 style=\"font-family:Courier;\">End: {tour.TourTo}</h2>" +
                              $"<img src='{tour.TourImage}'>" +
                              $"<h1>LOGS:</h1>" +
                              $"<ol>";

            foreach (var log in logs)
            {
                buildPDF += $"<li>" +
                            $"<h3> Log date: {log.LogDate}</h3>" +
                            $"<p> distance: {log.Distance}</p>" +
                            $"<p> elevation: {log.Elevation}</p>" +
                            $"<p> average Speed: {log.AvgSpeed}</p>" +
                            $"<p> BPM: {log.BPM}</p>" +
                            $"<p> rating: {log.Rating}</p>" +
                            $"<p> report: {log.Report}</p>" +
                            $"<p> used supplies: {log.UsedSupplies}</p>" +
                            $"<p> tourmates: {log.Tourmates}</p>" +
                            $"</li>";
            }

            buildPDF += "</ol>";

            var renderer = new IronPdf.HtmlToPdf();
            var pdf = renderer.RenderHtmlAsPdf(buildPDF);




            // Advanced:  Load external html assets: images, css and javascript.
            // An optional "base" asset locating path can be set as a url or local directory. 
            //var PDF = Renderer.RenderHtmlAsPdf("<img src='icons/iron.png'>", @"C:\site\assets\");

            return pdf;
            //PDF.SaveAs("html-with-assets.pdf");
        }
    }
}