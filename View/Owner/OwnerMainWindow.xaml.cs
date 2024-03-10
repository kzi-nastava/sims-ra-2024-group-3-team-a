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
        public static ObservableCollection<AccommodationDTO> Accommodations { get; set; }
        public static ObservableCollection<AccommodationReservationDTO> AccommodationReservations { get; set; }

        private readonly AccommodationRepository _accommodationRepository;
        private readonly AccommodationReservationRepository _accommodationReservationRepository;

        private static Timer _notificationTimer;

        public OwnerMainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _accommodationRepository = new AccommodationRepository();
            _accommodationReservationRepository = new AccommodationReservationRepository();

            Accommodations = new ObservableCollection<AccommodationDTO>();
            AccommodationReservations = new ObservableCollection<AccommodationReservationDTO>();

            Update();

            SetNotificationTimer();
        }

        public void Update()
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

        private bool isRateGuestWindowOpened = false;

        private bool isAddAccommodationWindowOpened = false;

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(tabControl.SelectedItem == tabItemAccommodations)
            {
                if (isRateGuestWindowOpened)
                {
                    buttonFunction.Click -= ShowRateGuestPage;
                    isRateGuestWindowOpened = false;
                }

                if (!isAddAccommodationWindowOpened)
                {
                    buttonFunction.Click += ShowAddAccommodationPage;
                    isAddAccommodationWindowOpened = true;
                }
                imageFunction.Source = new BitmapImage(new Uri(@"..\..\Resources\Images\add.png", UriKind.Relative));
                textBlockFunction.Text = "Add";
            }
            else
            {
                if (isAddAccommodationWindowOpened)
                {
                    buttonFunction.Click -= ShowAddAccommodationPage;
                    isAddAccommodationWindowOpened = false;
                }

                if (!isRateGuestWindowOpened)
                {
                    buttonFunction.Click += ShowRateGuestPage;
                    isRateGuestWindowOpened = true;
                }
                imageFunction.Source = new BitmapImage(new Uri(@"..\..\Resources\Images\edit.png", UriKind.Relative));
                textBlockFunction.Text = "Rate";
            }
            frameMain.Content = null;
        }

        private void SetNotificationTimer()
        {
            _notificationTimer = new Timer(1000 * 10);

            _notificationTimer.Elapsed += (sender, e) => Notify(frameNotification, this);

            _notificationTimer.AutoReset = true;
            _notificationTimer.Enabled = true;
        }

        private static void Notify(Frame frameNotification, OwnerMainWindow ownerMainWindow)
        {
            if(AccommodationReservations.Any(reservation => reservation.RatingDTO.CleannessRating == 0))
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    NotificationPage notificationPage = new NotificationPage(ownerMainWindow);
                    notificationPage.buttonNotification.Click += (sender, e) => frameNotification.Content = null;
                    foreach(var reservation in AccommodationReservations)
                    {
                        if(reservation.RatingDTO.CleannessRating == 0)
                        {
                            notificationPage.buttonNotification.ToolTip += "You didn't rate Guest" + reservation.GuestId + "\n";
                        }                       
                    }

                    frameNotification.Content = notificationPage;
                });
            }
        }

        private void frameSideMenu_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

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
