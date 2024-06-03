using BookingApp.DTO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for GuestHomePage.xaml
    /// </summary>
    public partial class GuestHomePage : Page
    {

        private GuestMainViewModel _guestMainViewModel;
        public static GuestHomePage Instance;
        public GuestHomePage(UserDTO loggedInUser)
        {
            InitializeComponent();

            _guestMainViewModel = new GuestMainViewModel(loggedInUser);
            DataContext = _guestMainViewModel;
            if (Instance == null)
            {
                Instance = this;
            }
        }
    }
}
