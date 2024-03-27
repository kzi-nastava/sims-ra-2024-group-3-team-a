using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for ReviewsPage.xaml
    /// </summary>
    public partial class ReviewsPage : Page
    {
        public static ObservableCollection<AccommodationReservationDTO> AccommodationReservationsDTO { get; set; }

        private readonly AccommodationRepository _accommodationRepository;
        private readonly AccommodationReservationRepository _accommodationReservationRepository;
        private readonly UserRepository _userRepository;

        private UserDTO _loggedInOwner;
        private OwnerMainWindow _ownerMainWindow;

        public ReviewsPage(OwnerMainWindow ownerMainWindow)
        {
            InitializeComponent();
            DataContext = this;

            _accommodationRepository = new AccommodationRepository();
            _accommodationReservationRepository = new AccommodationReservationRepository();
            _userRepository = new UserRepository();

            AccommodationReservationsDTO = new ObservableCollection<AccommodationReservationDTO>();

            _ownerMainWindow = ownerMainWindow;
            _loggedInOwner = _ownerMainWindow.LoggedInOwner;

            _ownerMainWindow.Update();
            AccommodationReservationsDTO = OwnerMainWindow.AccommodationReservationsDTO;
        }

        private void ShowSideMenu(object sender, RoutedEventArgs e)
        {
            _ownerMainWindow.ShowSideMenu(sender, e);
        }

        private void ShowMyReviewPage(object sender, SelectionChangedEventArgs e)
        {
            if(myReviewsList.SelectedItem == null)
            {
                return;
            }

            var selectedItem = myReviewsList.SelectedItem as AccommodationReservationDTO;

            if(selectedItem.RatingDTO.OwnerCleannessRating == 0)
            {
                _ownerMainWindow.frameMain.Content = new MyReviewNotRatedPage(_ownerMainWindow, selectedItem);
                myReviewsList.SelectedItem = null;
            }
            else
            {
                _ownerMainWindow.frameMain.Content = new MyReviewRatedPage(_ownerMainWindow, selectedItem);
                myReviewsList.SelectedItem = null;
            }
        }
    }

    public class GuestIdToNameConverter : IValueConverter
    {
        private readonly UserRepository _userRepository = new UserRepository();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int guestId = (int)value;
            var guest = _userRepository.GetById(guestId);
            if (guest != null)
            {
                return guest.Username;
            }
            else
            {
                return "Unknown Guest";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string guestName = value as string;
            if (guestName != null)
            {
                var guest = _userRepository.GetByUsername(guestName);
                if (guest != null)
                {
                    return guest.Id;
                }
            }
            return DependencyProperty.UnsetValue;
        }
    }
    public class AccommodationIdToAccommodationConverter : IValueConverter
    {
        private readonly AccommodationRepository _accommodationRepository = new AccommodationRepository();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int accommodationId = (int)value;
            var accommodation = _accommodationRepository.GetById(accommodationId);
            if (accommodation != null)
            {
                return accommodation.Name;
            }
            else
            {
                return "Unknown Accommodation";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string accommodationName = value as string;
            if (accommodationName != null)
            {
                var accommodation = _accommodationRepository.GetByName(accommodationName);
                if (accommodation != null)
                {
                    return accommodation.Id;
                }
            }
            return DependencyProperty.UnsetValue;
        }
    }

    public class RatingToStarsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int rating = (int)value;
            String ratingStars = string.Empty;

            if(rating == 0)
            {
                return "NOT RATED YET";
            }

            for(int i = 0; i < rating; i++) 
            {
                ratingStars += "★";
            }
            return ratingStars;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
