using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using SWE2_TOURPLANNER.HelperObjects;
using SWE2_TOURPLANNER.Model;

namespace SWE2_TOURPLANNER.DataAccessLayer
{
    public class DatabaseHandler
    {
        private static DatabaseHandler instance = new DatabaseHandler();

        private string DatabaseSource;

        private DatabaseHandler()
        {
            DatabaseSource = ConfigFetcher.Instance.DatabaseSource;
        }

        public static DatabaseHandler Instance => instance;

        public ObservableCollection<LogEntry> GetLogsOfTour(string tourName)
        {
            ObservableCollection<LogEntry> tmpLogContainer = new ObservableCollection<LogEntry>();

            using var con = new NpgsqlConnection(DatabaseSource);
            con.Open();

            string sql1 = $"SELECT * FROM logs WHERE tourname = '{tourName}'";
            using var cmd1 = new NpgsqlCommand(sql1, con);

            using var reader = cmd1.ExecuteReader();


            while (reader.Read())
            {
                DateTime logDate = DateTime.Parse(reader.GetString(reader.GetOrdinal("logdate")));
                int totalTime = reader.GetInt32(reader.GetOrdinal("totaltime"));
                int distance = reader.GetInt32(reader.GetOrdinal("distance"));
                int elevation = reader.GetInt32(reader.GetOrdinal("elevation"));
                string avgSpeed = reader.GetString(reader.GetOrdinal("avgspeed"));
                int bmp = reader.GetInt32(reader.GetOrdinal("bpm"));
                string rating = reader.GetString(reader.GetOrdinal("rating"));
                string report = reader.GetString(reader.GetOrdinal("report"));
                string usedSupplies = reader.GetString(reader.GetOrdinal("usedsupplies"));
                string tourmates = reader.GetString(reader.GetOrdinal("tourmates"));

                tmpLogContainer.Add(new LogEntry(tourName, logDate, totalTime,
                    distance, elevation, avgSpeed, bmp, rating, report,
                    usedSupplies, tourmates));
            }
            con.Close();

            return tmpLogContainer;
        }

        public ObservableCollection<TourEntry> GetToursFromDb()
        {
            ObservableCollection<TourEntry> tmpLogContainer = new ObservableCollection<TourEntry>();

            using var con = new NpgsqlConnection(DatabaseSource);
            con.Open();

            string sql1 = $"SELECT * FROM tours";
            using var cmd1 = new NpgsqlCommand(sql1, con);

            using var reader = cmd1.ExecuteReader();


            while (reader.Read())
            {
                string tourName = reader.GetString(reader.GetOrdinal("tourname"));
                string tourDescription = reader.GetString(reader.GetOrdinal("tourdescription"));
                string routeInformation = reader.GetString(reader.GetOrdinal("routeinformation"));
                string tourDistance = reader.GetString(reader.GetOrdinal("Tourdistance"));
                string tourFrom = reader.GetString(reader.GetOrdinal("tourfrom"));
                string tourTo = reader.GetString(reader.GetOrdinal("tourTo"));
                string tourImage = reader.GetString(reader.GetOrdinal("tourimage"));

                tmpLogContainer.Add(new TourEntry(tourName, tourDescription, routeInformation, tourDistance, tourFrom, tourTo, tourImage));
            }
            con.Close();

            return tmpLogContainer;
        }

        public int AddLogToTour(string tourName, DateTime logDate, int totalTime, int distance,
            int elevation, string avgSpeed, int bpm, string rating,
            string report, string usedSupplies, string tourmates)
        {
            using var con = new NpgsqlConnection(DatabaseSource);
            con.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"INSERT INTO logs (tourname, logdate, totaltime, distance, elevation, avgspeed, bpm, rating, report, usedsupplies, tourmates) " +
                              $"VALUES ('{tourName}', '{logDate}', {totalTime},{distance}, {elevation}, '{avgSpeed}'," +
                              $"{bpm}, '{rating}', '{report}', '{usedSupplies}', '{tourmates}')";
            var status = cmd.ExecuteNonQuery();

            if (status < 0)
            {
                MessageBox.Show($"AddLogToTour -> An Error occurred, Error Code: '{status}'");
            }

            return status;
        }

        public int AddTourToDb(string tourName, string tourDescription, string routeInformation, 
                                string tourDistance, string tourFrom, string tourTo, string tourImage)
        {
            using var con = new NpgsqlConnection(DatabaseSource);
            con.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"INSERT INTO tours (tourname, tourdescription, routeinformation, tourdistance, tourfrom, tourto, tourimage) " +
                              $"VALUES ('{tourName}','{tourDescription}','{routeInformation}','{tourDistance}','{tourFrom}','{tourTo}','{tourImage}')";
            var status = cmd.ExecuteNonQuery();

            if (status < 0)
            {
                MessageBox.Show($"AddTourToDb -> An Error occurred, Error Code: '{status}'");
            }

            return status;
        }

