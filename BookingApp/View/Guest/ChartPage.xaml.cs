using BookingApp.ViewModel.Guest;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for ChartPage.xaml
    /// </summary>
    public partial class ChartPage : Page
    {
        public static GuestReservationsViewModel _guestReservationsViewModel;
        public ChartPage()
        {
            InitializeComponent();
            _guestReservationsViewModel = new GuestReservationsViewModel(GuestMainViewWindow.LoggedInGuest);
            DataContext = _guestReservationsViewModel;

            var yAxis = new Axis
            {
                Title = "Number of Reservations",
                Foreground = System.Windows.Media.Brushes.Yellow,
                FontSize = 15,
                LabelFormatter = value => value.ToString("N0"),
                Separator = new LiveCharts.Wpf.Separator
                {
                    Step = 1,
                    IsEnabled = true
                }
            };

            accommodationChart.AxisY.Clear();
            accommodationChart.AxisY.Add(yAxis);
        }
    }
}
