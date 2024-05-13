using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.ViewModel.Owner;
using BookingApp.ViewModel.Guest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
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
using BookingApp.View.Owner;
using System.Timers;

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for GuestMainWindow.xaml
    /// </summary>
    public partial class GuestMainWindow : Window
    {
        public static ObservableCollection<AccommodationDTO> AccommodationsDTO { get; set; }
        public static UserDTO _userDTO;
        public static Frame MainFrame;
        public static Frame MyRequestsFrame;
        public static GuestMainWindow Instance;
        private static Timer _notificationTimer;
        private static GuestMainViewModel _guestMainViewModel;

        public GuestMainWindow(User user)
        {
            InitializeComponent();
            _userDTO = new UserDTO(user);
            _guestMainViewModel = new GuestMainViewModel(_userDTO);
            DataContext = _guestMainViewModel;
            SetNotificationTimer();
            AccommodationsDTO = new ObservableCollection<AccommodationDTO>();
            MainFrame = frameMain;
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public static GuestMainWindow GetInstance()
        {
            return Instance;
        }
        private void SetNotificationTimer()
        {
            _notificationTimer = new Timer(5000);

            _notificationTimer.Elapsed += (sender, e) => Notify(frameNotification);

            _notificationTimer.AutoReset = true;
            _notificationTimer.Enabled = true;
        }
        private static void Notify(Frame frameNotification)
        {
            _guestMainViewModel.UpdateReservations();
            if (_guestMainViewModel.MyMessagesDTO.Any())
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    NotificationPage notificationPage = new NotificationPage();
                    notificationPage.buttonNotification.Click += (sender, e) => frameNotification.Content = null;
                    foreach (var message in _guestMainViewModel.MyMessagesDTO)
                    {
                        notificationPage.buttonNotification.ToolTip += "You have new unread messages!" + "\n"; 
                    }

                    frameNotification.Content = notificationPage;
                });
            }
        }
    }
}