        public int DeleteLogFromTour(string tourName, DateTime logDate, int totalTime, int distance,
            int elevation, string avgSpeed, int bpm, string rating,
            string report, string usedSupplies, string tourmates)
        {
            using var con = new NpgsqlConnection(DatabaseSource);
            con.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"DELETE FROM logs WHERE " +
                              $"tourname = '{tourName}' AND logdate = '{logDate.ToString()}' AND totaltime = {totalTime} AND distance ={distance} AND " +
                              $"elevation = {elevation} AND avgspeed = '{avgSpeed}' AND " +
                              $"bpm = {bpm} AND rating = '{rating}' AND report = '{report}' AND usedsupplies = '{usedSupplies}' AND tourmates = '{tourmates}'";
            var status = cmd.ExecuteNonQuery();

            if (status < 0)
            {
                MessageBox.Show($"DeleteLogFromTour -> An Error occurred, Error Code: {status}");
            }

            return status;
        }

        public int DeleteTourFromDb(string tourName)
        {
            using var con = new NpgsqlConnection(DatabaseSource);
            con.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"DELETE FROM tours WHERE " +
                              $"tourname = '{tourName}'";
            var status = cmd.ExecuteNonQuery();

            if (status < 0)
            {
                MessageBox.Show($"DeleteTourFromDb -> An Error occurred, Error Code: {status}");
            }

            return status;
        }

        public int DeleteAllLogsFromTour(string tourName)
        {
            using var con = new NpgsqlConnection(DatabaseSource);
            con.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"DELETE FROM logs WHERE " +
                              $"tourname = '{tourName}'";
            var status = cmd.ExecuteNonQuery();

            if (status < 0)
            {
                MessageBox.Show($"DeleteAllLogsFromTour -> An Error occurred, Error Code: {status}");
            }

            return status;
        }

        public ObservableCollection<TourEntry> GetToursContainingString(string searchString)
        {
            if (searchString == null)
            {
                return GetToursFromDb();
            }

            ObservableCollection<TourEntry> tmpLogContainer = new ObservableCollection<TourEntry>();

            using var con = new NpgsqlConnection(DatabaseSource);
            con.Open();

            string sql1 = $"SELECT * FROM tours FULL JOIN logs ON tours.tourname = logs.tourname WHERE " +
                          $" LOWER(tours.tourname) LIKE LOWER('%{searchString}%') OR " +
                          $" LOWER(tours.tourdescription) LIKE LOWER('%{searchString}%') OR " +
                          $" LOWER(tours.routeinformation) LIKE LOWER('%{searchString}%') OR " +
                          $" LOWER(tours.tourdistance) LIKE LOWER('%{searchString}%') OR " +
                          $" LOWER(tours.tourfrom) LIKE LOWER('%{searchString}%') OR " +
                          $" LOWER(tours.tourto) LIKE LOWER('%{searchString}%') OR " +
                          $" LOWER(tours.tourimage) LIKE LOWER('%{searchString}%') OR " +
                          $" LOWER(logs.logdate) LIKE LOWER('%{searchString}%') OR " +
                          $" LOWER(logs.avgspeed) LIKE LOWER('%{searchString}%') OR " +
                          $" LOWER(logs.rating) LIKE LOWER('%{searchString}%') OR " +
                          $" LOWER(logs.report) LIKE LOWER('%{searchString}%') OR " +
                          $" LOWER(logs.usedsupplies) LIKE LOWER('%{searchString}%') OR " +
                          $" LOWER(logs.tourmates) LIKE LOWER('%{searchString}%')";
            using var cmd1 = new NpgsqlCommand(sql1, con);

            using var reader = cmd1.ExecuteReader();


            while (reader.Read())
            {
                string tourName = reader.GetString(reader.GetOrdinal("tourname"));
                string tourDescription = reader.GetString(reader.GetOrdinal("tourdescription"));
                string routeInformation = reader.GetString(reader.GetOrdinal("routeinformation"));
                string tourDistance = reader.GetString(reader.GetOrdinal("Tourdistance"));
                string tourFrom = reader.GetString(reader.GetOrdinal("tourfrom"));
                string tourTo = reader.GetString(reader.GetOrdinal("tourTo"));
                string tourImage = reader.GetString(reader.GetOrdinal("tourimage"));

                if (!tmpLogContainer.Any(x => x.TourName == tourName)){
                    tmpLogContainer.Add(new TourEntry(tourName, tourDescription, routeInformation, tourDistance, tourFrom, tourTo, tourImage));
                }
            }
            con.Close();

            return tmpLogContainer;
        }


    }
}
