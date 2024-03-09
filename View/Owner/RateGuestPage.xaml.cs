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
    /// Interaction logic for RateGuestPage.xaml
    /// </summary>
    public partial class RateGuestPage : Page
    {
        private OwnerMainWindow _ownerMainWindow;

        private AccommodationReservationRepository _repository;
        private AccommodationReservationDTO _accommodationReservationDTO;

        public RateGuestPage(OwnerMainWindow ownerMainWindow, AccommodationReservationDTO reservation)
        {
            InitializeComponent();

            _repository = new AccommodationReservationRepository();
            _accommodationReservationDTO = new AccommodationReservationDTO(reservation);
            _ownerMainWindow = ownerMainWindow;

            DataContext = _accommodationReservationDTO;  
        }

        private void Rate(object sender, RoutedEventArgs e)
        {
            _repository.Update(_accommodationReservationDTO.ToAccommodationReservation());
            _ownerMainWindow.Update();
        }
        private void textBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
