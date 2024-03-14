using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApp.View
{

    public partial class GuideMainWindow : Window
    {

        public static ObservableCollection<TourDTO> Tours { get; set; }
        private static TourDTO tour { get; set; }

        private readonly TourRepository _repository;
        private TourDTO _activeTour; 

        public GuideMainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new TourRepository();
            Tours = new ObservableCollection<TourDTO>();
            tour = new TourDTO();
            Update();
        }
        private void Update()
        {
            Tours.Clear();
           
            foreach (Tour tour in _repository.GetAll())
            {
                TourDTO tourDTO = new TourDTO(tour);
                if (tourDTO.BeginingTime.Date == DateTime.Today)
                {
                    Tours.Add(tourDTO);
                }
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchInput = textboxSearch.Text.ToLower();
            string[] resultArray = searchInput.Split(',').Select(s => s.Trim()).ToArray();

            var filtered = Tours;

            if (resultArray.Length == 0 || string.IsNullOrWhiteSpace(searchInput))
            {
                dataGridTour.ItemsSource = Tours;
            }

            foreach (string input in resultArray)
            {
                string value = input;

                if (string.IsNullOrWhiteSpace(value))
                    continue;

                filtered = FilterTours(filtered, value);
            }

            dataGridTour.ItemsSource = filtered;
        }

        private void Tour_Click(object sender, RoutedEventArgs e)
        {
           
            TourDTO selectedTour = ((Button)sender).DataContext as TourDTO;

          
            if (_activeTour == null || _activeTour != selectedTour)
            {
                _activeTour = selectedTour;

                KeyPointsWindow tourDetailsWindow = new KeyPointsWindow(_activeTour);
                tourDetailsWindow.Show();
            }
           
        }
        private void ShowAll_Click (object sender, RoutedEventArgs e)
        {
            AllToursView allToursView = new AllToursView();
            allToursView.Show();
        }

        private ObservableCollection<BookingApp.DTO.TourDTO> FilterTours(ObservableCollection<BookingApp.DTO.TourDTO> tours, string value)
        {
            var filtered = new ObservableCollection<BookingApp.DTO.TourDTO>();

            foreach (var tour in tours)
            {
                if (tour.Duration.ToString().Contains(value)
                    || tour.Language.ToString().ToLower().Contains(value)
                    || tour.BeginingTime.ToString().Contains(value)
                    || (tour.LocationDTO.City.ToLower() + ", " + tour.LocationDTO.Country.ToLower()).Contains(value)
                    || tour.MaxTouristNumber.ToString().Contains(value)
                    || tour.Duration.ToString().Contains(value)
                    || tour.Description.ToLower().Contains(value))
                    
                {
                    filtered.Add(tour);
                }
            }

            return filtered;
        }

    }
}
