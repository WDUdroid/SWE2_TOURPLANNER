using System;
using System.IO;
using IronPdf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using NUnit.Framework;
using SWE2_TOURPLANNER.DataAccessLayer;
using SWE2_TOURPLANNER.Model;
using SWE2_TOURPLANNER.Services;

namespace SWE2_TOURPLANNER_TEST
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void LocationExistsCheckTestShouldNotExist()
        {
            var testConfig = new TestConfigFetcher();

            var map = new MapQuest(testConfig.MapQuestKey, testConfig.ImageSource);

            Assert.AreEqual(-1, map.DoesLocationExist("Dieser Ort existiert nicht"));
        }
        
        [Test]
        public void LocationExistsCheckTestShouldExist()
        {
            var testConfig = new TestConfigFetcher();

            var map = new MapQuest(testConfig.MapQuestKey, testConfig.ImageSource);

            Assert.AreEqual(0, map.DoesLocationExist("Wien"));
        }

        [Test]
        public void AddTourTest()
        {
            var testConfig = new TestConfigFetcher();
            var database = new DatabaseHandler(testConfig.DatabaseSource);

            database.AddTourToDb("nametest", "destest",
                "routest", "distest", "fromtest",
                "totest", "imatest");



            string tourName = null;
            string tourDescription = null;
            string routeInformation = null;
            string tourDistance = null;
            string tourFrom = null;
            string tourTo = null;
            string tourImage = null;

            using var con = new NpgsqlConnection(testConfig.DatabaseSource);
            con.Open();

            string sql1 = $"SELECT * FROM tours";
            using var cmd1 = new NpgsqlCommand(sql1, con);

            using var reader = cmd1.ExecuteReader();

            var counter = 0;

            while (reader.Read())
            {
                tourName = reader.GetString(reader.GetOrdinal("tourname"));
                tourDescription = reader.GetString(reader.GetOrdinal("tourdescription"));
                routeInformation = reader.GetString(reader.GetOrdinal("routeinformation"));
                tourDistance = reader.GetString(reader.GetOrdinal("Tourdistance"));
                tourFrom = reader.GetString(reader.GetOrdinal("tourfrom"));
                tourTo = reader.GetString(reader.GetOrdinal("tourTo"));
                tourImage = reader.GetString(reader.GetOrdinal("tourimage"));

                counter++;
            }
            con.Close();

            using var conDel = new NpgsqlConnection(testConfig.DatabaseSource);
            conDel.Open();

            using var cmdDel = new NpgsqlCommand
            {
                Connection = conDel,
                CommandText = $"DELETE FROM tours WHERE " +
                              $"tourname = '{tourName}'"
            };
            var status = cmdDel.ExecuteNonQuery();

            conDel.Close();

            Assert.AreEqual(1, counter);
            Assert.AreEqual("nametest", tourName);
            Assert.AreEqual("destest", tourDescription);
            Assert.AreEqual("routest", routeInformation);
            Assert.AreEqual("distest", tourDistance);
            Assert.AreEqual("fromtest", tourFrom);
            Assert.AreEqual("totest", tourTo);
            Assert.AreEqual("imatest", tourImage);
        }

        [Test]
        public void ImportTourTest()
        {
            var testConfig = new TestConfigFetcher();
            var database = new DatabaseHandler(testConfig.DatabaseSource);

            var tmpTourEntry = new TourEntry("nametest", "destest",
                "routest", "distest", "fromtest",
                "totest", "imatest");

            database.ImportTourToDb(tmpTourEntry);



            string tourName = null;
            string tourDescription = null;
            string routeInformation = null;
            string tourDistance = null;
            string tourFrom = null;
            string tourTo = null;
            string tourImage = null;

            using var con = new NpgsqlConnection(testConfig.DatabaseSource);
            con.Open();

            string sql1 = $"SELECT * FROM tours";
            using var cmd1 = new NpgsqlCommand(sql1, con);

            using var reader = cmd1.ExecuteReader();

            var counter = 0;

            while (reader.Read())
            {
                tourName = reader.GetString(reader.GetOrdinal("tourname"));
                tourDescription = reader.GetString(reader.GetOrdinal("tourdescription"));
                routeInformation = reader.GetString(reader.GetOrdinal("routeinformation"));
                tourDistance = reader.GetString(reader.GetOrdinal("Tourdistance"));
                tourFrom = reader.GetString(reader.GetOrdinal("tourfrom"));
                tourTo = reader.GetString(reader.GetOrdinal("tourTo"));
                tourImage = reader.GetString(reader.GetOrdinal("tourimage"));

                counter++;
            }
            con.Close();

            using var conDel = new NpgsqlConnection(testConfig.DatabaseSource);
            conDel.Open();

            using var cmdDel = new NpgsqlCommand
            {
                Connection = conDel,
                CommandText = $"DELETE FROM tours WHERE " +
                              $"tourname = '{tourName}'"
            };
            var status = cmdDel.ExecuteNonQuery();

            conDel.Close();

            Assert.AreEqual(1, counter);
            Assert.AreEqual("nametest", tourName);
            Assert.AreEqual("destest", tourDescription);
            Assert.AreEqual("routest", routeInformation);
            Assert.AreEqual("distest", tourDistance);
            Assert.AreEqual("fromtest", tourFrom);
            Assert.AreEqual("totest", tourTo);
            Assert.AreEqual("imatest", tourImage);
        }

        [Test]
        public void DeleteTourTest()
        {
            var testConfig = new TestConfigFetcher();
            var database = new DatabaseHandler(testConfig.DatabaseSource);

            using var con = new NpgsqlConnection(testConfig.DatabaseSource);
            con.Open();

            using var cmd = new NpgsqlCommand
            {
                Connection = con,
                CommandText =
                    $"INSERT INTO tours (tourname, tourdescription, routeinformation, tourdistance, tourfrom, tourto, tourimage) " +
                    $"VALUES ('nametest','nametest','nametest','nametest','nametest','nametest','nametest')"
            };

            cmd.ExecuteNonQuery();
            con.Close();

            database.DeleteTourFromDb("nametest");


            using var con2 = new NpgsqlConnection(testConfig.DatabaseSource);
            con2.Open();


            string sql1 = $"SELECT Count(*) FROM tours WHERE tourname = 'nametest'";
            using var cmd1 = new NpgsqlCommand(sql1, con2);

            long exi = (long)cmd1.ExecuteScalar();
            con2.Close();

            Assert.AreEqual(0, exi);

        }

        [Test]
        public void DoesTourExistTestShouldNotExist()
        {
            var testConfig = new TestConfigFetcher();
            var database = new DatabaseHandler(testConfig.DatabaseSource);


            using var con2 = new NpgsqlConnection(testConfig.DatabaseSource);
            con2.Open();

            string sql1 = $"SELECT Count(*) FROM tours WHERE tourname = 'tourExists'";
            using var cmd1 = new NpgsqlCommand(sql1, con2);

            long exi = (long)cmd1.ExecuteScalar();
            con2.Close();

            Assert.AreEqual(0, exi);

            Assert.AreEqual(0, database.DoesTourAlreadyExist("tourExists"));

        }


        [Test]
        public void DoesTourExistTestShouldExist()
        {
            var testConfig = new TestConfigFetcher();
            var database = new DatabaseHandler(testConfig.DatabaseSource);

            using var con = new NpgsqlConnection(testConfig.DatabaseSource);
            con.Open();

            using var cmd = new NpgsqlCommand
            {
                Connection = con,
                CommandText =
                    $"INSERT INTO tours (tourname, tourdescription, routeinformation, tourdistance, tourfrom, tourto, tourimage) " +
                    $"VALUES ('tourThatExists','nametest','nametest','nametest','nametest','nametest','nametest')"
            };

            cmd.ExecuteNonQuery();
            con.Close();


            using var con2 = new NpgsqlConnection(testConfig.DatabaseSource);
            con2.Open();

            string sql1 = $"SELECT Count(*) FROM tours WHERE tourname = 'tourThatExists'";
            using var cmd1 = new NpgsqlCommand(sql1, con2);

            long exi = (long)cmd1.ExecuteScalar();
            con2.Close();

            var doesExist = database.DoesTourAlreadyExist("tourThatExists");

            using var conDel = new NpgsqlConnection(testConfig.DatabaseSource);
            conDel.Open();

            using var cmdDel = new NpgsqlCommand
            {
                Connection = conDel,
                CommandText = $"DELETE FROM tours WHERE " +
                              $"tourname = 'tourThatExists'"
            };
            var status = cmdDel.ExecuteNonQuery();

            conDel.Close();


            Assert.AreEqual(1, exi);
            Assert.AreEqual(-1, doesExist);

        }


        [Test]
        public void AddLogTest()
        {
            var testConfig = new TestConfigFetcher();
            var database = new DatabaseHandler(testConfig.DatabaseSource);

            database.AddLogToTour("addlogtest", DateTime.Now, 1, 1, 1,
                "1", 1, "1", "1", "1", "1");

            using var con = new NpgsqlConnection(testConfig.DatabaseSource);
            con.Open();

            string sql1 = $"SELECT Count(*) FROM logs WHERE tourname = 'addlogtest'";
            using var cmd1 = new NpgsqlCommand(sql1, con);

            long exi = (long)cmd1.ExecuteScalar();

            con.Close();


            using var conDel = new NpgsqlConnection(testConfig.DatabaseSource);
            conDel.Open();

            using var cmdDel = new NpgsqlCommand
            {
                Connection = conDel,
                CommandText = $"DELETE FROM logs WHERE " +
                              $"tourname = 'addlogtest'"
            };
            var status = cmdDel.ExecuteNonQuery();

            conDel.Close();


            Assert.AreEqual(1, exi);
        }

        [Test]
        public void ImportLogTest()
        {
            var testConfig = new TestConfigFetcher();
            var database = new DatabaseHandler(testConfig.DatabaseSource);

            var tmpLogEntry = new LogEntry("addlogtest", DateTime.Now, 1, 1, 1,
                "1", 1, "1", "1", "1", "1");

            database.ImportLogToTour(tmpLogEntry);

            using var con = new NpgsqlConnection(testConfig.DatabaseSource);
            con.Open();

            string sql1 = $"SELECT Count(*) FROM logs WHERE tourname = 'addlogtest'";
            using var cmd1 = new NpgsqlCommand(sql1, con);

            long exi = (long)cmd1.ExecuteScalar();

            con.Close();


            using var conDel = new NpgsqlConnection(testConfig.DatabaseSource);
            conDel.Open();

            using var cmdDel = new NpgsqlCommand
            {
                Connection = conDel,
                CommandText = $"DELETE FROM logs WHERE " +
                              $"tourname = 'addlogtest'"
            };
            var status = cmdDel.ExecuteNonQuery();

            conDel.Close();


            Assert.AreEqual(1, exi);
        }

        [Test]
        public void DeleteLogTest()
        {
            var testConfig = new TestConfigFetcher();
            var database = new DatabaseHandler(testConfig.DatabaseSource);

            using var con = new NpgsqlConnection(testConfig.DatabaseSource);
            con.Open();

            var currentDateTime = DateTime.Now;

            using var cmd = new NpgsqlCommand
            {
                Connection = con,
                CommandText =
                    $"INSERT INTO logs (tourname, logdate, totaltime, distance, elevation, avgspeed, bpm, rating, report, usedsupplies, tourmates) " +
                    $"VALUES ('deleteLogTest', '{currentDateTime}', 1,1, 1, '1'," +
                    $"1, '1', '1', '1', '1')"
            };
            var status = cmd.ExecuteNonQuery();
            con.Close();

            database.DeleteLogFromTour("deleteLogTest", currentDateTime, 1, 1, 1,
                "1", 1, "1", "1", "1", "1");


            using var con2 = new NpgsqlConnection(testConfig.DatabaseSource);
            con2.Open();

            string sql1 = $"SELECT Count(*) FROM logs WHERE tourname = 'deleteLogTest'";
            using var cmd1 = new NpgsqlCommand(sql1, con2);

            long exi = (long)cmd1.ExecuteScalar();

            con2.Close();

            Assert.AreEqual(0, exi);
        }

        [Test]
        public void DeleteAllLogsFromTourTest()
        {
            var testConfig = new TestConfigFetcher();
            var database = new DatabaseHandler(testConfig.DatabaseSource);

            using var con = new NpgsqlConnection(testConfig.DatabaseSource);
            con.Open();

            var currentDateTime = DateTime.Now;

            using var cmd = new NpgsqlCommand
            {
                Connection = con,
                CommandText =
                    $"INSERT INTO logs (tourname, logdate, totaltime, distance, elevation, avgspeed, bpm, rating, report, usedsupplies, tourmates) " +
                    $"VALUES ('deleteAllLogsTest', '{currentDateTime}', 1,1, 1, '1'," +
                    $"1, '1', '1', '1', '1')"
            };
            cmd.ExecuteNonQuery();
            con.Close();

            using var con1 = new NpgsqlConnection(testConfig.DatabaseSource);
            con1.Open();

            using var cmd2 = new NpgsqlCommand
            {
                Connection = con1,
                CommandText =
                    $"INSERT INTO logs (tourname, logdate, totaltime, distance, elevation, avgspeed, bpm, rating, report, usedsupplies, tourmates) " +
                    $"VALUES ('deleteAllLogsTest', '{currentDateTime}', 1,1, 1, '1'," +
                    $"1, '1', '1', '1', '1')"
            };
            cmd2.ExecuteNonQuery();
            con1.Close();


            using var con3 = new NpgsqlConnection(testConfig.DatabaseSource);
            con3.Open();

            string sql3 = $"SELECT Count(*) FROM logs WHERE tourname = 'deleteAllLogsTest'";
            using var cmd3 = new NpgsqlCommand(sql3, con3);

            long exiBefore = (long)cmd3.ExecuteScalar();

            con3.Close();

            database.DeleteAllLogsFromTour("deleteAllLogsTest");


            using var con2 = new NpgsqlConnection(testConfig.DatabaseSource);
            con2.Open();

            string sql1 = $"SELECT Count(*) FROM logs WHERE tourname = 'deleteAllLogsTest'";
            using var cmd1 = new NpgsqlCommand(sql1, con2);

            long exi = (long)cmd1.ExecuteScalar();

            con2.Close();

            Assert.AreEqual(2, exiBefore);
            Assert.AreEqual(0, exi);
        }


        [Test]
        public void DoesLogExistTestShouldNotExist()
        {
            var testConfig = new TestConfigFetcher();
            var database = new DatabaseHandler(testConfig.DatabaseSource);


            using var con2 = new NpgsqlConnection(testConfig.DatabaseSource);
            con2.Open();

            string sql1 = $"SELECT Count(*) FROM logs WHERE tourname = 'logExists'";
            using var cmd1 = new NpgsqlCommand(sql1, con2);

            long exi = (long)cmd1.ExecuteScalar();
            con2.Close();

            Assert.AreEqual(0, exi);

            Assert.AreEqual(0, database.DoesLogAlreadyExist("tourExists", DateTime.Now));

        }


        [Test]
        public void DoesLogExistTestShouldExist()
        {
            var testConfig = new TestConfigFetcher();
            var database = new DatabaseHandler(testConfig.DatabaseSource);

            using var con = new NpgsqlConnection(testConfig.DatabaseSource);
            con.Open();

            var currentDateTime = DateTime.Now;

            using var cmd = new NpgsqlCommand
            {
                Connection = con,
                CommandText =
                    $"INSERT INTO logs (tourname, logdate, totaltime, distance, elevation, avgspeed, bpm, rating, report, usedsupplies, tourmates) " +
                    $"VALUES ('logThatExists', '{currentDateTime}', 1,1, 1, '1'," +
                    $"1, '1', '1', '1', '1')"
            };
            cmd.ExecuteNonQuery();
            con.Close();


            using var con2 = new NpgsqlConnection(testConfig.DatabaseSource);
            con2.Open();

            string sql1 = $"SELECT Count(*) FROM logs WHERE tourname = 'logThatExists'";
            using var cmd1 = new NpgsqlCommand(sql1, con2);

            long exi = (long)cmd1.ExecuteScalar();
            con2.Close();

            var doesExist = database.DoesLogAlreadyExist("logThatExists", currentDateTime);

            using var conDel = new NpgsqlConnection(testConfig.DatabaseSource);
            conDel.Open();

            using var cmdDel = new NpgsqlCommand
            {
                Connection = conDel,
                CommandText = $"DELETE FROM logs WHERE " +
                              $"tourname = 'logThatExists'"
            };
            var status = cmdDel.ExecuteNonQuery();

            conDel.Close();


            Assert.AreEqual(1, exi);
            Assert.AreEqual(-1, doesExist);

        }

        [Test]
        public void GetTourTestWithNoTours()
        {
            var testConfig = new TestConfigFetcher();
            var database = new DatabaseHandler(testConfig.DatabaseSource);

            var returnTour = database.GetTour("doesNotExist");

            Assert.AreEqual(null, returnTour);
        }

        [Test]
        public void GetTourTestWithTours()
        {
            var testConfig = new TestConfigFetcher();
            var database = new DatabaseHandler(testConfig.DatabaseSource);

            using var con = new NpgsqlConnection(testConfig.DatabaseSource);
            con.Open();

            using var cmd = new NpgsqlCommand
            {
                Connection = con,
                CommandText =
                    $"INSERT INTO tours (tourname, tourdescription, routeinformation, tourdistance, tourfrom, tourto, tourimage) " +
                    $"VALUES ('getTourThatExists','nametest','nametest','nametest','nametest','nametest','nametest')"
            };

            cmd.ExecuteNonQuery();
            con.Close();

            var returnTour = database.GetTour("getTourThatExists");

            using var conDel = new NpgsqlConnection(testConfig.DatabaseSource);
            conDel.Open();

            using var cmdDel = new NpgsqlCommand
            {
                Connection = conDel,
                CommandText = $"DELETE FROM tours WHERE " +
                              $"tourname = 'getTourThatExists'"
            };
            var status = cmdDel.ExecuteNonQuery();

            conDel.Close();

            var tmpTourContainer = new TourEntry("getTourThatExists", "nametest", 
                "nametest","nametest", "nametest",
                "nametest", "nametest");

            Assert.AreEqual(tmpTourContainer.TourName, returnTour.TourName);
            Assert.AreEqual(tmpTourContainer.RouteInformation, returnTour.RouteInformation);
            Assert.AreEqual(tmpTourContainer.TourDescription, returnTour.TourDescription);
            Assert.AreEqual(tmpTourContainer.TourDistance, returnTour.TourDistance);
            Assert.AreEqual(tmpTourContainer.TourFrom, returnTour.TourFrom);
            Assert.AreEqual(tmpTourContainer.TourTo, returnTour.TourTo);
            Assert.AreEqual(tmpTourContainer.TourImage, returnTour.TourImage);

        }

        [Test]
        public void GetImportJsonTestShouldGetJson()
        {
            var testConfig = new TestConfigFetcher();
            var iO = new IO(testConfig.ImageSource);
            string json = null;

            using (StreamReader r = new StreamReader(testConfig.ConfigFileLocation))
            {
                json = r.ReadToEnd();
            }

            var tmpJsonString = iO.GetImportJson(testConfig.ConfigFileLocation);

            Assert.AreEqual(json, tmpJsonString);
        }

        [Test]
        public void GetImportJsonTestShouldReturnError()
        {
            var testConfig = new TestConfigFetcher();
            var iO = new IO(testConfig.ImageSource);


            var tmpJsonString = iO.GetImportJson(testConfig.ConfigFileLocation + testConfig.ConfigFileLocation);

            Assert.AreEqual("ERROR", tmpJsonString);
        }

        [Test]
        public void SaveImageTestShouldSaveImage()
        {
            var testConfig = new TestConfigFetcher();
            var iO = new IO(testConfig.ImageSource);


            iO.SaveNewTourImage(testConfig.ImageSource + @"\SaveImageTestShouldSaveImage.jpg", new byte[0]);

            var doesImageExist = File.Exists(testConfig.ImageSource + @"\SaveImageTestShouldSaveImage.jpg");

            File.Delete(testConfig.ImageSource + @"\SaveImageTestShouldSaveImage.jpg");

            Assert.AreEqual(true, doesImageExist);

        }

        [Test]
        public void SaveImageTestShouldOverrideImage()
        {
            var testConfig = new TestConfigFetcher();
            var iO = new IO(testConfig.ImageSource);

            File.Delete(testConfig.ImageSource + @"\testImage.jpg");
            File.Copy(testConfig.ImageSource + @"\testImageTemplate.jpg", testConfig.ImageSource + @"\testImage.jpg");


            Assert.AreEqual(0, iO.SaveNewTourImage(testConfig.ImageSource + @"\testImage.jpg", new byte[0]));
            Assert.AreEqual(new byte[0], File.ReadAllBytes(testConfig.ImageSource + @"\testImage.jpg"));


            File.Delete(testConfig.ImageSource + @"\testImage.jpg");
        }

        [Test]
        public void SavePdfTestShouldReturn0()
        {
            var testConfig = new TestConfigFetcher();
            var iO = new IO(testConfig.ImageSource);


            var status = iO.SavePdf(new PdfDocument(testConfig.ImageSource + @"\testPdfTemplate.pdf"), testConfig.ImageSource + @"\SavePdf.pdf");

            var doesPdfExist = File.Exists(testConfig.ImageSource + @"\SavePdf.pdf");

            File.Delete(testConfig.ImageSource + @"\SavePdf.pdf");

            Assert.AreEqual(0, status);
            Assert.AreEqual(true, doesPdfExist);

        }


        [Test]
        public void SavePdfTestShouldReturnNegative1()
        {
            var testConfig = new TestConfigFetcher();
            var iO = new IO(testConfig.ImageSource);


            var status = iO.SavePdf(new PdfDocument(testConfig.ImageSource + @"\testPdfTemplate.pdf"), 
                testConfig.ImageSource + @"\SavePdf.jpg");


            File.Delete(testConfig.ImageSource + @"\SavePdf.jpg");

            Assert.AreEqual(-1, status);
        }

    }
}