using Accessibility;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
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
    /// Interaction logic for UserReviewDetailsPage.xaml
    /// </summary>
    public partial class UserReviewDetailsPage : Page
    {
        private UserReviewDetailsViewModel _userReviewDetailsViewModel;

        public UserReviewDetailsPage(AccommodationReservationDTO accommodationReservationDTO)
        {
            InitializeComponent();
            
            _userReviewDetailsViewModel = new UserReviewDetailsViewModel(accommodationReservationDTO);
            DataContext = _userReviewDetailsViewModel;
        }
    }
}
