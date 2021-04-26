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
        private string _tourFrom;
        private string _tourTo;
        private string _tourImage;

        public TourEntry(string tourName, string tourDescription, string routeInformation, string tourDistance, string tourFrom, string tourTo, string tourImage)
        {
            this._tourName = tourName;
            this._tourDescription = tourDescription;
            this._routeInformation = routeInformation;
            this._tourDistance = tourDistance;
            this._tourFrom = tourFrom;
            this._tourTo = tourTo;
            this._tourImage = tourImage;
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

        public string TourFrom
        {
            get => this._tourFrom;
            set
            {
                this._tourFrom = value;
                this.OnPropertyChanged(nameof(TourFrom));
            }
        }

        public string TourTo
        {
            get => this._tourTo;
            set
            {
                this._tourTo = value;
                this.OnPropertyChanged(nameof(TourTo));
            }
        }

        public string TourImage
        {
            get => this._tourImage;
            set
            {
                this._tourImage = value;
                this.OnPropertyChanged(nameof(TourImage));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
