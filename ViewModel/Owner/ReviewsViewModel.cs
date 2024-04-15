using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Service;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BookingApp.ViewModel.Owner
{
    public class ReviewsViewModel : ViewModel
    {
        private AccommodationReservationService _accommodationReservationService;

        private ObservableCollection<AccommodationReservationDTO> _finishedAccommodationReservationsDTO;
        private ObservableCollection<AccommodationReservationDTO> _userAndOwnerReviewedAccommodationReservationsDTO;
        private double _averageRating;

        private RelayCommand _showSideMenuCommand;

        private UserDTO _loggedInUser;
        private AccommodationReservationDTO _selectedMyReviewDTO = null;
        private AccommodationReservationDTO _selectedUserReviewDTO = null;

        public ReviewsViewModel(UserDTO loggedInUser)
        {
            _accommodationReservationService = new AccommodationReservationService();
            List<AccommodationReservationDTO> finishedAccommodationReservationsList = _accommodationReservationService.GetFinishedAccommodationReservations(loggedInUser.ToUser()).Select(reservation => new AccommodationReservationDTO(reservation)).ToList();
            _finishedAccommodationReservationsDTO = new ObservableCollection<AccommodationReservationDTO>(finishedAccommodationReservationsList);

            List<AccommodationReservationDTO> _userReviewedAccommodationReservationsList = _accommodationReservationService.GetUserAndOwnerReviewedAccommodationReservations(loggedInUser.ToUser()).Select(reservation => new AccommodationReservationDTO(reservation)).ToList();
            _userAndOwnerReviewedAccommodationReservationsDTO = new ObservableCollection<AccommodationReservationDTO>(_userReviewedAccommodationReservationsList);

            _averageRating = _accommodationReservationService.GetAverageRating(loggedInUser.ToUser());

            _loggedInUser = loggedInUser;

            _showSideMenuCommand = new RelayCommand(ShowSideMenu);
        }

        public ObservableCollection<AccommodationReservationDTO> FinishedAccommodationReservationsDTO
        {
            get { return _finishedAccommodationReservationsDTO; }
            set
            {
                _finishedAccommodationReservationsDTO = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<AccommodationReservationDTO> UserAndOwnerReviewedAccommodationReservationsDTO
        {
            get { return _userAndOwnerReviewedAccommodationReservationsDTO; }
            set
            {
                _userAndOwnerReviewedAccommodationReservationsDTO = value;
                OnPropertyChanged();
            }
        }

        public double AverageRating
        {
            get { return _averageRating; }
            set
            {
                _averageRating = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ShowSideMenuCommand
        {
            get { return _showSideMenuCommand; }
            set
            {
                _showSideMenuCommand = value;
                OnPropertyChanged();
            }
        }
        public AccommodationReservationDTO SelectedMyReviewDTO
        {
            get { return _selectedMyReviewDTO; }
            set
            {
                _selectedMyReviewDTO = value;
                OnPropertyChanged();
                ShowMyReviewPage();
            }
        }

        public AccommodationReservationDTO SelectedUserReviewDTO
        {
            get { return _selectedUserReviewDTO; }
            set
            {
                _selectedUserReviewDTO = value;
                OnPropertyChanged();
                ShowUserReviewPage();
            }
        }

        public void ShowSideMenu()
        {
            OwnerMainWindow.SideMenuFrame.Content = new SideMenuPage();
        }

        private void ShowMyReviewPage()
        {
            if (SelectedMyReviewDTO == null)
            {
                return;
            }

            var selectedItem = SelectedMyReviewDTO as AccommodationReservationDTO;

            if (selectedItem.RatingDTO.OwnerCleannessRating == 0)
            {
                OwnerMainWindow.MainFrame.Content = new MyReviewNotRatedPage(selectedItem);
                SelectedMyReviewDTO = null;
            }
            else
            {
                OwnerMainWindow.MainFrame.Content = new MyReviewRatedPage(selectedItem);
                SelectedMyReviewDTO = null;
            }
        }

        private void ShowUserReviewPage()
        {
            if (_selectedUserReviewDTO == null)
            {
                return;
            }

            var selectedItem = _selectedUserReviewDTO as AccommodationReservationDTO;

            OwnerMainWindow.MainFrame.Content = new UserReviewDetailsPage(selectedItem);
            _selectedUserReviewDTO = null;
        }
    }
}
