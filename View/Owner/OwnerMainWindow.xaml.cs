using BookingApp.DTO;
using BookingApp.Model;
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
using System.Windows.Shapes;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for OwnerMainWindow.xaml
    /// </summary>
    public partial class OwnerMainWindow : Window
    {
        public static ObservableCollection<AccommodationDTO> Accommodations { get; set; }
        public static ObservableCollection<AccommodationReservationDTO> AccommodationReservations { get; set; }

        private readonly AccommodationRepository _accommodationRepository;
        private readonly AccommodationReservationRepository _accommodationReservationRepository;

        public OwnerMainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _accommodationRepository = new AccommodationRepository();
            _accommodationReservationRepository = new AccommodationReservationRepository();

            Accommodations = new ObservableCollection<AccommodationDTO>();
            AccommodationReservations = new ObservableCollection<AccommodationReservationDTO>();

            Update();
        }

        private void Update()
        {
            Accommodations.Clear();
            AccommodationReservations.Clear();
            foreach (var accommodation in _accommodationRepository.GetAll())
            {
                Accommodations.Add(new AccommodationDTO(accommodation));
            }

            foreach (var reservation in _accommodationReservationRepository.GetAll())
            {
                if(reservation.EndDate <= DateOnly.FromDateTime(DateTime.Now) && DateOnly.FromDateTime(DateTime.Now) <= reservation.EndDate.AddDays(5))
                    AccommodationReservations.Add(new AccommodationReservationDTO(reservation));
            }
        }

        private void UpdateEvent(object sender, EventArgs e)
        {
            Update();
        }

        private void ShowAddAccommodationWindow(object sender, RoutedEventArgs e)
        {
            AddAccommodationWindow addAccommodationWindow = new AddAccommodationWindow();
            addAccommodationWindow.AccommodationAdded += UpdateEvent;
            addAccommodationWindow.ShowDialog();
        }

        private void ShowRateGuestWindow(object sender, RoutedEventArgs e)
        {
            if(dataGridReservations.SelectedItem != null)
            {
                RateGuestWindow rateGuestWindow = new RateGuestWindow(dataGridReservations.SelectedItem as AccommodationReservationDTO);
                rateGuestWindow.GuestRated += UpdateEvent;
                rateGuestWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("User to rate not selected!");
            }
        }


        private bool isRateGuestWindowSubscribed = false;
        private bool isAddAccommodationWindowSubscribed = false;
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(tabControl.SelectedItem == tabItemAccommodations)
            {
                if (isRateGuestWindowSubscribed)
                {
                    buttonFunction.Click -= ShowRateGuestWindow;
                    isRateGuestWindowSubscribed = false;
                }

                if (!isAddAccommodationWindowSubscribed)
                {
                    buttonFunction.Click += ShowAddAccommodationWindow;
                    isAddAccommodationWindowSubscribed = true;
                }
                imageFunction.Source = new BitmapImage(new Uri(@"..\..\Resources\Images\add.png", UriKind.Relative));
                textBlockFunction.Text = "Add";
            }
            else
            {
                if (isAddAccommodationWindowSubscribed)
                {
                    buttonFunction.Click -= ShowAddAccommodationWindow;
                    isAddAccommodationWindowSubscribed = false;
                }

                if (!isRateGuestWindowSubscribed)
                {
                    buttonFunction.Click += ShowRateGuestWindow;
                    isRateGuestWindowSubscribed = true;
                }
                imageFunction.Source = new BitmapImage(new Uri(@"..\..\Resources\Images\edit.png", UriKind.Relative));
                textBlockFunction.Text = "Rate";
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
}
