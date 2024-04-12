using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
        private TouristRepository _touristRepository;
        private UserRepository _userRepository;


        public ObservableCollection<TouristDTO> Tourists { get; set; }
        public TourReviewsWindow(TourDTO tour)
        {
            InitializeComponent();
            _touristDTO = new TouristDTO();
            _tourReviewDTO = new TourReviewDTO();
            _tourReservationRepository = new TourReservationRepository();
            _tourReviewRepository = new TourReviewRepository();
            _userRepository = new UserRepository();
            _touristRepository = new TouristRepository();
            Tourists = new ObservableCollection<TouristDTO>();
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
                            AddReview(tourReservation, tourist);
                            _touristDTO = new TouristDTO(tourist);
                            _touristRepository.Save(_touristDTO.ToTourist());
                            Tourists.Add(_touristDTO);
                        }
                    }
                }

            }
        }

        private void AddReview(TourReservation tourReservation, Model.Tourist tourist)
        {
            foreach (TourReview tourReview in _tourReviewRepository.GetAll())
            {
                if (tourReview.TouristId == tourReservation.UserId && tourist.Name == FindUserName(tourReview.TouristId) && tourReservation.TourId==tourReview.TourId)
                {
                    TourReviewDTO tourReviewDTO = new TourReviewDTO(tourReview);
                    tourist.Review = tourReview;
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
            if (selectedTourist.Review != null)
            {
                ReviewDetailsWindow reviewDetails = new ReviewDetailsWindow(selectedTourist);
                reviewDetails.Show();
            }
            else
            {
                MessageBox.Show("There is no further information about this review", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
   
}
