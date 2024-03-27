using BookingApp.DTO;
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
    /// Interaction logic for MyReviewRatedPage.xaml
    /// </summary>
    public partial class MyReviewRatedPage : Page
    {
        private OwnerMainWindow _ownerMainWindow;

        private AccommodationReservationRepository _repository;
        private AccommodationReservationDTO _accommodationReservationDTO;

        public MyReviewRatedPage(OwnerMainWindow ownerMainWindow, AccommodationReservationDTO reservation)
        {
            InitializeComponent();
            _ownerMainWindow = ownerMainWindow;
            _accommodationReservationDTO = reservation;

            _repository = new AccommodationReservationRepository();
            
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
