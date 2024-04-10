using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
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
    /// Interaction logic for TourReviewWindow.xaml
    /// </summary>
    public partial class TourReviewWindow : Window
    {
        private TourReviewDTO _tourReviewDTO {  get; set; }
        private TourReviewRepository _tourReviewRepository {  get; set; }
        private TourDTO _tourDTO { get; set; }
        private UserDTO _userDTO { get; set; }
        public TourReviewWindow(TourDTO tourDTO, UserDTO userDTO)
        {
            InitializeComponent();

            _tourDTO = tourDTO;
            _userDTO = userDTO;
            _tourReviewRepository = new TourReviewRepository();
            _tourReviewDTO = new TourReviewDTO();

            DataContext = new { Tour = _tourDTO, TourReview = _tourReviewDTO };
        }

        public void RateTour_Click(object sender, RoutedEventArgs e)
        {
            _tourReviewDTO.TourId = _tourDTO.Id;
            _tourReviewDTO.TouristId = _userDTO.Id;
            _tourReviewDTO.Comment = "no comment";
            _tourReviewRepository.Save(_tourReviewDTO.ToTourReview());
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
