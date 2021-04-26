using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SWE2_TOURPLANNER.Model
{
    public class LogEntry : INotifyPropertyChanged
    {
        private string _tourName;
        private string _logDate;
        private int _totalTime;
        private int _distance;
        private int _elevation;
        private string _avgSpeed;
        private int _bpm;
        private string _rating;
        private string _report;
        private string _usedSupplies;
        private string _tourmates;

        public LogEntry(string tourName, string logDate, int totalTime, int distance,
                        int elevation, string avgSpeed, int bpm, string rating,
                        string report, string usedSupplies, string tourmates)
        {
            this._tourName = tourName;
            this._logDate = logDate;
            this._totalTime = totalTime;
            this._distance = distance;
            this._elevation = elevation;
            this._avgSpeed = avgSpeed;
            this._bpm = bpm;
            this._rating = rating;
            this._report = report;
            this._usedSupplies = usedSupplies;
            this._tourmates = tourmates;
        }

        public string TourName
        {
            get => this._tourName;
            set
            {
                this._tourName = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public string LogDate
        {
            get => this._logDate;
            set
            {
                this._logDate = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public int TotalTime
        {
            get => this._totalTime;
            set
            {
                this._totalTime = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public int Distance
        {
            get => this._distance;
            set
            {
                this._distance = value;
                this.OnPropertyChanged(); // using CallerMemberName
            }
        }
        public int Elevation
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
        public int BPM
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


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
