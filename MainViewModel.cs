using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Ink;
using SWE2_TOURPLANNER.Annotations;
using SWE2_TOURPLANNER.Model;

namespace SWE2_TOURPLANNER
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly BusinessLayer.BusinessLayer _businessLayer = BusinessLayer.BusinessLayer.Instance;

        public static ObservableCollection<TourEntry> Data { get; }
            = new ObservableCollection<TourEntry>();

        public static ObservableCollection<LogEntry> CurrentTourLogs { get; set; }
            = new ObservableCollection<LogEntry>();

        private LogEntry _currentLog;  
        public LogEntry CurrentLog
        {
            get => _currentLog;
            set
            {
                _currentLog = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }


        private TourEntry _currentData;

        // Tour Props
        private string _tourName;
        private string _tourDescription;
        private string _routeInformation;
        private string _tourDistance;
        private string _tourFrom;
        private string _tourTo;
        private string _tourImage;

        // Tour Option Props
        private string _routeType;

        // Log Props
        //private DateTime _logDate { get; set; }
        private string _totalTime;
        private string _distance;
        private string _elevation;
        private string _avgSpeed;
        private string _bpm;
        private string _rating;
        private string _report;
        private string _usedSupplies;
        private string _tourmates;

        // Edit Props

        private string _editTourName;
        private string _editTourDescription;
        private string _editRouteInformation;
        private string _editTourDistance;
        private string _editTourFrom;
        private string _editTourTo;
        private string _editTourImage;
        private string _editRouteType;

        // Edit Log Props
        //private DateTime _logDate { get; set; }
        private string _editTotalTime;
        private string _editDistance;
        private string _editElevation;
        private string _editAvgSpeed;
        private string _editBpm;
        private string _editRating;
        private string _editReport;
        private string _editUsedSupplies;
        private string _editTourmates;

        // UI Bindings list and search
        private string _searchText;
        private TourEntry _selectedTourListItem;
        private LogEntry _selectedLogListItem;

        // other Props
        private StrokeCollection _inkStroke = new StrokeCollection();

        public StrokeCollection InkStroke
        {
            get => this._inkStroke;
            set
            {
                this._inkStroke = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }

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
        public string Bpm
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

        public string EditTourName
        {
            get => this._editTourName;
            set
            {
                this._editTourName = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public string EditTourDescription
        {
            get => this._editTourDescription;
            set
            {
                this._editTourDescription = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public string EditRouteInformation
        {
            get => this._editRouteInformation;
            set
            {
                this._editRouteInformation = value;
                this.OnPropertyChanged();
            }
        }
        public string EditTourDistance
        {
            get => this._editTourDistance;
            set
            {
                this._editTourDistance = value;
                this.OnPropertyChanged();
            }
        }
        public string EditTourFrom
        {
            get => this._editTourFrom;
            set
            {
                this._editTourFrom = value;
                this.OnPropertyChanged();
            }
        }
        public string EditTourTo
        {
            get => this._editTourTo;
            set
            {
                this._editTourTo = value;
                this.OnPropertyChanged();
            }
        }
        public string EditTourImage
        {
            get => this._editTourImage;
            set
            {
                this._editTourImage = value;
                this.OnPropertyChanged();
            }
        }
        public string EditRouteType
        {
            get => this._editRouteType;
            set
            {
                this._editRouteType = value;
                this.OnPropertyChanged();
            }
        }


        public string EditTotalTime
        {
            get => this._editTotalTime;
            set
            {
                this._editTotalTime = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public string EditDistance
        {
            get => this._editDistance;
            set
            {
                this._editDistance = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public string EditElevation
        {
            get => this._editElevation;
            set
            {
                this._editElevation = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public string EditAvgSpeed
        {
            get => this._editAvgSpeed;
            set
            {
                this._editAvgSpeed = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public string EditBpm
        {
            get => this._editBpm;
            set
            {
                this._editBpm = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public string EditRating
        {
            get => this._editRating;
            set
            {
                this._editRating = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public string EditReport
        {
            get => this._editReport;
            set
            {
                this._editReport = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public string EditUsedSupplies
        {
            get => this._editUsedSupplies;
            set
            {
                this._editUsedSupplies = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public string EditTourmates
        {
            get => this._editTourmates;
            set
            {
                this._editTourmates = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }

        public TourEntry CurrentData
        {
            get => _currentData;
            set
            {
                _currentData = value;
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

        public string CurrentlySelectedTour { get; set; }

        public DateTime CurrentlySelectedLog { get; set; }

        public RelayCommand AddTourCommand { get; }
        public RelayCommand DeleteTourCommand { get; }
        public RelayCommand EditTourCommand { get; }
        public RelayCommand EditLogCommand { get; }

        public RelayCommand AddLogCommand { get; }
        public RelayCommand DeleteLogCommand { get; }

        public RelayCommand ExportTourAsPdfCommand { get; }
        public RelayCommand ExportToursAsJsonCommand { get; }
        public RelayCommand ImportToursCommand { get; }

        public RelayCommand ClearStrokes { get; }
        public RelayCommand SaveStrokes { get; }


        public MainViewModel()
        {
            foreach (var item in _businessLayer.GetAllTours())
            {
                Data.Add(new TourEntry(item.TourName, item.TourDescription, item.RouteInformation,
                    item.TourDistance, item.TourFrom, item.TourTo, item.TourImage));
            }

            ClearStrokes = new RelayCommand((_) =>
            {
                InkStroke = new StrokeCollection();
            });

            SaveStrokes = new RelayCommand((_) =>
            {
                if (CurrentData.TourName != null)
                {
                    CurrentData.TourImage =
                        _businessLayer.SaveStrokes(CurrentData.TourName, CurrentData.TourImage, InkStroke);
                    InkStroke = new StrokeCollection();
                }
            });

            AddTourCommand = new RelayCommand((_) =>
            {
                if (this.TourName == null || this.TourDescription == null ||
                    this.TourFrom == null || this.TourTo == null)
                {
                    MessageBox.Show("Please fill out all boxes!");
                }

                else if (_businessLayer.DoLocationsExist(TourFrom, TourTo) == -1)
                {
                    MessageBox.Show("One or more locations do not exist!");
                }

                else if (_businessLayer.DoesTourExist(TourName) == -1)
                {
                    MessageBox.Show("Tour does already exist!");
                }

                else if (TourFrom.ToLower() == TourTo.ToLower())
                {
                    MessageBox.Show("Start can not be same location as finish!");
                }

                else
                {
                    MapQuestDataHelper tmpDc = _businessLayer.GetMapQuestInfo(TourFrom, TourTo, RouteType);

                    this.RouteInformation = $"Tour length: {tmpDc.Distance}\r\n" +
                                            $"Approx. time to complete: {tmpDc.ApproxTime}\r\n" +
                                            $"Tolls: {tmpDc.HasTollRoad}\r\n" +
                                            $"Bridges: {tmpDc.HasBridge}\r\n" +
                                            $"Ferries: {tmpDc.HasFerry}\r\n" +
                                            $"Highways: {tmpDc.HasHighway}\r\n" +
                                            $"Tunnels: {tmpDc.HasTunnel}\r\n" +
                                            $"Used sessionID: {tmpDc.SessionId}";

                    Data.Add(new TourEntry(TourName, TourDescription, RouteInformation,
                        tmpDc.Distance, TourFrom, TourTo, tmpDc.TourImage));

                    _businessLayer.AddTour(TourName, TourDescription, RouteInformation,
                        tmpDc.Distance, TourFrom, TourTo, tmpDc.TourImage);

                    SearchText = "";

                    TourName = string.Empty;
                    TourDescription = string.Empty;
                    RouteInformation = string.Empty;
                    TourDistance = string.Empty;
                    RouteType = "fastest";
                    TourFrom = string.Empty;
                    TourTo = string.Empty;
                }
            });

            AddLogCommand = new RelayCommand((_) =>
            {
                if (this.TotalTime == null || this.Distance == null ||
                    this.Elevation == null || this.AvgSpeed == null ||
                    this.Bpm == null || this.Report == null || this.UsedSupplies == null || this.Tourmates == null)
                {
                    MessageBox.Show("Please fill out all boxes!");
                }
                else
                {

                    if (CurrentlySelectedTour != null)
                    {
                        var tmpLogDate = DateTime.Now;

                        CurrentTourLogs.Add(new LogEntry(CurrentlySelectedTour, tmpLogDate,
                            int.Parse(TotalTime), int.Parse(Distance),
                            int.Parse(Elevation), AvgSpeed,
                            int.Parse(Bpm), Rating, Report,
                            UsedSupplies, Tourmates));

                        //DatabaseHandler tmpDatabaseHandler = DatabaseHandler.Instance;
                        _businessLayer.AddLog(CurrentlySelectedTour, tmpLogDate,
                            TotalTime, Distance,
                            Elevation, AvgSpeed,
                            Bpm, Rating, Report,
                            UsedSupplies, Tourmates);



                        TotalTime = string.Empty;
                        Distance = string.Empty;
                        Elevation = string.Empty;
                        AvgSpeed = string.Empty;
                        Bpm = string.Empty;
                        Report = string.Empty;
                        UsedSupplies = string.Empty;
                        Tourmates = string.Empty;
                    }
                }
            });

            DeleteLogCommand = new RelayCommand((_) =>
            {
                if (CurrentLog != null && CurrentlySelectedTour != null)
                {
                    //DatabaseHandler tmpDatabaseHandler = DatabaseHandler.Instance;
                    _businessLayer.DeleteLog(CurrentLog.TourName, CurrentLog.LogDate,
                        CurrentLog.TotalTime.ToString(), CurrentLog.Distance.ToString(),
                        CurrentLog.Elevation.ToString(), CurrentLog.AvgSpeed,
                        CurrentLog.BPM.ToString(), CurrentLog.Rating, CurrentLog.Report,
                        CurrentLog.UsedSupplies, CurrentLog.Tourmates);
                    CurrentLog = null;
                    CurrentTourLogs.Remove(CurrentTourLogs.Single(i => i.LogDate == CurrentlySelectedLog));
                }
            });

            DeleteTourCommand = new RelayCommand((_) =>
            {
                if (CurrentlySelectedTour != null && Data.Any(i => i.TourName == CurrentlySelectedTour))
                {
                    //DatabaseHandler tmpDatabaseHandler = DatabaseHandler.Instance;
                    _businessLayer.DeleteTour(CurrentlySelectedTour);
                    CurrentData = null;
                    CurrentTourLogs.Clear();
                    Data.Remove(Data.Single(i => i.TourName == CurrentlySelectedTour));
                }
            });

            EditTourCommand = new RelayCommand((_) =>
            {
                if (EditTourDescription == string.Empty ||
                    EditTourFrom == string.Empty || EditTourTo == string.Empty)
                {
                    MessageBox.Show("Please fill out all boxes!");
                }

                else if (_businessLayer.DoLocationsExist(EditTourFrom, EditTourTo) == -1)
                {
                    MessageBox.Show("One or more locations do not exist!");
                }

                else if (EditTourFrom.ToLower() == EditTourTo.ToLower())
                {
                    MessageBox.Show("Start can not be same location as finish!");
                }

                else
                {

                    if (CurrentlySelectedTour != null && Data.Any(i => i.TourName == CurrentlySelectedTour))
                    {
                        _businessLayer.DeleteOnlyTour(CurrentlySelectedTour);
                        CurrentData = null;
                        Data.Remove(Data.Single(i => i.TourName == CurrentlySelectedTour));

                        MapQuestDataHelper tmpDc = _businessLayer.GetMapQuestInfo(EditTourFrom, EditTourTo, EditRouteType);

                        this.EditRouteInformation = $"Tour length: {tmpDc.Distance}\r\n" +
                                                $"Approx. time to complete: {tmpDc.ApproxTime}\r\n" +
                                                $"Tolls: {tmpDc.HasTollRoad}\r\n" +
                                                $"Bridges: {tmpDc.HasBridge}\r\n" +
                                                $"Ferries: {tmpDc.HasFerry}\r\n" +
                                                $"Highways: {tmpDc.HasHighway}\r\n" +
                                                $"Tunnels: {tmpDc.HasTunnel}\r\n" +
                                                $"Used sessionID: {tmpDc.SessionId}";

                        Data.Add(new TourEntry(CurrentlySelectedTour, EditTourDescription, EditRouteInformation,
                            tmpDc.Distance, EditTourFrom, EditTourTo, tmpDc.TourImage));

                        _businessLayer.AddTour(CurrentlySelectedTour, EditTourDescription, EditRouteInformation,
                            tmpDc.Distance, EditTourFrom, EditTourTo, tmpDc.TourImage);

                        SearchText = "";
                    }
                }
            });

            EditLogCommand = new RelayCommand((_) =>
            {
                if (EditTotalTime == string.Empty || EditDistance == string.Empty 
                    || EditElevation == string.Empty || EditAvgSpeed == string.Empty || 
                    EditBpm == string.Empty || EditRating == string.Empty || EditReport == string.Empty 
                    || EditUsedSupplies == String.Empty || EditTourmates == string.Empty)
                {
                    MessageBox.Show("Please fill out all boxes!");
                }

                else
                {

                    if (CurrentLog != null && CurrentlySelectedTour != null)
                    {
                        var tmpDateSave = CurrentLog.LogDate; 

                        _businessLayer.DeleteLog(CurrentLog.TourName, CurrentLog.LogDate,
                            CurrentLog.TotalTime.ToString(), CurrentLog.Distance.ToString(),
                            CurrentLog.Elevation.ToString(), CurrentLog.AvgSpeed,
                            CurrentLog.BPM.ToString(), CurrentLog.Rating, CurrentLog.Report,
                            CurrentLog.UsedSupplies, CurrentLog.Tourmates);
                        CurrentLog = null;
                        CurrentTourLogs.Remove(CurrentTourLogs.Single(i => i.LogDate == CurrentlySelectedLog));


                        CurrentTourLogs.Add(new LogEntry(CurrentlySelectedTour, tmpDateSave,
                            int.Parse(EditTotalTime), int.Parse(EditDistance),
                            int.Parse(EditElevation), EditAvgSpeed,
                            int.Parse(EditBpm), EditRating, EditReport,
                            EditUsedSupplies, EditTourmates));

                        _businessLayer.AddLog(CurrentlySelectedTour, tmpDateSave,
                            EditTotalTime, EditDistance,
                            EditElevation, EditAvgSpeed,
                            EditBpm, EditRating, EditReport,
                            EditUsedSupplies, EditTourmates);
                    }


                }
            });

            ExportTourAsPdfCommand = new RelayCommand((_) =>
            {
                if (CurrentlySelectedTour != null)
                {
                    _businessLayer.ExportTourAsPdf(CurrentlySelectedTour);
                }
            });

            ExportToursAsJsonCommand = new RelayCommand((_) =>
            {
                if (CurrentlySelectedTour != null)
                {
                    _businessLayer.ExportToursAsJson();
                }
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

        private void SearchTextProcessing(string searchText)
        {
            Data.Clear();

            foreach (var item in _businessLayer.GetToursContainingString(searchText))
            {
                Data.Add(new TourEntry(item.TourName, item.TourDescription, item.RouteInformation,
                    item.TourDistance, item.TourFrom, item.TourTo,
                    item.TourImage));
            }

        }

        private void SelectedTourListItemProcessing(TourEntry selectedTourEntry)
        {
            if (selectedTourEntry != null)
            {
                CurrentLog = null;
                CurrentlySelectedTour = selectedTourEntry.TourName;
                CurrentData = selectedTourEntry;

                EditTourName = selectedTourEntry.TourName;
                EditTourDescription = selectedTourEntry.TourDescription;
                EditRouteInformation = selectedTourEntry.RouteInformation;
                EditTourDistance = selectedTourEntry.TourDistance;
                EditTourFrom = selectedTourEntry.TourFrom;
                EditTourTo = selectedTourEntry.TourTo;
                EditTourImage = selectedTourEntry.TourImage;

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

        private void SelectedLogListItemProcessing(LogEntry selectedLogEntry)
        {
            if (selectedLogEntry != null)
            {
                CurrentlySelectedLog = selectedLogEntry.LogDate;

                EditTotalTime = selectedLogEntry.TotalTime.ToString();
                EditDistance = selectedLogEntry.Distance.ToString();
                EditElevation = selectedLogEntry.Elevation.ToString();
                EditAvgSpeed = selectedLogEntry.AvgSpeed;
                EditBpm = selectedLogEntry.BPM.ToString();
                EditRating = selectedLogEntry.Rating;
                EditReport = selectedLogEntry.Report;
                EditUsedSupplies = selectedLogEntry.UsedSupplies;
                EditTourmates = selectedLogEntry.Tourmates;


                CurrentLog = new LogEntry(selectedLogEntry.TourName, selectedLogEntry.LogDate,
                                                selectedLogEntry.TotalTime, selectedLogEntry.Distance, selectedLogEntry.Elevation,
                                                selectedLogEntry.AvgSpeed, selectedLogEntry.BPM, selectedLogEntry.Rating, selectedLogEntry.Report,
                                                selectedLogEntry.UsedSupplies, selectedLogEntry.Tourmates);
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
