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
        private UserDTO _userDTO;

        private TourRepository _tourRepository;
        private TourReservationRepository _tourReservationRepository { get; set; }

        private TouristMainWindow _tourMainWindow; 
        public ObservableCollection<TourDTO> AlternativeTours { get; set; }
        
        public AlternativeToursWindow(TouristMainWindow tourMainWindow, TourDTO tourDTO, UserDTO userDTO)
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository = new TourRepository();
            _tourReservationRepository= new TourReservationRepository();

            _tourMainWindow = tourMainWindow;

            _tourDTO = tourDTO;
            _userDTO= userDTO;

            AlternativeTours = new ObservableCollection<TourDTO>();

            Update();
        }
        private void Update()
        {
            AlternativeTours.Clear();
            foreach (Tour tour in _tourRepository.GetToursWithSameLocation(_tourDTO.ToTour()))
                AlternativeTours.Add(new TourDTO(tour));
        }
        private void ShowTourReservationWindow(object sender, RoutedEventArgs e)
        {
            _tourDTO = dataGridAlternativeTour.SelectedItem as TourDTO;

            if (_tourDTO != null)
            {
              //  TourReservationWindow tourReservationWindow = new TourReservationWindow(_tourMainWindow,_tourReservationRepository, _tourDTO, _userDTO);
              //  tourReservationWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("You didn't choose any tour!");
            }
        }
    }
}
