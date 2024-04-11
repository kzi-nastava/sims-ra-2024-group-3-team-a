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

namespace BookingApp.View.Guide
{
    /// <summary>
    /// Interaction logic for TourReviewsWindow.xaml
    /// </summary>
    public partial class TourReviewsWindow : Window
    {
        private TouristDTO _touristDTO;
        private TourDTO _tourDTO;
        private TourReviewDTO _tourReviewDTO; 
        private TourReservationRepository _tourReservationRepository;
        private TourReviewRepository _tourReviewRepository;
        private UserRepository _userRepository;

        public ObservableCollection<TourReviewDTO> Reviews { get; set; }
        public ObservableCollection<TouristDTO> Tourists { get; set; }
        public TourReviewsWindow(TourDTO tour)
        {
            InitializeComponent();
            _touristDTO = new TouristDTO();
            _tourReviewDTO = new TourReviewDTO();
            _tourReservationRepository = new TourReservationRepository();
            _tourReviewRepository = new TourReviewRepository();
            _userRepository = new UserRepository();
            Tourists = new ObservableCollection<TouristDTO>();
            Reviews = new ObservableCollection<TourReviewDTO>();
            _tourDTO = tour;
            Update();
            DataContext = this;
        }
        private void Update()
        {
            foreach (TourReservation tourReservation in _tourReservationRepository.GetAll())
            {
                if (_tourDTO.Id == tourReservation.TourId)
                {
                    foreach (Model.Tourist tourist in tourReservation.Tourists)
                    {
                        if (tourist.JoiningKeyPoint != "")
                        {
                            _touristDTO = new TouristDTO(tourist);
                            AddReview(tourReservation);
                            Tourists.Add(_touristDTO);
                        }
                    }
                }

            }
        }

        private void AddReview(TourReservation tourReservation)//TouristDTO tourist)
        {
            foreach (TourReview tourReview in _tourReviewRepository.GetAll())
            {
                if (tourReview.TouristId == tourReservation.UserId && _touristDTO.Name == FindUserName(tourReview.TouristId))
                {
                    TourReviewDTO tourReviewDTO = new TourReviewDTO(tourReview);
                    _touristDTO.Review = tourReviewDTO;
                }
            }
        }
        private string FindUserName(int id)
        {
            User user = _userRepository.GetById(id);
            string name = user.Username;
            return name;
        }
        private void ShowReviewDetails(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            TouristDTO selectedTourist  = new TouristDTO();
            selectedTourist = ((Button)sender).DataContext as TouristDTO;
            ReviewDetailsWindow reviewDetails = new ReviewDetailsWindow(selectedTourist);
            reviewDetails.Show();

        }

    }
}
