using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Timers;
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
using Application = System.Windows.Application;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for OwnerMainWindow.xaml
    /// </summary>
    public partial class OwnerMainWindow : Window
    {
        public static ObservableCollection<AccommodationDTO> AccommodationsDTO { get; set; }
        public static ObservableCollection<AccommodationReservationDTO> AccommodationReservationsDTO { get; set; }

        private readonly AccommodationRepository _accommodationRepository;
        private readonly AccommodationReservationRepository _accommodationReservationRepository;
        private readonly UserRepository _userRepository;

        private UserDTO _loggedInOwner;

        private static Timer _notificationTimer;

        public OwnerMainWindow(User owner)
        {
            InitializeComponent();
            DataContext = this;
            _accommodationRepository = new AccommodationRepository();
            _accommodationReservationRepository = new AccommodationReservationRepository();
            _userRepository = new UserRepository();

            AccommodationsDTO = new ObservableCollection<AccommodationDTO>();
            AccommodationReservationsDTO = new ObservableCollection<AccommodationReservationDTO>();

            _loggedInOwner = new UserDTO(owner);

            Update();

            SetNotificationTimer();
        }

        public void Update()
        {
            AccommodationsDTO.Clear();
            AccommodationReservationsDTO.Clear();
            foreach (var accommodation in _accommodationRepository.GetAll())
            {
                AccommodationDTO accommodationDTO = new AccommodationDTO(accommodation);
                if(IsLoggedOwner(accommodationDTO))   
                    AccommodationsDTO.Add(accommodationDTO);
            }

            foreach (var reservation in _accommodationReservationRepository.GetAll())
            {
                AccommodationReservationDTO reservationDTO = new AccommodationReservationDTO(reservation);
                AccommodationDTO accommodationDTO = new AccommodationDTO(_accommodationRepository.GetById(reservationDTO.AccommodationId));
                if(IsLoggedOwner(accommodationDTO) && IsNotExpired(reservationDTO))
                    AccommodationReservationsDTO.Add(reservationDTO);
            }
        }
        private bool IsLoggedOwner(AccommodationDTO accommodationDTO)
        {
            if(accommodationDTO.OwnerId == _loggedInOwner.Id)
            {
                return true;
            }
            return false;
        }
        private bool IsNotExpired(AccommodationReservationDTO reservationDTO)
        {
            if(reservationDTO.EndDate <= DateOnly.FromDateTime(DateTime.Now) && DateOnly.FromDateTime(DateTime.Now) <= reservationDTO.EndDate.AddDays(5))
            {
                return true;
            }
            return false;
        }

        private void ShowAddAccommodationPage(object sender, RoutedEventArgs e)
        {
            if(frameMain.Content != null)
            {
                frameMain.Content = null;
            }
            else
            {
                frameMain.Content = new AddAccommodationPage(this);
            }  
        }
        private void ShowRateGuestPage(object sender, RoutedEventArgs e)
        {
            if(dataGridReservations.SelectedItem != null && frameMain.Content == null)
            {
                frameMain.Content = new RateGuestPage(this, dataGridReservations.SelectedItem as AccommodationReservationDTO);
            }
            else if(frameMain.Content == null)
            {
                MessageBox.Show("User to rate not selected!");
            }
            else
            {
                frameMain.Content = null;
            }
        }
        private void ShowSideMenu(object sender, RoutedEventArgs e)
        {
            frameSideMenu.Content = new SideMenuPage(this);
        }

        private bool isRateGuestPageOpened = false;
        private bool isAddAccommodationPageOpened = false;
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(tabControl.SelectedItem == tabItemAccommodations)
            {
                if (isRateGuestPageOpened)
                {
                    buttonFunction.Click -= ShowRateGuestPage;
                    isRateGuestPageOpened = false;
                }

                if (!isAddAccommodationPageOpened)
                {
                    buttonFunction.Click += ShowAddAccommodationPage;
                    isAddAccommodationPageOpened = true;
                }
                imageFunction.Source = new BitmapImage(new Uri(@"..\..\Resources\Images\add.png", UriKind.Relative));
                textBlockFunction.Text = "Add";
            }
            else
            {
                if (isAddAccommodationPageOpened)
                {
                    buttonFunction.Click -= ShowAddAccommodationPage;
                    isAddAccommodationPageOpened = false;
                }

                if (!isRateGuestPageOpened)
                {
                    buttonFunction.Click += ShowRateGuestPage;
                    isRateGuestPageOpened = true;
                }
                imageFunction.Source = new BitmapImage(new Uri(@"..\..\Resources\Images\edit.png", UriKind.Relative));
                textBlockFunction.Text = "Rate";
            }
            frameMain.Content = null;
        }

        private void SetNotificationTimer()
        {
            _notificationTimer = new Timer(5000);

            _notificationTimer.Elapsed += (sender, e) => Notify(frameNotification, _userRepository);

            _notificationTimer.AutoReset = true;
            _notificationTimer.Enabled = true;
        }
        private static void Notify(Frame frameNotification, UserRepository userRepository)
        {
            if(AccommodationReservationsDTO.Any(reservation => reservation.RatingDTO.CleannessRating == 0))
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    NotificationPage notificationPage = new NotificationPage();
                    notificationPage.buttonNotification.Click += (sender, e) => frameNotification.Content = null;
                    foreach(var reservation in AccommodationReservationsDTO)
                    {
                        if(reservation.RatingDTO.CleannessRating == 0)
                        {
                            UserDTO guest = new UserDTO(userRepository.GetById(reservation.GuestId));
                            notificationPage.buttonNotification.ToolTip += "You didn't rate Guest " + guest.Username + "\n";
                        }                       
                    }

                    frameNotification.Content = notificationPage;
                });
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
