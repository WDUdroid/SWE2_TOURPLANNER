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
using SWE2_TOURPLANNER.HelperObjects;
using SWE2_TOURPLANNER.Model;
using SWE2_TOURPLANNER.Services;

namespace SWE2_TOURPLANNER
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private BusinessLayer _businessLayer = BusinessLayer.Instance;

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

        // Tour Option Props
        private string _routeType { get; set; }

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

        // UI Bindings list and search
        private string _searchText;
        private TourEntry _selectedTourListItem;
        private LogEntry _selectedLogListItem;

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

        public string RouteType
        {
            get => this._routeType;
            set
            {
                this._routeType = value;
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

        public string SearchText
        {
            get => this._searchText;
            set
            {
                this._searchText = value;
                SearchTextProcessing(_searchText);
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }

        public TourEntry SelectedTourListItem
        {
            get => this._selectedTourListItem;
            set
            {
                this._selectedTourListItem = value;
                SelectedTourListItemProcessing(_selectedTourListItem);
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }

        public LogEntry SelectedLogListItem
        {
            get => this._selectedLogListItem;
            set
            {
                this._selectedLogListItem = value;
                SelectedLogListItemProcessing(_selectedLogListItem);
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

        public RelayCommand ExportTourAsPDFCommand { get; }
        public RelayCommand ExportToursAsJSONCommand { get; }
        public RelayCommand ImportToursCommand { get; }


        public MainViewModel()
        {
            ConfigFetcher configFetcher = ConfigFetcher.Instance;
            MapQuest GetMap = MapQuest.Instance;

            DatabaseHandler tmpDatabaseHandler = DatabaseHandler.Instance;

            foreach (var item in _businessLayer.GetAllTours())
            {
                Data.Add(new TourEntry(item.TourName, item.TourDescription, item.RouteInformation,
                    item.TourDistance, item.TourFrom, item.TourTo, item.TourImage));
            }

            AddTourCommand = new RelayCommand((_) =>
            {
                MapQuestDataHelper tmpDC = _businessLayer.GetMapQuestInfo(this.TourFrom, this.TourTo, this.RouteType);

                this.RouteInformation = $"Tour length: {tmpDC.Distance}\r\n" +
                                        $"Approx. time to complete: {tmpDC.ApproxTime}\r\n" +
                                        $"Tolls: {tmpDC.HasTollRoad}\r\n" +
                                        $"Bridges: {tmpDC.HasBridge}\r\n" +
                                        $"Ferries: {tmpDC.HasFerry}\r\n" +
                                        $"Highways: {tmpDC.HasHighway}\r\n" +
                                        $"Tunnels: {tmpDC.HasTunnel}\r\n" +
                                        $"Used sessionID: {tmpDC.SessionId}";

                Data.Add(new TourEntry(this.TourName, this.TourDescription, this.RouteInformation,
                                            tmpDC.Distance, this.TourFrom, this.TourTo, tmpDC.TourImage));

                _businessLayer.AddTour(this.TourName, this.TourDescription, this.RouteInformation,
                                        tmpDC.Distance, this.TourFrom, this.TourTo, tmpDC.TourImage);

                SearchText = "";

                TourName = String.Empty;
                TourDescription = string.Empty;
                RouteInformation = string.Empty;
                TourDistance = string.Empty;
                RouteType = "fastest";
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

                    //DatabaseHandler tmpDatabaseHandler = DatabaseHandler.Instance;
                    _businessLayer.AddLog(CurrentlySelectedTour, tmpLogDate,
                        this.TotalTime, this.Distance,
                        this.Elevation, this.AvgSpeed,
                        this.BPM, this.Rating, this.Report,
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
                if (CurrentLog.Count != 0 && CurrentlySelectedTour != null)
                {
                    //DatabaseHandler tmpDatabaseHandler = DatabaseHandler.Instance;
                    _businessLayer.DeleteLog(CurrentLog[0].TourName, CurrentLog[0].LogDate,
                        CurrentLog[0].TotalTime.ToString(), CurrentLog[0].Distance.ToString(),
                        CurrentLog[0].Elevation.ToString(), CurrentLog[0].AvgSpeed,
                        CurrentLog[0].BPM.ToString(), CurrentLog[0].Rating, CurrentLog[0].Report,
                        CurrentLog[0].UsedSupplies, CurrentLog[0].Tourmates);
                    CurrentLog.Clear();
                    CurrentTourLogs.Remove(CurrentTourLogs.Single(i => i.LogDate == CurrentlySelectedLog));
                }
            });

            DeleteTourCommand = new RelayCommand((_) =>
            {
                if (CurrentlySelectedTour != null && Data.Any(i => i.TourName == CurrentlySelectedTour))
                {
                    //DatabaseHandler tmpDatabaseHandler = DatabaseHandler.Instance;
                    _businessLayer.DeleteTour(CurrentlySelectedTour);
                    CurrentData.Clear();
                    Data.Remove(Data.Single(i => i.TourName == CurrentlySelectedTour));
                }
            });

            ExportTourAsPDFCommand = new RelayCommand((_) =>
            {
                _businessLayer.ExportTourAsPDF(CurrentlySelectedTour);
            });

            ExportToursAsJSONCommand = new RelayCommand((_) =>
            {
                _businessLayer.ExportToursAsJSON();
            });

            ImportToursCommand = new RelayCommand((_) =>
            {
                _businessLayer.ImportTours();
                Data.Clear();
                foreach (var item in _businessLayer.GetAllTours())
                {
                    Data.Add(new TourEntry(item.TourName, item.TourDescription, item.RouteInformation,
                        item.TourDistance, item.TourFrom, item.TourTo, item.TourImage));
                }
            });


        }

        void SearchTextProcessing(string searchText)
        {
            Data.Clear();

            foreach (var item in _businessLayer.GetToursContainingString(searchText))
            {
                Data.Add(new TourEntry(item.TourName, item.TourDescription, item.RouteInformation,
                    item.TourDistance, item.TourFrom, item.TourTo,
                    item.TourImage));
            }

        }

        void SelectedTourListItemProcessing(TourEntry selectedTourEntry)
        {
            if (selectedTourEntry != null)
            {
                CurrentLog.Clear();
                CurrentlySelectedTour = selectedTourEntry.TourName;
                CurrentData.Clear();
                CurrentData.Add(new TourEntry(selectedTourEntry.TourName, selectedTourEntry.TourDescription,
                    selectedTourEntry.RouteInformation, selectedTourEntry.TourDistance, selectedTourEntry.TourFrom,
                    selectedTourEntry.TourTo, selectedTourEntry.TourImage));

                CurrentTourLogs.Clear();

                foreach (var item in _businessLayer.GetLogsOfTour(selectedTourEntry.TourName))
                {
                    CurrentTourLogs.Add(new LogEntry(item.TourName, item.LogDate, item.TotalTime,
                        item.Distance, item.Elevation, item.AvgSpeed,
                        item.BPM, item.Rating, item.Report, item.UsedSupplies,
                        item.Tourmates));
                }
            }
        }

        void SelectedLogListItemProcessing(LogEntry selectedLogEntry)
        {
            if (selectedLogEntry != null)
            {
                CurrentlySelectedLog = selectedLogEntry.LogDate;
                CurrentLog.Clear();
                CurrentLog.Add(new LogEntry(selectedLogEntry.TourName, selectedLogEntry.LogDate,
                                                selectedLogEntry.TotalTime, selectedLogEntry.Distance, selectedLogEntry.Elevation,
                                                selectedLogEntry.AvgSpeed, selectedLogEntry.BPM, selectedLogEntry.Rating, selectedLogEntry.Report,
                                                selectedLogEntry.UsedSupplies, selectedLogEntry.Tourmates));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
