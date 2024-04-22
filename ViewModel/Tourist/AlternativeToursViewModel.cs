using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Tourist
{
    public class AlternativeToursViewModel:ViewModel
    {

        private TourDTO _tourDTO;

        private UserDTO _userDTO;

        private TourService _tourService;

        private TourReservationService _tourReservationService;

        private ObservableCollection<TourDTO> _toursDTO;

        private RelayCommand _showTourReservationWindowCommand;

        private TourDTO _selectedTourDTO { get; set; }
        public AlternativeToursViewModel(TourDTO tourDTO, UserDTO loggedInUser)
        {
            
            _tourDTO = tourDTO;
            _userDTO = loggedInUser;
            
            IUserRepository userRepository = Injector.CreateInstance<IUserRepository>();
            ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
            ITourReservationRepository tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            ITouristRepository touristRepository = Injector.CreateInstance<ITouristRepository>();
            ITourReviewRepository tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            IVoucherRepository voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            _tourReservationService = new TourReservationService(tourReservationRepository, userRepository, touristRepository, tourReviewRepository, voucherRepository);
            _tourService = new TourService(tourRepository, userRepository, touristRepository, tourReservationRepository, tourReviewRepository, voucherRepository);

            List<TourDTO> tours = _tourService.GetToursWithSameLocation(_tourDTO.ToTourAllParam()).Select(tours => new TourDTO(tours)).ToList();
            _toursDTO = new ObservableCollection<TourDTO>(tours);
            
            _showTourReservationWindowCommand = new RelayCommand(ShowTourReservationWindow);
        }

        public TourDTO TourDTO
        {
            get
            {
                return _tourDTO;
            }
            set
            {
                _tourDTO = value;
                OnPropertyChanged();
            }
        }

        public UserDTO UserDTO
        {
            get
            {
                return _userDTO;
            }
            set
            {
                _userDTO = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TourDTO> ToursDTO
        {
            get
            {
                return _toursDTO;
            }

            set
            {
                _toursDTO = value;
                OnPropertyChanged();

            }
        }

        public TourDTO SelectedTourDTO
        {
            get { return _selectedTourDTO; }
            set
            {
                _selectedTourDTO = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ShowTourReservationWindowCommand
        {
            get
            {
                return _showTourReservationWindowCommand;
            }
            set
            {
                _showTourReservationWindowCommand = value;
                OnPropertyChanged();
            }
        }

        public void ShowTourReservationWindow()
        {

            if (_selectedTourDTO == null)
            {
                return;
            }

            var selectedItem = _selectedTourDTO as TourDTO;
            TourReservationWindow tourReservationWindow = new TourReservationWindow(_tourReservationService, new TourDTO(selectedItem), _userDTO);
            tourReservationWindow.ShowDialog();
        }
    }
}
