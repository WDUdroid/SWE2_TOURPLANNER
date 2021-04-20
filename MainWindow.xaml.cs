using System.Windows;
using System.Windows.Controls;
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
                MainViewModel.CurrentlySelectedTour = curItem.TourName;
                MainViewModel.CurrentData.Clear();
                MainViewModel.CurrentData.Add(new TourEntry(curItem.TourName, curItem.TourDescription, curItem.RouteInformation, curItem.TourDistance, curItem.TourFrom, curItem.TourTo, curItem.TourImage));
            }
        }
    }
}
