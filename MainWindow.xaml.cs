using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SWE2_TOURPLANNER.DataAccessLayer;
using SWE2_TOURPLANNER.Model;

namespace SWE2_TOURPLANNER
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void tourSelectListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TourEntry curItem = (TourEntry)TourSelectListBox.SelectedItem;
            if (curItem != null)
            {
                MainViewModel.CurrentLog.Clear();
                MainViewModel.CurrentlySelectedTour = curItem.TourName;
                MainViewModel.CurrentData.Clear();
                MainViewModel.CurrentData.Add(new TourEntry(curItem.TourName, curItem.TourDescription, curItem.RouteInformation, curItem.TourDistance, curItem.TourFrom, curItem.TourTo, curItem.TourImage));

                MainViewModel.CurrentTourLogs.Clear();
                DatabaseHandler tmpDatabaseHandler = DatabaseHandler.Instance;

                foreach (var item in tmpDatabaseHandler.GetLogsOfTour(curItem.TourName))
                {
                    MainViewModel.CurrentTourLogs.Add(new LogEntry(item.TourName, item.LogDate, item.TotalTime, 
                                                                        item.Distance, item.Elevation, item.AvgSpeed, 
                                                                        item.BPM, item.Rating, item.Report, item.UsedSupplies, 
                                                                        item.Tourmates));
                }
            }
        }

        private void logSelectListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LogEntry curItem = (LogEntry)LogSelectListBox.SelectedItem;
            if (curItem != null)
            {
                MainViewModel.CurrentlySelectedLog = curItem.LogDate;
                MainViewModel.CurrentLog.Clear();
                MainViewModel.CurrentLog.Add(new LogEntry(curItem.TourName, curItem.LogDate,curItem.TotalTime, 
                                            curItem.Distance, curItem.Elevation, 
                                            curItem.AvgSpeed, curItem.BPM, 
                                            curItem.Rating, curItem.Report, 
                                            curItem.UsedSupplies, curItem.Tourmates));
            }
        }

        private void searchTextChanged(object sender, TextChangedEventArgs args)
        {

            MainViewModel.Data.Clear();
            DatabaseHandler tmpDatabaseHandler = DatabaseHandler.Instance;

            foreach (var item in tmpDatabaseHandler.GetToursContainingString(SearchBox.Text))
            {
                MainViewModel.Data.Add(new TourEntry(item.TourName, item.TourDescription, item.RouteInformation,
                    item.TourDistance, item.TourFrom, item.TourTo,
                    item.TourImage));
            }

        }


        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "";
        }
    }
}
