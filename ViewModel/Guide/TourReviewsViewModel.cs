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

namespace BookingApp.ViewModel.Guide
{
    class TourReviewsViewModel : ViewModel
    {

        private TourDTO _tourDTO;
        private TourReservationService _tourReservationService;

        private RelayCommand _showReviewDetailsCommand;

        private ObservableCollection<TouristDTO> _touristsDTO { get; set; }
        public TourReviewsViewModel(TourDTO tour)
        {
            _tourDTO = tour;
            _tourReservationService = new TourReservationService();
            List<TouristDTO> touristsDTO = _tourReservationService.GetJoinedTourists(_tourDTO.ToTourAllParam()).Select(tourist => new TouristDTO(tourist)).ToList();
            _touristsDTO = new ObservableCollection<TouristDTO>(touristsDTO);
            _showReviewDetailsCommand = new RelayCommand(ShowReviewDetails);
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

        public RelayCommand ShowReviewDetailsCommand
        {
            get { return _showReviewDetailsCommand; }
            set
            {
                _showReviewDetailsCommand = value;
                OnPropertyChanged();
            }
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
