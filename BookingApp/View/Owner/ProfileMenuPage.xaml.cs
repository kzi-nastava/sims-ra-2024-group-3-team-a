using BookingApp.DTO;
using BookingApp.ViewModel.Owner;
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

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for ProfileMenuPage.xaml
    /// </summary>
    public partial class ProfileMenuPage : Page
    {
        private ProfileMenuViewModel _profileMenuViewModel; 

        public ProfileMenuPage(UserDTO loggedInUser)
        {
            InitializeComponent();

            _profileMenuViewModel = new ProfileMenuViewModel(loggedInUser);
            DataContext = _profileMenuViewModel;
        }
    }
}
