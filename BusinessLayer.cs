using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using SWE2_TOURPLANNER.DataAccessLayer;
using SWE2_TOURPLANNER.HelperObjects;
using SWE2_TOURPLANNER.Model;
using SWE2_TOURPLANNER.Services;

namespace SWE2_TOURPLANNER
{
    public class BusinessLayer
    {
        private static BusinessLayer instance = new BusinessLayer();

        private ConfigFetcher Config = ConfigFetcher.Instance;
        private DatabaseHandler Database = DatabaseHandler.Instance;
        private MapQuest Map = MapQuest.Instance;

        private BusinessLayer()
        {
            
        }


        public string GetImage(string tourFrom, string tourTo)
        {
            return Map.GetImage(tourFrom, tourTo);
        }

        public void AddTour(string tourName, string tourDescription, 
                    string routeInformation, string tourDistance, 
                    string tourFrom, string tourTo, string tourImage)
        {

            Database.AddTourToDb(tourName, tourDescription, routeInformation, tourDistance, tourFrom, tourTo, tourImage);
        }

        public int AddLog(string tourName, DateTime logDate, string totalTime,
            string distance, string elevation, string avgSpeed, string bpm,
            string rating, string report, string usedSupplies, string tourmates)
        {
            Database.AddLogToTour(tourName, logDate,
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
            Database.DeleteLogFromTour(tourName, logDate,
                int.Parse(totalTime), int.Parse(distance),
                int.Parse(elevation), avgSpeed,
                int.Parse(bpm), rating, report,
                usedSupplies, tourmates);
            
            return 0;
        }

        public int DeleteTour(string tourName)
        {
            Database.DeleteTourFromDb(tourName);
            Database.DeleteAllLogsFromTour(tourName);

            return 0;
        }

        public int ExportToursAsPDF()
        {
            var fileDialog = new FileDialog();
            fileDialog.SaveFileDialogFunc();

            return 0;
        }

        public int ExportToursAsJSON()
        {
            var fileDialog = new FileDialog();
            string dirToSaveTo = fileDialog.SaveFileDialogFunc();

            var dataToSave = new List<PortHelper>();
            dataToSave = Database.GetExportablePackage();

            //PortListHelper dataObjectToSave = new PortListHelper(dataToSave);

            string jsonString = JsonConvert.SerializeObject(dataToSave);
            if (dirToSaveTo != String.Empty)
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
            var iO = new IO();
            var fileDialog = new FileDialog();
            string fileDir = fileDialog.OpenFileDialogFunc();

            if (fileDir == "ERROR")
            {
                MessageBox.Show("Could not get File Directory!");
                return -1;
            }

            string tmpJsonRaw = iO.GetImportJson(fileDir);
            MessageBox.Show(tmpJsonRaw);

            List<PortHelper> jsonConverted = new List<PortHelper>();
            jsonConverted = JsonConvert.DeserializeObject<List<PortHelper>>(tmpJsonRaw);

            for (int i = 0; i < jsonConverted.Count; i++)
            {
                if (Database.DoesTourAlreadyExist(jsonConverted[i].Tour.TourName) != -1)
                {
                    Database.ImportTourToDb(jsonConverted[i].Tour);
                }

                if (jsonConverted[i].Logs.Count > 0)
                {
                    for (int j = 0; j < jsonConverted[i].Logs.Count; j++)
                    {
                        if (Database.DoesLogAlreadyExist(jsonConverted[i].Logs[j].TourName,
                            jsonConverted[i].Logs[j].LogDate) != -1)
                        {
                            Database.ImportLogToTour(jsonConverted[i].Logs[j]);
                        }
                    }
                }
            }

            return 0;
        }

        public ObservableCollection<TourEntry> GetAllTours()
        {
            return Database.GetToursFromDb();
        }

        public ObservableCollection<TourEntry> GetToursContainingString(string searchText)
        {
            return Database.GetToursContainingString(searchText);
        }

        public ObservableCollection<LogEntry> GetLogsOfTour(string tourName)
        {
            return Database.GetLogsOfTour(tourName);
        }

        public static BusinessLayer Instance => instance;


    }


}
