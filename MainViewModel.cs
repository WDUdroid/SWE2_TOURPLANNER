using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using SWE2_TOURPLANNER.Annotations;
using SWE2_TOURPLANNER.DataAccessLayer;
using SWE2_TOURPLANNER.Model;
using SWE2_TOURPLANNER.Services;

namespace SWE2_TOURPLANNER
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public static ObservableCollection<TourEntry> Data { get; }
            = new ObservableCollection<TourEntry>();

        public static ObservableCollection<TourEntry> CurrentData { get; }
            = new ObservableCollection<TourEntry>();

        public static ObservableCollection<LogEntry> CurrentTourLogs { get; set; }
            = new ObservableCollection<LogEntry>();

        public static ObservableCollection<LogEntry> CurrentLog { get; }
            = new ObservableCollection<LogEntry>();

        // Tour Props
        private string _tourName { get; set; }
        private string _tourDescription { get; set; }
        private string _routeInformation { get; set; }
        private string _tourDistance { get; set; }
        private string _tourFrom { get; set; }
        private string _tourTo { get; set; }
        private string _tourImage { get; set; }

        // Log Props
        //private DateTime _logDate { get; set; }
        private string _totalTime { get; set; }
        private string _distance { get; set; }
        private string _elevation { get; set; }
        private string _avgSpeed { get; set; }
        private string _bpm { get; set; }
        private string _rating { get; set; }
        private string _report { get; set; }
        private string _usedSupplies { get; set; }
        private string _tourmates { get; set; }

        // other Props
        private static string _currentlySelectedTour { get; set; }
        private static DateTime _currentlySelectedLog { get; set; }

        // Tour Getter/Setter
        public string TourName
        {
            get => this._tourName;
            set
            {
                this._tourName = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public string TourDescription
        {
            get => this._tourDescription;
            set
            {
                this._tourDescription = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }

        public string RouteInformation
        {
            get => this._routeInformation;
            set
            {
                this._routeInformation = value;
                this.OnPropertyChanged();
            }
        }

        public string TourDistance
        {
            get => this._tourDistance;
            set
            {
                this._tourDistance = value;
                this.OnPropertyChanged();
            }
        }

        public string TourFrom
        {
            get => this._tourFrom;
            set
            {
                this._tourFrom = value;
                this.OnPropertyChanged();
            }
        }

        public string TourTo
        {
            get => this._tourTo;
            set
            {
                this._tourTo = value;
                this.OnPropertyChanged();
            }
        }

        public string TourImage
        {
            get => this._tourImage;
            set
            {
                this._tourImage = value;
                this.OnPropertyChanged();
            }
        }


        public string TotalTime
        {
            get => this._totalTime;
            set
            {
                this._totalTime = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public string Distance
        {
            get => this._distance;
            set
            {
                this._distance = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public string Elevation
        {
            get => this._elevation;
            set
            {
                this._elevation = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public string AvgSpeed
        {
            get => this._avgSpeed;
            set
            {
                this._avgSpeed = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public string BPM
        {
            get => this._bpm;
            set
            {
                this._bpm = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public string Rating
        {
            get => this._rating;
            set
            {
                this._rating = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public string Report
        {
            get => this._report;
            set
            {
                this._report = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public string UsedSupplies
        {
            get => this._usedSupplies;
            set
            {
                this._usedSupplies = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public string Tourmates
        {
            get => this._tourmates;
            set
            {
                this._tourmates = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }

        public static string CurrentlySelectedTour
        {
            get => _currentlySelectedTour;
            set => _currentlySelectedTour = value;
        }
        public static DateTime CurrentlySelectedLog
        {
            get => _currentlySelectedLog;
            set => _currentlySelectedLog = value;
        }

        public RelayCommand AddTourCommand { get; }
        public RelayCommand DeleteTourCommand { get; }

        public RelayCommand AddLogCommand { get; }
        public RelayCommand DeleteLogCommand { get; }


        public MainViewModel()
        {
            ConfigFetcher configFetcher = ConfigFetcher.Instance;
            MapQuest GetMap = MapQuest.Instance;

            DatabaseHandler tmpDatabaseHandler = DatabaseHandler.Instance;

            foreach (var item in tmpDatabaseHandler.GetToursFromDb())
            {
                Data.Add(new TourEntry(item.TourName, item.TourDescription, item.RouteInformation,
                    item.TourDistance, item.TourFrom, item.TourTo, item.TourImage));
            }

            AddTourCommand = new RelayCommand((_) =>
            {
                string tmpImageString = GetMap.GetImage(this.TourFrom, this.TourTo);
                Data.Add(new TourEntry(this.TourName, this.TourDescription, this.RouteInformation, this.TourDistance, this.TourFrom, this.TourTo, tmpImageString));

                DatabaseHandler tmpDatabaseHandler = DatabaseHandler.Instance;
                tmpDatabaseHandler.AddTourToDb(this.TourName, this.TourDescription, this.RouteInformation, this.TourDistance, this.TourFrom, this.TourTo, tmpImageString);

                TourName = String.Empty;
                TourDescription = string.Empty;
                RouteInformation = string.Empty;
                TourDistance = string.Empty;
                TourFrom = string.Empty;
                TourTo = string.Empty;
            });

            AddLogCommand = new RelayCommand((_) =>
            {
                if (CurrentlySelectedTour != null)
                {
                    var tmpLogDate = DateTime.Now;

                    CurrentTourLogs.Add(new LogEntry(CurrentlySelectedTour, tmpLogDate,
                        int.Parse(this.TotalTime), int.Parse(this.Distance),
                        int.Parse(this.Elevation), this.AvgSpeed,
                        int.Parse(this.BPM), this.Rating, this.Report,
                        this.UsedSupplies, this.Tourmates));

                    DatabaseHandler tmpDatabaseHandler = DatabaseHandler.Instance;
                    tmpDatabaseHandler.AddLogToTour(CurrentlySelectedTour, tmpLogDate,
                        int.Parse(this.TotalTime), int.Parse(this.Distance),
                        int.Parse(this.Elevation), this.AvgSpeed,
                        int.Parse(this.BPM), this.Rating, this.Report,
                        this.UsedSupplies, this.Tourmates);
                }

                TotalTime = string.Empty;
                Distance = string.Empty;
                Elevation = string.Empty;
                AvgSpeed = string.Empty;
                BPM = string.Empty;
                Rating = string.Empty;
                Report = string.Empty;
                UsedSupplies = string.Empty;
                Tourmates = string.Empty;

            });

            DeleteLogCommand = new RelayCommand((_) =>
            {
                if (CurrentlySelectedLog != null && CurrentlySelectedTour != null)
                {
                    DatabaseHandler tmpDatabaseHandler = DatabaseHandler.Instance;
                    tmpDatabaseHandler.DeleteLogFromTour(CurrentLog[0].TourName, CurrentLog[0].LogDate,
                        int.Parse(CurrentLog[0].TotalTime.ToString()), int.Parse(CurrentLog[0].Distance.ToString()),
                        int.Parse(CurrentLog[0].Elevation.ToString()), CurrentLog[0].AvgSpeed,
                        int.Parse(CurrentLog[0].BPM.ToString()), CurrentLog[0].Rating, CurrentLog[0].Report,
                        CurrentLog[0].UsedSupplies, CurrentLog[0].Tourmates);
                    CurrentLog.Clear();
                    CurrentTourLogs.Remove(CurrentTourLogs.Single(i => i.LogDate == CurrentlySelectedLog));
                }
            });

            DeleteTourCommand = new RelayCommand((_) =>
            {
                if (CurrentlySelectedTour != null)
                {
                    DatabaseHandler tmpDatabaseHandler = DatabaseHandler.Instance;
                    tmpDatabaseHandler.DeleteTourFromDb(CurrentlySelectedTour);
                    tmpDatabaseHandler.DeleteAllLogsFromTour(CurrentlySelectedTour);
                    CurrentData.Clear();
                    Data.Remove(Data.Single(i => i.TourName == CurrentlySelectedTour));
                }
            });


            // real data to add (not design data)
            //Data.Add(new TourEntry("Gute Tour", "Eine schöne Tour", "Die Route ist hart und schwer", "Es ist sehr lang", "Afghanistan", "Berlin", "D:\\Images\\maxresdefault.jpg"));
            //Data.Add(new TourEntry("Schlechte Tour", "Eine hässliche Tour", "Die Route ist leicht und leicht", "Es ist sehr kurz", "Tirol", "Kiev", "D:\\Images\\maxresdefault.jpg"));
            //CurrentTourLogs.Add(new LogEntry("Gute Tour",1, 1, 1, "very fast", 1, "nice tour", "D:\\Images\\maxresdefault.jpg", "a lot", "Treeman"));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
