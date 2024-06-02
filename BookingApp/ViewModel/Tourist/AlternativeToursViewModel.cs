using BookingApp.Commands;
using BookingApp.DTO;
using BookingApp.InjectorNameSpace;
using BookingApp.Repository.Interfaces;
using BookingApp.Service;
using BookingApp.View;
using BookingApp.View.Tourist;
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
        private RelayCommand _showMyToursWindowCommand;
        private RelayCommand _showInboxWindowCommand;
        private RelayCommand _showFinishedToursWindowCommand;
        private RelayCommand _showVoucherWindowCommand;
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
            _showFinishedToursWindowCommand = new RelayCommand(ShowFinishedToursWindow);
            _showMyToursWindowCommand = new RelayCommand(ShowMyToursWindow);
            _showVoucherWindowCommand = new RelayCommand(ShowVoucherWindow);
            _showInboxWindowCommand = new RelayCommand(ShowinboxWindow);
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
                ShowTourReservationWindow();
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
        public RelayCommand ShowMyToursWindowCommand
        {
            get
            {
                return _showMyToursWindowCommand;
            }
            set
            {
                _showMyToursWindowCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ShowInboxWindowCommand
        {
            get
            {
                return _showInboxWindowCommand;
            }
            set
            {
                _showInboxWindowCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ShowFinishedToursWindowCommand
        {
            get
            {
                return _showFinishedToursWindowCommand;
            }
            set
            {
                _showFinishedToursWindowCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ShowVoucherWindowCommand
        {
            get
            {
                return _showVoucherWindowCommand;
            }
            set
            {
                _showVoucherWindowCommand = value;
                OnPropertyChanged();
            }
        }
        public void ShowFinishedToursWindow()
        {
            FinishedToursWindow finishedToursWindow = new FinishedToursWindow(_userDTO);
            finishedToursWindow.ShowDialog();
        }

        public void ShowMyToursWindow()
        {
            MyToursWindow myToursWindow = new MyToursWindow();
            myToursWindow.ShowDialog();
        }

        public void ShowVoucherWindow()
        {
            VoucherWindow voucherWindow = new VoucherWindow(_userDTO);
            voucherWindow.ShowDialog();
        }

        public void ShowinboxWindow()
        {
            InboxWindow inboxWindow = new InboxWindow(_userDTO);
            inboxWindow.ShowDialog();
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
