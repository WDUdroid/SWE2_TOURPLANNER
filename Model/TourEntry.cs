using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SWE2_TOURPLANNER.Model
{
    public class TourEntry : INotifyPropertyChanged
    {
        private string _tourName;
        private string _tourDescription;
        private string _routeInformation;
        private string _tourDistance;

        public TourEntry(string tourName, string tourDescription, string routeInformation, string tourDistance)
        {
            this._tourName = tourName;
            this._tourDescription = tourDescription;
            this._routeInformation = routeInformation;
            this._tourDistance = tourDistance;
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
                this.OnPropertyChanged(nameof(RouteInformation));
            }
        }

        public string TourDistance
        {
            get => this._tourDistance;
            set
            {
                this._tourDistance = value;
                this.OnPropertyChanged(nameof(TourDescription));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
