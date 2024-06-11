using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.View.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using BookingApp.View.Owner;
using BookingApp.Commands;
using System.Windows.Input;
using BookingApp.InjectorNameSpace;
using BookingApp.Repository.Interfaces;
using System.ComponentModel;
using System.Reflection.Metadata;

namespace BookingApp.ViewModel.Guide
{
    class TourReviewsReadOnlyViewModel : ViewModel
    {
        private TourDTO _tourDTO;
        private TourReservationService _tourReservationService;
        private TourReviewService _tourReviewService;
        private TouristService _touristService;
        private RelayCommand _showReviewDetailsCommand;
        private RelayCommand _markAsInvalidCommand;
        private RelayCommand _markAsValidCommand;
        private ObservableCollection<TouristDTO> _touristsDTO { get; set; }
        public TourReviewsReadOnlyViewModel(TourDTO tour)
        {
            _tourDTO = tour;
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _tourReservationService = new TourReservationService(tourReservationRepository, userRepository, touristRepository, tourReviewRepository, voucherRepository);
            _tourReviewService = new TourReviewService(tourReviewRepository);
            _touristService = new TouristService(touristRepository);
            List<TouristDTO> touristsDTO = _tourReservationService.GetJoinedTourists(_tourDTO.ToTourAllParam()).Select(tourist => new TouristDTO(tourist)).ToList();
            _touristsDTO = new ObservableCollection<TouristDTO>(touristsDTO);
            _showReviewDetailsCommand = new RelayCommand(ShowReviewDetails);
            _markAsInvalidCommand = new RelayCommand(MarkAsInvalid);
            _markAsValidCommand = new RelayCommand(MarkAsValid);
        }
        public ObservableCollection<TouristDTO> TouristsDTO
        {
            get { return _touristsDTO; }
            set
            {
                _touristsDTO = value;
                OnPropertyChanged();
            }
        }
        public TourDTO Tour
        {
            get { return _tourDTO; }
            set
            {
                _tourDTO = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowReviewDetailsCommand
        {
            get { return _showReviewDetailsCommand; }
            set
            {
                _showReviewDetailsCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand MarkAsInvalidCommand
        {
            get { return _markAsInvalidCommand; }
            set
            {
                _markAsInvalidCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand MarkAsValidCommand
        {
            get { return _markAsValidCommand; }
            set
            {
                _markAsValidCommand = value;
                OnPropertyChanged();
            }
        }
        /* public void MarkAsInvalid(object sender, RoutedEventArgs e)
         {
             CheckBox checkBox = sender as CheckBox;
             if (checkBox != null)
             {
                 TouristDTO tourist = checkBox.DataContext as TouristDTO;
                 if (tourist != null)
                 {
                     tourist.Review.IsNotValid = true;
                     _touristService.Update(tourist.ToTourist());
                     _tourReviewService.Update(tourist.Review.ToTourReview());
                 }
             }
         }*/

        public void MarkAsValid(object parameter)
        {
            TouristDTO tourist = parameter as TouristDTO;
            tourist.Review.IsNotValid = false;
            _touristService.Update(tourist.ToTourist());
            _tourReviewService.Update(tourist.Review.ToTourReview());
        }
        public void MarkAsInvalid(object parameter)
        {
            TouristDTO tourist = parameter as TouristDTO;
            tourist.Review.IsNotValid = true;
            _touristService.Update(tourist.ToTourist());
            _tourReviewService.Update(tourist.Review.ToTourReview());
        }
        private void ShowReviewDetails(object parameter)
        {
            TouristDTO selectedTourist = parameter as TouristDTO;
            if (selectedTourist.Review == null)
            {
                MessageBox.Show("There is no further information about this review", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            ReviewDetailsWindow reviewDetails = new ReviewDetailsWindow(selectedTourist);
            reviewDetails.Show();
        }
    }
}
