using BookingApp.DTO;
using BookingApp.ViewModel.Owner.AccommodationRenovationViewModels;
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

namespace BookingApp.View.Owner.AccommodationRenovationPages
{
    /// <summary>
    /// Interaction logic for RenovationsPage.xaml
    /// </summary>
    public partial class RenovationsPage : Page
    {
        private RenovationsViewModel _renovationsViewModel;
        public RenovationsPage(UserDTO loggedInUser)
        {
            InitializeComponent();

            _renovationsViewModel = new RenovationsViewModel(loggedInUser);
            DataContext = _renovationsViewModel;
        }
    }
}
