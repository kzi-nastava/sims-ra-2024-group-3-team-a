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
    /// Interaction logic for MyToursWindow.xaml
    /// </summary>
    public partial class MyToursWindow : Window
    {
        public static ObservableCollection<TourDTO> ActiveTours { get; set; }

        public static ObservableCollection<TourDTO> UnactiveTours { get; set; }

        private TourDTO _tourDTO;

        private readonly TourReservationRepository _tourReservationRepository;

        private readonly TourRepository _tourRepository;

        public MyToursWindow()
        {
            InitializeComponent();

            _tourDTO= new TourDTO();
            _tourReservationRepository = new TourReservationRepository();
            _tourRepository = new TourRepository();
            ActiveTours = new ObservableCollection<TourDTO>();
            UnactiveTours = new ObservableCollection<TourDTO>();

            DataContext = this;
            UpdateActiveTours();
            UpdateUnactiveTours();
        }

        private void UpdateActiveTours()
        {
            ActiveTours.Clear();
            foreach (Tour tour in _tourRepository.GetActiveTours())
                ActiveTours.Add(new TourDTO(tour));
        }

        private void UpdateUnactiveTours()
        {
            UnactiveTours.Clear();
            foreach (Tour tour in _tourRepository.GetUnactiveTours())
                UnactiveTours.Add(new TourDTO(tour));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _tourDTO = listBoxActiveTours.SelectedItem as TourDTO;
            TrackTourWindow trackTourWinodw = new TrackTourWindow(_tourDTO);
            trackTourWinodw.Show();
        }
    }
}
