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

        public string TourName { get; set; }
        public string TourDescription { get; set; }
        public string RouteInformation { get; set; }
        public string TourDistance { get; set; }
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
                OnPropertyChanged(nameof(TourDescription));
                OnPropertyChanged("RouteInformation");
                OnPropertyChanged("TourDistance");
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
