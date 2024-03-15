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
        private UserRepository _userRepository;
        private UserDTO _userDTO;
        private AccommodationDTO _accommodationDTO;
        private AccommodationReservationRepository _accommodationReservationRepository;
        private int GuestNumber;

        public ReservationDetailsPage(AccommodationDTO accommodationDTO, UserDTO userDTO, DateOnly begin, DateOnly end)
        {
            InitializeComponent();
            _accommodationDTO = accommodationDTO;
            _userDTO = userDTO;
            _selectedBeginDate = begin;
            _selectedEndDate = end;
            _accommodationReservationRepository = new AccommodationReservationRepository();

        }

        private void NewReservation_Click(object sender, RoutedEventArgs e)
        {
            GuestNumber = Int32.Parse(GuestNumberTextBox.Text);
            if(GuestNumber < 0 || GuestNumber > _accommodationDTO.Capacity ) 
            {
                MessageBox.Show($"Uneli ste los broj gostiju - maksimum je {_accommodationDTO.Capacity}");
                return;
            }
            GuestRating rating = new GuestRating(0, 0, "");
            //List<AnonymousGuest> anonymousGuests = new List<AnonymousGuest>();
            AccommodationReservation acc = new AccommodationReservation(0, _userDTO.Id, _accommodationDTO.Id, _selectedBeginDate, _selectedEndDate, rating);
            _accommodationReservationRepository.Save(acc);
        }
    }
}
