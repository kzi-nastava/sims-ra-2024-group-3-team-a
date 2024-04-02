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

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for ReservationDetailsPage.xaml
    /// </summary>
    public partial class ReservationDetailsPage : Page
    {
        private DateOnly _selectedBeginDate;
        private DateOnly _selectedEndDate;

        private UserDTO _userDTO;
        private AccommodationDTO _accommodationDTO;

        private AccommodationReservationRepository _accommodationReservationRepository;

        public ReservationDetailsPage(AccommodationDTO accommodationDTO, UserDTO userDTO, DateOnly begin, DateOnly end)
        {
            InitializeComponent();
            _accommodationReservationRepository = new AccommodationReservationRepository();
            _accommodationDTO = accommodationDTO;
            _userDTO = userDTO;
            _selectedBeginDate = begin;
            _selectedEndDate = end; 
        }

        private void NewReservation_Click(object sender, RoutedEventArgs e)
        {
            int _guestNumber;
            if(int.TryParse(GuestNumberTextBox.Text, out _guestNumber))
            {
                if (_guestNumber < 0 || _guestNumber > _accommodationDTO.Capacity)
                {
                    MessageBox.Show($"Error! Capacity is {_accommodationDTO.Capacity} guests!");
                    return;
                }
                Review rating = new Review();
                AccommodationReservation acc = new AccommodationReservation(0, _userDTO.Id, _accommodationDTO.Id, _selectedBeginDate, _selectedEndDate, false, rating);
                _accommodationReservationRepository.Save(acc);
                MessageBox.Show("Reservation successful!");
            }
            else
            {
                MessageBox.Show("Please enter number of guests!");
            }
        }
    }
}
