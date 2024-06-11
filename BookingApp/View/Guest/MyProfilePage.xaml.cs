using BookingApp.DTO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for MyProfilePage.xaml
    /// </summary>
    public partial class MyProfilePage : Page
    {
        public static MyProfilePage Instance;
        public static GuestReservationsViewModel _guestReservationsViewModel;
        public UserDTO User = GuestMainViewWindow.LoggedInGuest;
        public MyProfilePage()
        {
            InitializeComponent();
            _guestReservationsViewModel = new GuestReservationsViewModel(GuestMainViewWindow.LoggedInGuest);
            DataContext = _guestReservationsViewModel;
            if(Instance == null)
            {
                Instance = this;
            }
        }
    }
}
