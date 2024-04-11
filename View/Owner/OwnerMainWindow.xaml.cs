using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
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
        public static OwnerMainWindow Instance;

        public static ObservableCollection<AccommodationReservationDTO> FinishedAccommodationReservationsDTO { get; set; }

        private readonly AccommodationRepository _accommodationRepository;
        private readonly AccommodationReservationRepository _accommodationReservationRepository;
        private readonly UserRepository _userRepository;

        private readonly AccommodationReservationService _accommodationReservationService;

        public static UserDTO LoggedInOwner;

        private static Timer _notificationTimer;

        public static Frame MainFrame;
        public static Frame SideMenuFrame;

        public OwnerMainWindow(User owner)
        {
            InitializeComponent();
            DataContext = this;
            _accommodationRepository = new AccommodationRepository();
            _accommodationReservationRepository = new AccommodationReservationRepository();
            _userRepository = new UserRepository();

            _accommodationReservationService = new AccommodationReservationService();
            _accommodationReservationService.SetSuperOwner(owner);

            FinishedAccommodationReservationsDTO = new ObservableCollection<AccommodationReservationDTO>();

            LoggedInOwner = new UserDTO(owner);

            Update();
            SetNotificationTimer();

            MainFrame = frameMain;
            SideMenuFrame = frameSideMenu;
            MainFrame.Content = new AccommodationsPage(LoggedInOwner);

            if(Instance == null)
            {
                Instance = this;
            }
        }

        public static OwnerMainWindow GetInstance()
        {
            return Instance;
        }

        public void Update()
        {
            UpdateAccomodationReservations();
        }
        private void UpdateAccomodationReservations()
        {
            FinishedAccommodationReservationsDTO.Clear();
            foreach (var reservation in _accommodationReservationRepository.GetAll())
            {
                AccommodationReservationDTO reservationDTO = new AccommodationReservationDTO(reservation);
                AccommodationDTO accommodationDTO = new AccommodationDTO(_accommodationRepository.GetById(reservationDTO.AccommodationId));
                if (IsLoggedOwner(accommodationDTO) && (IsNotExpired(reservationDTO) || IsOwnerReviewed(reservationDTO)))
                    FinishedAccommodationReservationsDTO.Add(reservationDTO);
            }
        }
        private bool IsLoggedOwner(AccommodationDTO accommodationDTO)
        {
            if (accommodationDTO.OwnerId == LoggedInOwner.Id)
            {
                return true;
            }
            return false;
        }
        private bool IsNotExpired(AccommodationReservationDTO reservationDTO)
        {
            if (reservationDTO.EndDate <= DateOnly.FromDateTime(DateTime.Now) && DateOnly.FromDateTime(DateTime.Now) <= reservationDTO.EndDate.AddDays(5))
            {
                return true;
            }
            return false;
        }
        private bool IsOwnerReviewed(AccommodationReservationDTO reservationDTO)
        {
            if (reservationDTO.RatingDTO.OwnerCleannessRating != 0)
            {
                return true;
            }
            return false;
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
            if(FinishedAccommodationReservationsDTO.Any(reservation => reservation.RatingDTO.OwnerCleannessRating == 0))
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    NotificationPage notificationPage = new NotificationPage();
                    notificationPage.buttonNotification.Click += (sender, e) => frameNotification.Content = null;
                    foreach(var reservation in FinishedAccommodationReservationsDTO)
                    {
                        if(reservation.RatingDTO.OwnerCleannessRating == 0)
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
}
