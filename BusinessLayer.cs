using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SWE2_TOURPLANNER.DataAccessLayer;
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
