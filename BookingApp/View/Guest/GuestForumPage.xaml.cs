using BookingApp.DTO;
using BookingApp.ViewModel.Guest;
using BookingApp.ViewModel.Owner.ForumViewModels;
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
    /// Interaction logic for GuestForumPage.xaml
    /// </summary>
    public partial class GuestForumPage : Page
    {
        private GuestForumViewModel _guestForumViewModel;
        public GuestForumPage(UserDTO loggedInUser)
        {
            InitializeComponent();

            _guestForumViewModel = new GuestForumViewModel(loggedInUser);
            DataContext = _guestForumViewModel;
        }
    }
}
