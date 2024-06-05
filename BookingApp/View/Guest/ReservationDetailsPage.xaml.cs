using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.ViewModel.Guest;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for ReservationDetailsPage.xaml
    /// </summary>
    public partial class ReservationDetailsPage : Page
    {
        public static ReservationDetailsPage Instance;
        public ReservationDetailsPage(AccommodationDTO accommodationDTO, UserDTO userDTO, DateOnly begin, DateOnly end)
        {
            InitializeComponent();
            DataContext = new GuestReservationDetailsViewModel(accommodationDTO, userDTO, begin, end);
            //ToggleSwitch.GetBindingExpression(VisibilityProperty).UpdateTarget();
            //ToggleSwitch.Visibility = GuestReservationDetailsViewModel.IsSuper ? Visibility.Visible : Visibility.Collapsed;
            
            if (Instance == null)
            {
                Instance = this;
            }
           // Instance.ToggleSwitch.GetBindingExpression(VisibilityProperty).UpdateTarget();
        }
    }
    /*
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                // Optionally handle a converter parameter to invert the logic if needed
                bool invert = parameter?.ToString().ToLower() == "invert";
                return boolValue != invert ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Converting from Visibility to Boolean is not supported.");
        }
    }
    */
}