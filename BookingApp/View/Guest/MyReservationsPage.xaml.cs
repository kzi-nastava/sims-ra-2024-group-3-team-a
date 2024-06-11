using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Model;
using BookingApp.Repository.Interfaces;
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
    /// Interaction logic for MyReservationsPage.xaml
    /// </summary>
    public partial class MyReservationsPage : Page
    {
        public static GuestReservationsViewModel _guestReservationsViewModel;
        public static MyReservationsPage Instance;
        public MyReservationsPage(UserDTO loggedInGuest)
        {
            InitializeComponent();
            _guestReservationsViewModel = new GuestReservationsViewModel(loggedInGuest);
            DataContext = _guestReservationsViewModel;
            if(Instance == null)
            {
                Instance = this;
            }
        }
    }
    public class LocationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var place = value as AccommodationDTO;
            return place != null ? $"{place.PlaceDTO.City}, {place.PlaceDTO.Country}" : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class AccommodationIdToLocationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var accommodationId = (int)value;
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            AccommodationService accommodationService = new AccommodationService(accommodationRepository);
            Accommodation accommodation = accommodationService.GetById(accommodationId);
            return accommodation != null ? $"{accommodation.Place.City}, {accommodation.Place.Country}" : "Unknown location";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class AccommodationIdToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var accommodationId = (int)value;
            IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            AccommodationService accommodationService = new AccommodationService(accommodationRepository);
            Accommodation accommodation = accommodationService.GetById(accommodationId);

            if (accommodation != null && accommodation.Images.Count > 0)
            {
                try
                {
                    var imagePath = accommodation.Images[0];
                    return new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
                }
                catch (Exception)
                {
                    return null; // Handle invalid image path
                }
            }
            //return null; // Default or fallback image if needed

            // Return a default image if no images are available
            return new BitmapImage(new Uri("pack://application:,,,/Resources/Images/add.png"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
