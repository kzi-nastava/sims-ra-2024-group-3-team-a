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
using System.Windows.Shapes;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for TourReservation.xaml
    /// </summary>
    public partial class TourReservationWindow : Window
    {
        private TourReservationDTO _tourReservationDTO;
        private TourDTO _tourDTO;
        private TourReservationRepository _tourReservationRepository;
        private UserDTO _userDTO;
        public TourReservationWindow(TourReservationRepository tourReservationRepository,TourDTO tourDTO, UserDTO userDTO)
        {
            InitializeComponent();
            _tourReservationRepository = tourReservationRepository;
            _tourDTO = new TourDTO(tourDTO);
            _tourReservationDTO = new TourReservationDTO(tourDTO);
            _tourReservationDTO(_tourDTO, userDTO);
            DataContext = _tourDTO;

        }

        private void ConfirmReservation_Click(object sender, RoutedEventArgs e)
        {
           _tourReservationRepository.Save(_tourReservationDTO.ToTourReservation());
        }
    }
}
