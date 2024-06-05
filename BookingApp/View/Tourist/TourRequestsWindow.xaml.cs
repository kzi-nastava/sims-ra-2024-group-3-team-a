using BookingApp.DTO;
using BookingApp.ViewModel.Owner;
using BookingApp.ViewModel.Tourist;
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
using System.Windows.Shapes;

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for TourRequestsWindow.xaml
    /// </summary>
    public partial class TourRequestsWindow : Window
    {
        private TourRequestsViewModel _tourRequestsViewModel;
        public TourRequestsWindow(UserDTO loggedInUser)
        {

            _tourRequestsViewModel = new TourRequestsViewModel(loggedInUser);
            InitializeComponent();
            DataContext = _tourRequestsViewModel;
            if (_tourRequestsViewModel.CloseAction == null)
                _tourRequestsViewModel.CloseAction = new Action(this.Close);
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

        }

        
    }
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && boolValue)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;

        }
    }
}
public class RadioBoolToIntConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int integer = (int)value;
        if (integer == int.Parse(parameter.ToString()))
            return true;
        else
            return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return parameter;
    }
}