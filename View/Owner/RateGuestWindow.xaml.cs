using BookingApp.DTO;
using BookingApp.Model.Enums;
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
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for RateGuestWindow.xaml
    /// </summary>
    public partial class RateGuestWindow : Window
    {
        private AccommodationReservationRepository _repository;
        private AccommodationReservationDTO _accommodationReservationDTO;

        public event EventHandler GuestRated;

        public RateGuestWindow(AccommodationReservationDTO reservation)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            _repository = new AccommodationReservationRepository();
            _accommodationReservationDTO = new AccommodationReservationDTO(reservation);

            DataContext = _accommodationReservationDTO;
        }

        private void Rate(object sender, RoutedEventArgs e)
        {
            _repository.Update(_accommodationReservationDTO.ToAccommodationReservation());

            GuestRated?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
