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

        private string _tourName { get; set; }
        private string _tourDescription { get; set; }
        private string _routeInformation { get; set; }
        private string _tourDistance { get; set; }
        private string _tourFrom { get; set; }
        private string _tourTo { get; set; }

        private static string _currentlySelectedTour { get; set; }
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

        public static string CurrentlySelectedTour
        {
            get => _currentlySelectedTour;
            set => _currentlySelectedTour = value;
        }

        public RelayCommand AddCommand { get; }
        public RelayCommand DeleteCommand { get; }


        public MainViewModel()
        {
            ConfigFetcher configFetcher = ConfigFetcher.Instance;
            MapQuest test = MapQuest.Instance;
            test.GetImage("Wien", "Berlin");

            AddCommand = new RelayCommand((_) =>
            {
                Data.Add(new TourEntry(this.TourName, this.TourDescription, this.RouteInformation, this.TourDistance, this.TourFrom, this.TourTo));
                TourName = String.Empty;
                TourDescription = string.Empty;
                RouteInformation = string.Empty;
                TourDistance = string.Empty;
                TourFrom = string.Empty;
                TourTo = string.Empty;
            });

            DeleteCommand = new RelayCommand((_) =>
            {
                if (CurrentlySelectedTour != null)
                {
                    Data.Remove(Data.Single(i => i.TourName == CurrentlySelectedTour));
                }
            });


            // real data to add (not design data)
            Data.Add(new TourEntry("Gute Tour", "Eine schöne Tour", "Die Route ist hart und schwer", "Es ist sehr lang", "Afghanistan", "Berlin"));
            Data.Add(new TourEntry("Schlechte Tour", "Eine hässliche Tour", "Die Route ist leicht und leicht", "Es ist sehr kurz", "Tirol", "Kiev"));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
