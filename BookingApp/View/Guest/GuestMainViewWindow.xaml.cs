using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.View.Owner;
using BookingApp.ViewModel.Owner;
using BookingApp.ViewModel.Guest;
using System;
using System.Collections.Generic;
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
using System.Timers;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Application;

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for GuestMainViewWindow.xaml
    /// </summary>
    public partial class GuestMainViewWindow : Window
    {
        public static GuestMainViewWindow Instance;

        public static UserDTO LoggedInGuest;

        private static Timer _notificationTimer;

        public static Frame MainFrame;
        public static Frame SideMenuFrame;
        public static Frame NotificationFrame;

        private static GuestMainViewModel _guestMainViewModel;
        public GuestMainViewWindow(User guest)
        {
            InitializeComponent();

            LoggedInGuest = new UserDTO(guest);
            _guestMainViewModel = new GuestMainViewModel(LoggedInGuest);
            DataContext = _guestMainViewModel;

            SetNotificationTimer();

            MainFrame = frameMain;
            SideMenuFrame = frameSideMenu;
            NotificationFrame = frameNotification;
            MainFrame.Content = new GuestHomePage(LoggedInGuest);

            if (Instance == null)
            {
                Instance = this;
            }
        }

        public static GuestMainViewWindow GetInstance()
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
