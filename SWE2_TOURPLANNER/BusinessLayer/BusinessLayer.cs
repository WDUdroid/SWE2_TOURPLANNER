using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Ink;
using IronPdf;
using Newtonsoft.Json;
using SWE2_TOURPLANNER.DataAccessLayer;
using SWE2_TOURPLANNER.Logger;
using SWE2_TOURPLANNER.Model;
using SWE2_TOURPLANNER.Services;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace SWE2_TOURPLANNER.BusinessLayer
{
    public class BusinessLayer
    {
        private static readonly BusinessLayer instance = new BusinessLayer();

        private static readonly log4net.ILog _log = LogHelper.GetLogger();

        private readonly ConfigFetcher _config;
        private readonly DatabaseHandler _database;
        private readonly MapQuest _map;
        private readonly PdfCreater _pdf;
        private readonly IO _iO;

        private BusinessLayer()
        {
            _config = new ConfigFetcher();
            _database = new DatabaseHandler(_config.DatabaseSource);
            _map = new MapQuest(_config.MapQuestKey, _config.ImageSource);
            _pdf = new PdfCreater();
            _iO = new IO(_config.ImageSource);
        }

        public int DoesTourExist(string tourName)
        {
            _log.Info("Entered DoesTourExist");

            if (_database.DoesTourAlreadyExist(tourName) != 0)
            {
                _log.Info("Database.DoesTourAlreadyExist returned -1");
                return -1;
            }

            _log.Info("Database.DoesTourAlreadyExist returned 0");
            return 0;
        }

        public int DoLocationsExist(string tourFrom, string tourTo)
        {
            _log.Info("Entered DoLocationsExist");

            if (_map.DoesLocationExist(tourFrom) == -1 ||
                _map.DoesLocationExist(tourTo) == -1)
            {
                _log.Info("MapQuest.DoesLocationExist returned -1");
                return -1;
            }

            _log.Info("MapQuest.DoesLocationExist returned 0");
            return 0;
        }


        public MapQuestDataHelper GetMapQuestInfo(string tourFrom, string tourTo, string routeType)
        {
            _log.Info("Entered GetMapQuestInfo");
            var tmpMapQuestDataHelper = _map.GetMapQuestRouteSession(tourFrom, tourTo, routeType);

            if (tmpMapQuestDataHelper.SessionId != null)
            {


                var availableFileName = _iO.AvailableFileName();
                var imageBytes = _map.LoadImage(tmpMapQuestDataHelper.SessionId);
                _iO.SaveNewTourImage(availableFileName, imageBytes);

                tmpMapQuestDataHelper.TourImage = availableFileName;

                return tmpMapQuestDataHelper;
            }

            _log.Warn("MapQuest did not return SessionID");
            tmpMapQuestDataHelper.TourImage = _config.ImageSource + "/fail.jpg";
            return tmpMapQuestDataHelper;
        }

        public void AddTour(string tourName, string tourDescription,
                    string routeInformation, string tourDistance,
                    string tourFrom, string tourTo, string tourImage)
        {
            _log.Info("Entered AddTour");
            _database.AddTourToDb(tourName, tourDescription, routeInformation, tourDistance, tourFrom, tourTo, tourImage);
        }

        public int AddLog(string tourName, DateTime logDate, string totalTime,
            string distance, string elevation, string avgSpeed, string bpm,
            string rating, string report, string usedSupplies, string tourmates)
        {
            _log.Info("Entered AddLog");
            _database.AddLogToTour(tourName, logDate,
                int.Parse(totalTime), int.Parse(distance),
                int.Parse(elevation), avgSpeed,
                int.Parse(bpm), rating, report,
                usedSupplies, tourmates);

            return 0;
        }

        public int DeleteLog(string tourName, DateTime logDate, string totalTime,
            string distance, string elevation, string avgSpeed, string bpm,
            string rating, string report, string usedSupplies, string tourmates)
        {
            _log.Info("Entered DeleteLog");
            _database.DeleteLogFromTour(tourName, logDate,
                int.Parse(totalTime), int.Parse(distance),
                int.Parse(elevation), avgSpeed,
                int.Parse(bpm), rating, report,
                usedSupplies, tourmates);

            return 0;
        }

        public int DeleteTour(string tourName)
        {
            _log.Info("Entered DeleteTour");
            _database.DeleteTourFromDb(tourName);
            _database.DeleteAllLogsFromTour(tourName);

            return 0;
        }

        public int DeleteOnlyTour(string tourName)
        {
            _log.Info("Entered DeleteOnlyTour");
            _database.DeleteTourFromDb(tourName);

            return 0;
        }

        public void ExportTourAsPdf(string tourName)
        {
            _log.Info("Entered ExportTourAsPdf");
            var fileDialog = new FileDialog();
            string dirToSaveTo = fileDialog.SavePdfDialogFunc();

            if (dirToSaveTo != string.Empty)
            {
                _log.Info("FileDialog.SavePdfDialogFunc returned string");
                PdfDocument compPdf = _pdf.CreatePdf(_database.GetTour(tourName), _database.GetLogsList(tourName));
                _iO.SavePdf(compPdf, dirToSaveTo);
            }

            else
            {
                _log.Info("FileDialog.SavePdfDialogFunc returned empty string");
            }
        }


        public int ExportToursAsJson()
        {
            _log.Info("Entered ExportToursAsJson");
            var fileDialog = new FileDialog();
            string dirToSaveTo = fileDialog.SaveFileDialogFunc();


            var dataToSave = _database.GetExportablePackage();


            string jsonString = JsonConvert.SerializeObject(dataToSave);
            if (dirToSaveTo != string.Empty)
            {
                _log.Info("FileDialog.SaveFileDialogFunc returned string");
                File.WriteAllText(dirToSaveTo, jsonString);
            }
            else
            {
                _log.Info("FileDialog.SaveFileDialogFunc returned empty string");
                return -1;
            }


            return 0;
        }

        public int ImportTours()
        {
            _log.Info("Entered ImportTours");
            var fileDialog = new FileDialog();
            string fileDir = fileDialog.OpenFileDialogFunc();

            if (fileDir == "ERROR")
            {
                _log.Info("FileDialog.OpenFileDialogFunc returned ERROR");
                return -1;
            }

            _log.Info("FileDialog.OpenFileDialogFunc returned string");
            string tmpJsonRaw = _iO.GetImportJson(fileDir);
            MessageBox.Show(tmpJsonRaw);

            var jsonConverted = JsonConvert.DeserializeObject<List<PortHelper>>(tmpJsonRaw);

            if (jsonConverted != null)
                for (int i = 0; i < jsonConverted.Count; i++)
                {
                    if (_database.DoesTourAlreadyExist(jsonConverted[i].Tour.TourName) != -1)
                    {
                        _database.ImportTourToDb(jsonConverted[i].Tour);
                    }

                    if (jsonConverted[i].Logs.Count > 0)
                    {
                        for (int j = 0; j < jsonConverted[i].Logs.Count; j++)
                        {
                            if (_database.DoesLogAlreadyExist(jsonConverted[i].Logs[j].TourName,
                                jsonConverted[i].Logs[j].LogDate) != -1)
                            {
                                _database.ImportLogToTour(jsonConverted[i].Logs[j]);
                            }
                        }
                    }
                }

            return 0;
        }

        public ObservableCollection<TourEntry> GetAllTours()
        {
            return _database.GetToursFromDb();
        }

        public ObservableCollection<TourEntry> GetToursContainingString(string searchText)
        {
            return _database.GetToursContainingString(searchText);
        }

        public ObservableCollection<LogEntry> GetLogsOfTour(string tourName)
        {
            return _database.GetLogsOfTour(tourName);
        }

        public string SaveStrokes(string tourName, string originalImage, StrokeCollection strokes)
        {
            var baseBitmap = _iO.FetchImageFormPath(originalImage);
            var strokesBitmap = _iO.ConvertStrokestoImage(strokes, 800, 600);
            var combinedBitmap = _iO.CombineBitmap(baseBitmap, strokesBitmap);

            var newFilePath = _iO.AvailableFileName();

            _iO.UpdateTourImage(newFilePath ,combinedBitmap);
            _database.ReplaceTourImage(tourName, newFilePath);

            return newFilePath;
        }

        public static BusinessLayer Instance => instance;


    }


}
