using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
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

namespace BookingApp.View.Guide
{
    /// <summary>
    /// Interaction logic for TourStatisticsWindow.xaml
    /// </summary>
    public partial class TourStatisticsWindow : Window
    {

        private TourDTO _tourDTO ;
        public static ObservableCollection<TourDTO> Tours { get; set; }
        public static ObservableCollection<TourDTO> ToursFinished { get; set; }
        private TourRepository _tourRepository;
        private TourReservationRepository _tourReservationRepository;
        private int _maxTouristsCounter ;
        public TourStatisticsWindow()
        {
            InitializeComponent();
            _tourDTO =new TourDTO();
            Tours = new ObservableCollection<TourDTO>();
            ToursFinished = new ObservableCollection<TourDTO>();
            _tourRepository = new TourRepository();
            Update();
            DataContext = this;
        }
        private void Update()
        {
            _maxTouristsCounter = -1;
            foreach( Tour tour in _tourRepository.GetAll())
            {
                if (tour.TouristsPresent > _maxTouristsCounter && tour.CurrentKeyPoint == "finished" ){

                    _maxTouristsCounter=tour.TouristsPresent;
                    _tourDTO = new TourDTO(tour);

                }
                if(tour.CurrentKeyPoint == "finished")
                {
                    _tourDTO = new TourDTO(tour);
                    ToursFinished.Add(_tourDTO);
                }
            }
            Tours.Add(_tourDTO);
        }
        private void ShowMostVisitedByYear(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Content is string dateYear)
                {
                    int year = Convert.ToInt32(dateYear);
                    MostVisitedTourWindow mostVisitedTourWindow = new MostVisitedTourWindow(year);
                    mostVisitedTourWindow.Show();
                }

            }
        }
        private void ShowTouristStatistics(object sender, RoutedEventArgs e)
        {
            if (dataGridTour.SelectedItem != null)
            { 
                TourDTO selectedItem = dataGridTour.SelectedItem as TourDTO;
                TouristStatisticsWindow touristStatisticsWindow = new TouristStatisticsWindow(selectedItem);
                touristStatisticsWindow.Show();

            }
        }
    }

}
