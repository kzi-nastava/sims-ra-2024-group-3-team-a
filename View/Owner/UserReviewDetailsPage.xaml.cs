using Accessibility;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
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
        private OwnerMainWindow _ownerMainWindow;

        private AccommodationReservationRepository _repository;
        private AccommodationReservationDTO _accommodationReservationDTO;

        public UserReviewDetailsPage(OwnerMainWindow ownerMainWindow, AccommodationReservationDTO accommodationReservationDTO)
        {
            InitializeComponent();
            DataContext = this;

            _ownerMainWindow = ownerMainWindow;

            _accommodationReservationDTO = accommodationReservationDTO;
            DataContext = _accommodationReservationDTO;
        }

        private void ShowSideMenu(object sender, RoutedEventArgs e)
        {
            _ownerMainWindow.ShowSideMenu(sender, e);
        }
        private void GoBack(object sender, RoutedEventArgs e)
        {
            _ownerMainWindow.frameMain.GoBack();
        }
    }
}
