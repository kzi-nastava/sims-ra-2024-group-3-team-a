using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
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
using System.Windows.Shapes;

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for AlternativeToursWindow.xaml
    /// </summary>
    public partial class AlternativeToursWindow : Window
    {
        private  TourDTO _tourDTO { get; set; }
        private TourRepository _tourRepository;
        private TourReservationRepository _tourReservationRepository;
        private TourReservationDTO _tourReservationDTO;
        private UserDTO _userDTO;
        public ObservableCollection<TourDTO> AlternativeTours { get; set; }
        

        public AlternativeToursWindow(TourDTO tourDTO, UserDTO userDTO)
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository = new TourRepository();
            _tourDTO = tourDTO;
            _userDTO= userDTO;
            AlternativeTours = new ObservableCollection<TourDTO>();
            Update();


        }
        private void Update()
        {
            AlternativeTours.Clear();
            foreach (Tour tour in _tourRepository.GetToursWithSameLocation(_tourDTO.ToTour()))  AlternativeTours.Add(new TourDTO(tour));
        }
        private void Reservation_Click(object sender, RoutedEventArgs e)
        {
            _tourDTO = dataGridAlternativeTour.SelectedItem as TourDTO;

            if (_tourDTO != null)
            {
                TourReservationWindow tourReservationWindow = new TourReservationWindow(_tourReservationRepository, _tourDTO, _userDTO);
                tourReservationWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("You didn't choose any tour!");
            }
        }
    }
}
