using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.View.Owner;
using BookingApp.ViewModel.Tourist;
using Microsoft.Win32;
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
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for TourReviewWindow.xaml
    /// </summary>
    public partial class TourReviewWindow : Window
    {
        public static TourReviewWindow Instance;
        private TourReviewViewModel _tourReviewViewModel { get; set; }

        
        public TourReviewWindow(TourDTO tourDTO, UserDTO userDTO)
        {
            InitializeComponent();

            _tourReviewViewModel = new TourReviewViewModel(tourDTO, userDTO);
            DataContext = _tourReviewViewModel;

            if (Instance == null)
            {
                Instance = this;
            }
            if (_tourReviewViewModel.CloseAction == null)
                _tourReviewViewModel.CloseAction = new Action(this.Close);
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.PreviewKeyDown += TouristReviewWindow_PreviewKeyDown;
        }

        public static TourReviewWindow GetInstance()
        {
            return Instance;
        }

        private void TouristReviewWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F9 && (Keyboard.Modifiers & ModifierKeys.Shift) == 0 && (Keyboard.Modifiers & ModifierKeys.Control) == 0)
            {

                More.IsSubmenuOpen = true;
                Settings.Focus();
                e.Handled = true;
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


}
