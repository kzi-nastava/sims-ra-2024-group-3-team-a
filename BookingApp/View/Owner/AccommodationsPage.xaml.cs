using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.ViewModel.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AccommodationsPage.xaml
    /// </summary>
    public partial class AccommodationsPage : Page
    {
        private AccommodationsViewModel _accommodationViewModel;

        public AccommodationsPage(UserDTO loggedInUser)
        {
            InitializeComponent();
            
            _accommodationViewModel = new AccommodationsViewModel(loggedInUser);
            DataContext = _accommodationViewModel;
        }
    }
}
