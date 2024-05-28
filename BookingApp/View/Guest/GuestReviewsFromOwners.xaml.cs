using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Service;
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
    /// Interaction logic for GuestReviewsFromOwners.xaml
    /// </summary>
    public partial class GuestReviewsFromOwners : Page
    {
        private UserDTO _userDTO;
        public GuestReviewsFromOwners(UserDTO userDTO)
        {
            InitializeComponent();
            _userDTO = userDTO;
            DataContext = new GuestReservationsViewModel(_userDTO);
        }
    }
    public class AccommodationIdToOwnerNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int accommodationId)
            {
                int ownerId = GetOwnerIdByAccommodationId(accommodationId);
                //int ownerId  = 
                return GetOwnerNameById(ownerId);
            }
            return "Unknown Owner";  // Default if not found or if the input value is incorrect
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();  // This conversion is one-way only
        }

        private int GetOwnerIdByAccommodationId(int accommodationId)
        {
            AccommodationRepository accomodationRepository = new AccommodationRepository();
            AccommodationService accommodationService = new AccommodationService(accomodationRepository);
            var accommodation = accommodationService.GetById(accommodationId);
            return accommodation.OwnerId;
        }

        private string GetOwnerNameById(int ownerId)
        {
            UserRepository userRepository = new UserRepository();
            UserService userService = new UserService(userRepository);
            var user = userService.GetById(ownerId);
            return user.Username;
        }
    }
    public class AccommodationIdToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int accommodationId)
            {
                return GetAccommodationNameById(accommodationId);
            }
            return "Unknown Accommodation";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // This conversion is one-way only
        }

        private string GetAccommodationNameById(int accommodationId)
        {
            AccommodationRepository accomodationRepository = new AccommodationRepository();
            AccommodationService accommodationService = new AccommodationService(accomodationRepository);
            var accommodation = accommodationService.GetById(accommodationId);
            return accommodation.Name; 
        }
    }
}
