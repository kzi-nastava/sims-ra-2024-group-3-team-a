using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.View.Owner.WizardAndHelp;
using BookingApp.ViewModel.Owner;
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

        public static UserDTO LoggedInOwner;

        private static Timer _notificationTimer;

        public static Frame MainFrame;
        public static Frame SideMenuFrame;
        public static Frame NotificationFrame;  

        private static OwnerMainViewModel _ownerMainViewModel;
        public OwnerMainWindow(User owner)
        {
            InitializeComponent();

            LoggedInOwner = new UserDTO(owner);
            _ownerMainViewModel = new OwnerMainViewModel(LoggedInOwner);
            DataContext = _ownerMainViewModel;

            SetNotificationTimer();

            MainFrame = frameMain;
            SideMenuFrame = frameSideMenu;
            NotificationFrame = frameNotification;
            MainFrame.Content = new AccommodationsPage(LoggedInOwner);

            if(5 == 5)
            {
                MainFrame.Content = new Wizard(LoggedInOwner);
            }

            if(Instance == null)
            {
                Instance = this;
            }
        }

        public static OwnerMainWindow GetInstance()
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
            _ownerMainViewModel.UpdateFinishedReservations();
            if (_ownerMainViewModel.FinishedAccommodationReservationsDTO.Any(reservation => reservation.RatingDTO.OwnerCleannessRating == 0))
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    NotificationPage notificationPage = new NotificationPage();
                    notificationPage.buttonNotification.Click += (sender, e) => frameNotification.Content = null;
                    foreach(var reservation in _ownerMainViewModel.FinishedAccommodationReservationsDTO)
                    {
                        if(reservation.RatingDTO.OwnerCleannessRating == 0)
                        {
                            UserDTO guest = _ownerMainViewModel.GetUserDTOById(reservation.GuestId);
                            notificationPage.buttonNotification.ToolTip += "You didn't rate Guest " + guest.Username + "\n";
                        }                       
                    }

                    frameNotification.Content = notificationPage;
                });
            }  
        }
    }
}
