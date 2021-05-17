using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IronPdf;
using Newtonsoft.Json;
using SWE2_TOURPLANNER.DataAccessLayer;
using SWE2_TOURPLANNER.HelperObjects;
using SWE2_TOURPLANNER.Logger;
using SWE2_TOURPLANNER.Model;
using SWE2_TOURPLANNER.Services;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace SWE2_TOURPLANNER
{
    public class BusinessLayer
    {
        private static readonly BusinessLayer instance = new BusinessLayer();

        private static log4net.ILog _log = LogHelper.GetLogger();

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
            _iO = new IO();

            //Log = log4net.LogManager.GetLogger("BusinessLayer.cs");
        }

        public int DoesTourExist(string tourName)
        {
            if (_database.DoesTourAlreadyExist(tourName) != 0)
            {
                return -1;
            }

            return 0;
        }

        public int DoLocationsExist(string tourFrom, string tourTo)
        {
            if (_map.DoesExist(tourFrom) == -1 ||
                _map.DoesExist(tourTo) == -1)
            {
                return -1;
            }

            return 0;
        }


        public MapQuestDataHelper GetMapQuestInfo(string tourFrom, string tourTo, string routeType)
        {
            return _map.GetMapQuestRouteSession(tourFrom, tourTo, routeType);
        }

        public void AddTour(string tourName, string tourDescription,
                    string routeInformation, string tourDistance,
                    string tourFrom, string tourTo, string tourImage)
        {

            _database.AddTourToDb(tourName, tourDescription, routeInformation, tourDistance, tourFrom, tourTo, tourImage);
        }

        public int AddLog(string tourName, DateTime logDate, string totalTime,
            string distance, string elevation, string avgSpeed, string bpm,
            string rating, string report, string usedSupplies, string tourmates)
        {
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
            _database.DeleteLogFromTour(tourName, logDate,
                int.Parse(totalTime), int.Parse(distance),
                int.Parse(elevation), avgSpeed,
                int.Parse(bpm), rating, report,
                usedSupplies, tourmates);

            return 0;
        }

        public int DeleteTour(string tourName)
        {
            _database.DeleteTourFromDb(tourName);
            _database.DeleteAllLogsFromTour(tourName);

            return 0;
        }

        public int DeleteOnlyTour(string tourName)
        {
            _database.DeleteTourFromDb(tourName);

            return 0;
        }

        public void ExportTourAsPdf(string tourName)
        {
            var fileDialog = new FileDialog();
            string dirToSaveTo = fileDialog.SavePdfDialogFunc();

            if (dirToSaveTo != string.Empty)
            {
                PdfDocument compPdf = _pdf.CreatePdf(_database.GetTour(tourName), _database.GetLogsList(tourName));
                _iO.SavePdf(compPdf, dirToSaveTo);
            }
        }


        public int ExportToursAsJson()
        {
            var fileDialog = new FileDialog();
            string dirToSaveTo = fileDialog.SaveFileDialogFunc();


            var dataToSave = _database.GetExportablePackage();

            //PortListHelper dataObjectToSave = new PortListHelper(dataToSave);

            string jsonString = JsonConvert.SerializeObject(dataToSave);
            if (dirToSaveTo != string.Empty)
            {
                File.WriteAllText(dirToSaveTo, jsonString);
            }
            else
            {
                return -1;
            }


            return 0;
        }

        public int ImportTours()
        {
            var fileDialog = new FileDialog();
            string fileDir = fileDialog.OpenFileDialogFunc();

            if (fileDir == "ERROR")
            {
                MessageBox.Show("Could not get File Directory!");
                return -1;
            }

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

        public static BusinessLayer Instance => instance;


    }


}
