using BookingApp.DTO;
using BookingApp.ViewModel.Guest;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for DateChangeRequestsPage.xaml
    /// </summary>
    public partial class DateChangeRequestsPage : Page
    {
        public static DateChangeRequestsPage Instance;
        UserDTO _loggedInGuest = GuestMainViewWindow.LoggedInGuest;
        public static GuestReservationsViewModel _guestReservationsViewModel;
        public DateChangeRequestsPage()
        {
            InitializeComponent();
            _guestReservationsViewModel = new GuestReservationsViewModel(_loggedInGuest);
            DataContext = _guestReservationsViewModel;
            if(Instance == null)
            {
                Instance = this;
            }
        }
    }
    public class AccommodationReservationIdToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            // Assuming you have a way to access your list of reservations
            var reservations = GuestReservationsViewModel._myReservations;
            var reservation = reservations.FirstOrDefault(r => r.AccommodationId == (int)value);
            var accommodations = GuestMainViewModel._accommodationsDTO;
            var accommodation = accommodations.FirstOrDefault(a => a.Id == reservation.AccommodationId);

            return accommodation?.Name; // Assuming the reservation object has an AccommodationName property
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
