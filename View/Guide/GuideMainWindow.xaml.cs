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
        private static TourDTO _tour { get; set; }

        private readonly TourRepository _repository;
        private TourDTO _activeTour; 

        public GuideMainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new TourRepository();
            Tours = new ObservableCollection<TourDTO>();
            _tour = new TourDTO();
            Update();
        }
        public void Update()
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


        private void Tour_Click(object sender, RoutedEventArgs e)
        {
           
            TourDTO selectedTour = ((Button)sender).DataContext as TourDTO;
            if (selectedTour.CurrentKeyPoint != "finished")
            {

                if (_activeTour == null || _activeTour != selectedTour)
                {
                    _activeTour = selectedTour;

                    KeyPointsWindow tourDetailsWindow = new KeyPointsWindow(_activeTour);
                    tourDetailsWindow.Show();
                }
            }
            else
            {
                MessageBox.Show("This tour has already finished", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
           
        }
        private void ShowAll_Click (object sender, RoutedEventArgs e)
        {
            AllToursView allToursView = new AllToursView(this);
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
