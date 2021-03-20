using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using SWE2_TOURPLANNER.Annotations;
using SWE2_TOURPLANNER.Model;

namespace SWE2_TOURPLANNER
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TourEntry> Data { get; }
            = new ObservableCollection<TourEntry>();

        private string _tourName { get; set; }
        private string _tourDescription { get; set; }
        private string _routeInformation { get; set; }
        private string _tourDistance { get; set; }

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

        public RelayCommand AddCommand { get; }


        private bool _isUsernameFocused;
        public bool IsUsernameFocused
        {
            get => _isUsernameFocused;
            set
            {
                // it needs to flip, else it does not execute properly, so let's reset here
                _isUsernameFocused = false;
                OnPropertyChanged();
                _isUsernameFocused = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            AddCommand = new RelayCommand((_) =>
            {
                Data.Add(new TourEntry(this.TourName,this.TourDescription, this.RouteInformation, this.TourDistance));
                TourName = String.Empty;
                TourDescription = string.Empty;
                RouteInformation = string.Empty;
                TourDistance = string.Empty;
                IsUsernameFocused = true;
            });
            IsUsernameFocused = true;

            // real data to add (not design data)
            Data.Add(new TourEntry("Gute Tour","Eine schöne Tour", "Die Route ist hart und schwer", "Es ist sehr lang"));
            Data.Add(new TourEntry("Schlechte Tour","Eine hässliche Tour", "Die Route ist leicht und leicht", "Es ist sehr kurz"));
        }








        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
